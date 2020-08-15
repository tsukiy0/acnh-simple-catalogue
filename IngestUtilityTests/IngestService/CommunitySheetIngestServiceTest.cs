using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Catalogue;
using FluentAssertions;
using IngestUtility.IngestService;
using Moq;
using Xunit;

namespace IngestUtilityTests.IngestService
{
    [Trait("Category", "Unit")]
    public class CommunitySheetIngestServiceTest
    {
        [Fact]
        public async Task NonVariantItem()
        {
            var mock = SetupGetSheetService(new List<IList<object>>
            {
                    new List<object> { "Internal ID", "Name", "Catalog", "Source", "Image", "Variant ID", "Variantion" },
                    new List<object> { "1317", "anatomical model", "For sale", "Nook's Cranny", @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/FtrAnatomicalmodel.png"")", "NA", "NA"},
            });
            var service = new CommunitySheetIngestService(new List<IGetSheetService> { mock.Object });

            var actual = await service.Ingest();

            actual.Should().HaveCount(1);
            actual.Should().Contain(new Item(
                Item.Id.From("1317"),
                Item.Name.From("anatomical model"),
                CatalogueStatus.FOR_SALE,
                Source.NOOKS_CRANNY,
                Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAnatomicalmodel.png"),
                null
            ));
        }

        [Fact]
        public async Task VariantItem()
        {
            var mock = SetupGetSheetService(new List<IList<object>>
            {
                    new List<object> { "Internal ID", "Name", "Catalog", "Source", "Image", "Variant ID", "Variation" },
                    new List<object> { "3821", "air circulator", "For sale", "Nook's Cranny", @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/FtrCirculator_Remake_2_0.png"")", "2_0", "Pink"},
            });
            var service = new CommunitySheetIngestService(new List<IGetSheetService> { mock.Object });

            var actual = await service.Ingest();

            actual.Should().HaveCount(1);
            actual.Should().Contain(new Item(
                Item.Id.From("3821"),
                Item.Name.From("air circulator"),
                CatalogueStatus.FOR_SALE,
                Source.NOOKS_CRANNY,
                Image.From("https://acnhcdn.com/latest/FtrIcon/FtrCirculator_Remake_2_0.png"),
                new Item.Variant(
                    Item.Variant.Id.From("2_0"),
                    Item.Variant.Name.From("Pink")
                )
            ));
        }

        [Fact]
        public async Task NullVariantItem()
        {
            var mock = SetupGetSheetService(new List<IList<object>>
            {
                    new List<object> { "Internal ID", "Name", "Catalog", "Source", "Image" },
                    new List<object> { "5036", "aqua tile flooring", "For sale", "Nook's Cranny", @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/RoomTexFloorTile01.png"")"},
            });
            var service = new CommunitySheetIngestService(new List<IGetSheetService> { mock.Object });

            var actual = await service.Ingest();

            actual.Should().HaveCount(1);
            actual.Should().Contain(new Item(
                Item.Id.From("5036"),
                Item.Name.From("aqua tile flooring"),
                CatalogueStatus.FOR_SALE,
                Source.NOOKS_CRANNY,
                Image.From("https://acnhcdn.com/latest/FtrIcon/RoomTexFloorTile01.png"),
                null
            ));
        }

        [Theory]
        [InlineData("For sale", CatalogueStatus.FOR_SALE)]
        [InlineData("Not for sale", CatalogueStatus.NOT_FOR_SALE)]
        [InlineData("Not in catalog", CatalogueStatus.NOT_EXIST)]
        public async Task KnownCatalogueStatus(string input, CatalogueStatus output)
        {
            var mock = SetupGetSheetService(new List<IList<object>>
            {
                    new List<object> { "Internal ID", "Name", "Catalog", "Source", "Image", "Variant ID", "Variation" },
                    new List<object> { "3821", "air circulator", input, "Nook's Cranny", @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/FtrCirculator_Remake_2_0.png"")", "2_0", "Pink"},
            });
            var service = new CommunitySheetIngestService(new List<IGetSheetService> { mock.Object });

            var actual = await service.Ingest();

            actual.Should().ContainSingle(_ => _.catalogueStatus == output);
        }

