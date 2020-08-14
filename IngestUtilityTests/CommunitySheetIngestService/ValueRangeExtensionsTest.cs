using System;
using System.Collections.Generic;
using FluentAssertions;
using Google.Apis.Sheets.v4.Data;
using IngestUtility.CommunitySheetIngestService;
using Moq;
using Xunit;

namespace IngestUtilityTests.CommunitySheetIngestService
{
    [Trait("Category", "Unit")]
    public class ValueRangeExtensionsTest
    {
        [Fact]
        public void ZipHeaderRowWithValueRows()
        {
            var valueRangeMock = new Mock<ValueRange>();
            valueRangeMock.SetupGet(_ => _.Values).Returns(new List<IList<object>> {
                new List<object> { "k1", "k2" },
                new List<object> { "v1", "v2"},
                new List<object> { "v3", "v4"}
            });

            var actual = valueRangeMock.Object.ToListDictionary();

            actual.Should().HaveCount(2);
            actual[0]["k1"].Should().Be("v1");
            actual[0]["k2"].Should().Be("v2");
            actual[1]["k1"].Should().Be("v3");
            actual[1]["k2"].Should().Be("v4");
        }

        [Fact]
        public void ZipOnlyExistingValues()
        {
            var valueRangeMock = new Mock<ValueRange>();
            valueRangeMock.SetupGet(_ => _.Values).Returns(new List<IList<object>> {
                new List<object> { "k1", "k2" },
                new List<object> { "v1" },
                new List<object> { "v2" }
            });

            var actual = valueRangeMock.Object.ToListDictionary();

            actual.Should().HaveCount(2);
            actual[0]["k1"].Should().Be("v1");
            actual[1]["k1"].Should().Be("v2");
            FluentActions.Invoking(() => actual[0]["k2"]).Should().Throw<KeyNotFoundException>();
            FluentActions.Invoking(() => actual[1]["k2"]).Should().Throw<KeyNotFoundException>();
        }
    }
}
