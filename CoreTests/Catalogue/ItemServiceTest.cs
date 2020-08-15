using System;
using System.Collections.Generic;
using Core.Catalogue;
using FluentAssertions;
using Xunit;

namespace CoreTests.Catalogue
{
    public class ItemServiceTests
    {
        [Trait("Category", "Unit")]
        public class GroupByVariant
        {
            [Fact]
            public void ItemWithoutVariants()
            {
                var item = new Item(
                    Item.Id.From("1"),
                    Item.Name.From("acoustic guitar"),
                    CatalogueStatus.NOT_FOR_SALE,
                    Source.CRAFTING,
                    Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                    null
                );
                var items = new List<Item>{
                    item
                };
                var service = new InMemoryItemService(items);

                var actual = service.GroupByVariants();

                actual.Should().ContainKeys(item.id);
                actual[item.id].Should().HaveCount(1);
                actual[item.id][0].id.Should().Be(item.id);
            }

            [Fact]
            public void ItemWithVariants()
            {
                var item1 = new Item(
                    Item.Id.From("1"),
                    Item.Name.From("acoustic guitar"),
                    CatalogueStatus.NOT_FOR_SALE,
                    Source.CRAFTING,
                    Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                    new Item.Variant(
                        Item.Variant.Id.From("0_0"),
                        Item.Variant.Name.From("Natural")
                    )
                );
                var item2 = new Item(
                    Item.Id.From("1"),
                    Item.Name.From("acoustic guitar"),
                    CatalogueStatus.NOT_FOR_SALE,
                    Source.CRAFTING,
                    Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                    new Item.Variant(
                        Item.Variant.Id.From("1_0"),
                        Item.Variant.Name.From("Natural")
                    )
                );
                var items = new List<Item>{
                item1,
                item2
            };
                var service = new InMemoryItemService(items);

                var actual = service.GroupByVariants();

                actual.Should().ContainKeys(item1.id);
                actual[item1.id].Should().HaveCount(2);
                actual[item1.id][0].id.Should().Be(item1.id);
                actual[item1.id][0].id.Should().Be(item2.id);
            }
        }


        [Trait("Category", "Unit")]
        public class Get
        {
            [Fact]
            public void ItemWithoutVariant()
            {
                var item = new Item(
                    Item.Id.From("1"),
                    Item.Name.From("acoustic guitar"),
                    CatalogueStatus.NOT_FOR_SALE,
                    Source.CRAFTING,
                    Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                    null
                );
                var items = new List<Item>{
                    item
                };
                var service = new InMemoryItemService(items);

                var actual = service.Get(item.id, null);

                actual.Should().Be(item);
            }

            [Fact]
            public void ItemWithVariant()
            {
                var item = new Item(
                    Item.Id.From("1"),
                    Item.Name.From("acoustic guitar"),
                    CatalogueStatus.NOT_FOR_SALE,
                    Source.CRAFTING,
                    Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                    new Item.Variant(
                        Item.Variant.Id.From("1_0"),
                        Item.Variant.Name.From("Natural")
                    )
                );
                var items = new List<Item>{
                    item
                };
                var service = new InMemoryItemService(items);

                var actual = service.Get(item.id, item.variant?.id);

                actual.Should().Be(item);
            }

            [Fact]
            public void ItemNotExists()
            {
                var item = new Item(
                    Item.Id.From("1"),
                    Item.Name.From("acoustic guitar"),
                    CatalogueStatus.NOT_FOR_SALE,
                    Source.CRAFTING,
                    Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                    new Item.Variant(
                        Item.Variant.Id.From("1_0"),
                        Item.Variant.Name.From("Natural")
                    )
                );
                var items = new List<Item>{
                    item
                };
                var service = new InMemoryItemService(items);


                FluentActions
                    .Invoking(() => service.Get(Item.Id.From("1234"), null))
                    .Should().Throw<InvalidOperationException>();
            }
        }
    }
}