        // [Fact]
        // public async Task UnknownCatalogueStatus()
        // {
        //     var mock = SetupGetSheetService(new List<IList<object>>
        //     {
        //             new List<object> { "Internal ID", "Name", "Catalog", "Source", "Image", "Variant ID", "Variation" },
        //             new List<object> { "3821", "air circulator", "bad", "Nook's Cranny", @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/FtrCirculator_Remake_2_0.png"")", "2_0", "Pink"},
        //     });
        //     var service = new CommunitySheetIngestService(new List<IGetSheetService> { mock.Object });

        //     await FluentActions.Invoking(async () => await service.Ingest())
        //         .Should()
        //         .ThrowAsync<CommunitySheetIngestService.BadCatalogueStatusStringException>();
        // }

        [Fact]
        public async Task AlbumImage()
        {
            var mock = SetupGetSheetService(new List<IList<object>>
            {
                    new List<object> { "Internal ID", "Name", "Catalog", "Source", "Album Image" },
                    new List<object> { "3200", "K.K. Fusion", "For sale", "K.K. concert; Nook Shopping Catalog", @"=IMAGE(""https://acnhcdn.com/latest/Audio/mjk_Fusion.png"")"},
            });
            var service = new CommunitySheetIngestService(new List<IGetSheetService> { mock.Object });

            var actual = await service.Ingest();

            actual.Should().HaveCount(1);
            actual.Should().Contain(new Item(
                Item.Id.From("3200"),
                Item.Name.From("K.K. Fusion"),
                CatalogueStatus.FOR_SALE,
                Source.UNKNOWN,
                Image.From("https://acnhcdn.com/latest/Audio/mjk_Fusion.png"),
                null
            ));
        }

        [Theory]
        [InlineData("Nook's Cranny", Source.NOOKS_CRANNY)]
        [InlineData("Crafting", Source.CRAFTING)]
        [InlineData("Mom", Source.UNKNOWN)]
        public async Task KnownSource(string input, Source output)
        {
            var mock = SetupGetSheetService(new List<IList<object>>
            {
                    new List<object> { "Internal ID", "Name", "Catalog", "Source", "Image", "Variant ID", "Variation" },
                    new List<object> { "3821", "air circulator", "For sale", input, @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/FtrCirculator_Remake_2_0.png"")", "2_0", "Pink"},
            });
            var service = new CommunitySheetIngestService(new List<IGetSheetService> { mock.Object });

            var actual = await service.Ingest();

            actual.Should().ContainSingle(_ => _.source == output);
        }

        [Fact]
        public async Task MultipleSheets()
        {
            var mock1 = SetupGetSheetService(new List<IList<object>>
            {
                    new List<object> { "Internal ID", "Name", "Catalog", "Source", "Image", "Variant ID", "Variation" },
                    new List<object> { "1317", "anatomical model", "For sale", "Nook's Cranny", @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/FtrAnatomicalmodel.png"")", "NA", "NA"},
            });
            var mock2 = SetupGetSheetService(new List<IList<object>>
            {
                    new List<object> { "Internal ID", "Name", "Catalog", "Source", "Image", "Variant ID", "Variation" },
                    new List<object> { "3821", "air circulator", "For sale", "Nook's Cranny", @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/FtrCirculator_Remake_2_0.png"")", "2_0", "Pink"},
            });
            var service = new CommunitySheetIngestService(new List<IGetSheetService> { mock1.Object, mock2.Object });

            var actual = await service.Ingest();

            actual.Should().HaveCount(2);
            actual.Should().Contain(new Item(
                Item.Id.From("1317"),
                Item.Name.From("anatomical model"),
                CatalogueStatus.FOR_SALE,
                Source.NOOKS_CRANNY,
                Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAnatomicalmodel.png"),
                null
            ));
            actual.Should().Contain(new Item(
                Item.Id.From("3821"),
                Item.Name.From("air circulator"),
                CatalogueStatus.FOR_SALE,
                Source.NOOKS_CRANNY,
                Image.From("https://acnhcdn.com/latest/FtrIcon/FtrCirculator_Remake_2_0.png"),
                new Item.Variant(
                    Item.Variant.Id.From("2_0"),
                    Item.Variant.Name.From("Pink")
                )
            ));
        }

        private Mock<IGetSheetService> SetupGetSheetService(IList<IList<object>> rows)
        {
            var mock = new Mock<IGetSheetService>();
            mock.Setup(_ => _.Get()).ReturnsAsync(rows);
            return mock;
        }
    }
}