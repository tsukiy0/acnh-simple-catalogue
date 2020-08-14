using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Catalogue;
using Core.Shared;
using System.Text.RegularExpressions;

namespace IngestUtility.IngestService
{
    public class CommunitySheetIngestService : IIngestService
    {
        public readonly IGetSheetService getSheetService;

        public CommunitySheetIngestService(IGetSheetService getSheetService)
        {
            this.getSheetService = getSheetService;
        }

        public async Task<IList<Item>> Ingest()
        {
            var result = await getSheetService.Get();

            return result
                .ToListDictionary()
                .Select(ToItem)
                .ToList();
        }

        private Item ToItem(Dictionary<string, Object> row)
        {
            return new Item(
                Item.Id.From(row["Internal ID"].ToString()),
                Item.Name.From(row["Name"].ToString()),
                CatalogueStatusFromString(row["Catalog"].ToString()),
                ImageFromString(row["Image"].ToString()),
                null
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

        private Image ImageFromString(string input)
        {
            var pattern = new Regex(@"^=IMAGE\(""(?<url>.+)""\)$");

            var matches = pattern.Match(input);

            return Image.From(matches.Groups["url"].Value);
        }
    }
}