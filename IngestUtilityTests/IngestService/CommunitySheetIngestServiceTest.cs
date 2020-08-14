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
                    new List<object> { "Internal ID", "Name", "Catalog", "Image", "Variant ID", "Variant" },
                    new List<object> { "1317", "anatomical model", "For sale", @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/FtrAnatomicalmodel.png"")", "NA", "NA"},
            });
            var service = new CommunitySheetIngestService(mock.Object);

            var actual = await service.Ingest();

            actual.Should().HaveCount(1);
            actual[0].Equals(new Item(
                Item.Id.From("1317"),
                Item.Name.From("anatomical model"),
                CatalogueStatus.FOR_SALE,
                Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAnatomicalmodel.png"),
                null
            ));
        }

        [Fact]
        public async Task VariantItem()
        {
            var mock = SetupGetSheetService(new List<IList<object>>
            {
                    new List<object> { "Internal ID", "Name", "Catalog", "Image", "Variant ID", "Variation" },
                    new List<object> { "3821", "air circulator", "For sale", @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/FtrCirculator_Remake_2_0.png"")", "2_0", "Pink"},
            });
            var service = new CommunitySheetIngestService(mock.Object);

            var actual = await service.Ingest();

            actual.Should().HaveCount(1);
            actual[0].Equals(new Item(
                Item.Id.From("3821"),
                Item.Name.From("air circulator"),
                CatalogueStatus.FOR_SALE,
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
                    new List<object> { "Internal ID", "Name", "Catalog", "Image" },
                    new List<object> { "5036", "aqua tile flooring", "For sale", @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/RoomTexFloorTile01.png"")"},
            });
            var service = new CommunitySheetIngestService(mock.Object);

            var actual = await service.Ingest();

            actual.Should().HaveCount(1);
            actual[0].Equals(new Item(
                Item.Id.From("5036"),
                Item.Name.From("aqua tile flooring"),
                CatalogueStatus.FOR_SALE,
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
                    new List<object> { "Internal ID", "Name", "Catalog", "Image", "Variant ID", "Variation" },
                    new List<object> { "3821", "air circulator", input, @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/FtrCirculator_Remake_2_0.png"")", "2_0", "Pink"},
            });
            var service = new CommunitySheetIngestService(mock.Object);

            var actual = await service.Ingest();

            actual[0].catalogueStatus.Should().Be(output);
        }

        [Fact]
        public async Task UnknownCatalogueStatus()
        {
            var mock = SetupGetSheetService(new List<IList<object>>
            {
                    new List<object> { "Internal ID", "Name", "Catalog", "Image", "Variant ID", "Variation" },
                    new List<object> { "3821", "air circulator", "bad", @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/FtrCirculator_Remake_2_0.png"")", "2_0", "Pink"},
            });
            var service = new CommunitySheetIngestService(mock.Object);

            await FluentActions.Invoking(async () => await service.Ingest())
                .Should()
                .ThrowAsync<CommunitySheetIngestService.BadCatalogueStatusStringException>();
        }

        private Mock<IGetSheetService> SetupGetSheetService(IList<IList<object>> rows)
        {
            var mock = new Mock<IGetSheetService>();
            mock.Setup(_ => _.Get()).ReturnsAsync(rows);
            return mock;
        }
    }
}