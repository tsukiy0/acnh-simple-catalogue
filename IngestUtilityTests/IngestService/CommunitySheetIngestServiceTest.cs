using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task t()
        {
            var mock = SetupGetSheetService(new List<IList<object>>
            {
                    new List<object> { "Internal ID", "Name", "Catalog", "Image" },
                    new List<object> { "3821", "air circulator", "For sale", @"=IMAGE(""https://acnhcdn.com/latest/FtrIcon/FtrCirculator_Remake_2_0.png"")"},
            });
            var service = new CommunitySheetIngestService(mock.Object);

            var actual = await service.Ingest();

            actual.Should().HaveCount(1);
        }

        private Mock<IGetSheetService> SetupGetSheetService(IList<IList<object>> rows)
        {
            var mock = new Mock<IGetSheetService>();
            mock.Setup(_ => _.Get()).ReturnsAsync(rows);
            return mock;
        }
    }
}