using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Catalogue;
using Core.Shared;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace IngestUtility.IngestService
{
    public class CommunitySheetIngestService : IIngestService
    {
        private readonly IList<IGetSheetService> getSheetServices;

        public CommunitySheetIngestService(IList<IGetSheetService> getSheetServices)
        {
            this.getSheetServices = getSheetServices;
        }

        public async Task<IList<Item>> Ingest()
        {
            var results = new List<IList<Item>>(await Task.WhenAll(getSheetServices.Select(IngestOne)));
            return results.SelectMany(_ => _).ToList();
        }

        private async Task<IList<Item>> IngestOne(IGetSheetService getSheetService)
        {
            var result = await getSheetService.Get();

            return result
                .ToListDictionary()
                .Aggregate(new List<Item>(), (acc, next) =>
                {
                    var item = ToItem(next);
                    if (item.HasValue)
                    {
                        acc.Add(item.Value);
                    }

                    return acc;
                })
                .ToList();
        }

        private Item? ToItem(IDictionary<string, Object> row)
        {
            try
            {
                return new Item(
                    Item.Id.From(row["Internal ID"].ToString()),
                    Item.Name.From(row["Name"].ToString()),
                    CatalogueStatusFromString(row["Catalog"].ToString()),
                    SourceFromString(row["Source"].ToString()),
                    GetImage(row),
                    GetVariant(row)
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(JsonSerializer.Serialize(row));
                Console.WriteLine(e);
                return null;
            }
        }

        public class ImageKeyNotFound : BaseException { }

        private Image GetImage(IDictionary<string, Object> row)
        {
            var concrete = row.ToDictionary(_ => _.Key, _ => _.Value);
            var albumImage = concrete.GetValueOrDefault("Album Image", null);
            var image = concrete.GetValueOrDefault("Image", null);

            if (albumImage != null)
            {
                return ImageFromString(albumImage.ToString());
            }

            if (image != null)
            {
                return ImageFromString(image.ToString());
            }

            throw new ImageKeyNotFound();
        }

        private Item.Variant? GetVariant(IDictionary<string, Object> row)
        {
            var variantId = row
                .ToDictionary(_ => _.Key, _ => _.Value)
                .GetValueOrDefault("Variant ID", null);

            if (variantId == null)
            {
                return null;
            }

            if (variantId.ToString() == "NA")
            {
                return null;
            }

            return new Item.Variant(
                Item.Variant.Id.From(variantId.ToString()),
                Item.Variant.Name.From(row["Variation"].ToString())
            );
        }

        public class BadCatalogueStatusStringException : BaseException { }

        private CatalogueStatus CatalogueStatusFromString(string input)
        {
            switch (input)
            {
                case "For sale":
                    return CatalogueStatus.FOR_SALE;
                case "Not for sale":
                    return CatalogueStatus.NOT_FOR_SALE;
                case "Not in catalog":
                    return CatalogueStatus.NOT_EXIST;
                default:
                    throw new BadCatalogueStatusStringException();
            }
        }

        private Source SourceFromString(string input)
        {
            switch (input)
            {
                case "Crafting":
                    return Source.CRAFTING;
                case "Nook's Cranny":
                    return Source.NOOKS_CRANNY;
                default:
                    return Source.UNKNOWN;
            }
        }

        private Image ImageFromString(string input)
        {
            var pattern = new Regex(@"^=IMAGE\(""(?<url>.+)""\)$");

            var matches = pattern.Match(input);

            return Image.From(matches.Groups["url"].Value);
        }
    }
}