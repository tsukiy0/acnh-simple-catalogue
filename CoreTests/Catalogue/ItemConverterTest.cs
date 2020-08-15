using FluentAssertions;
using Xunit;
using System.Text.Json;
using Core.Catalogue;

namespace CoreTests.Catalogue
{
    [Trait("Category", "Unit")]
    public class ItemConverterTest
    {
        [Fact]
        public void Deserialize()
        {
            var actual = JsonSerializer.Deserialize<Item>(@"{""id"":""383"",""name"":""acoustic guitar"",""catalogueStatus"":1,""source"":1,""image"":""https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"",""variant"":{""id"":""0_0"",""name"":""Natural""}}");

            actual.Should().Be(new Item(
                Item.Id.From("383"),
                Item.Name.From("acoustic guitar"),
                CatalogueStatus.NOT_FOR_SALE,
                Source.CRAFTING,
                Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                new Item.Variant(
                    Item.Variant.Id.From("0_0"),
                    Item.Variant.Name.From("Natural")
                )
            ));
        }

        [Fact]
        public void Serialize()
        {
            var actual = JsonSerializer.Serialize(new Item(
                Item.Id.From("383"),
                Item.Name.From("acoustic guitar"),
                CatalogueStatus.NOT_FOR_SALE,
                Source.CRAFTING,
                Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                new Item.Variant(
                    Item.Variant.Id.From("0_0"),
                    Item.Variant.Name.From("Natural")
                )
            ));

            actual.Should().Be(@"{""id"":""383"",""name"":""acoustic guitar"",""catalogueStatus"":1,""source"":1,""image"":""https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"",""variant"":{""id"":""0_0"",""name"":""Natural""}}");
        }
    }
}
