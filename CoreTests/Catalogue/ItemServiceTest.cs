using System;
using System.Collections.Generic;
using Core.Catalogue;
using Core.Shared;
using FluentAssertions;
using Xunit;

namespace CoreTests.Catalogue
{
    public class ItemServiceTests
    {
        [Trait("Category", "Unit")]
        public class List
        {
            [Fact]
            public void All()
            {
                var item = new Item(
                    ItemId.From("1"),
                    ItemName.From("acoustic guitar"),
                    CatalogueStatus.NOT_FOR_SALE,
                    Source.CRAFTING,
                    Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                    null
                );
                var items = new List<Item>{
                    item
                };
                var service = new InMemoryItemService(items);

                var actual = service.List(
                    new ItemFilter("", new List<CatalogueStatus>(), new List<Source>()),
                    new PageCursor(100, 0)
                );

                actual.Count.Should().Be(1);
                actual.Items.Should().HaveCount(1);
                actual.Items.Should().Contain(item);
            }

            [Fact]
            public void FilterByOneCatalogueStatus()
            {
                var items = new List<Item> {
                    new Item(
                        ItemId.From("1"),
                        ItemName.From("acoustic guitar"),
                        CatalogueStatus.NOT_FOR_SALE,
                        Source.CRAFTING,
                        Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                        null
                    ),
                    new Item(
                        ItemId.From("1"),
                        ItemName.From("formal paper"),
                        CatalogueStatus.FOR_SALE,
                        Source.CRAFTING,
                        Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                        null
                    )
                };
                var service = new InMemoryItemService(items);

                var actual = service.List(
                    new ItemFilter("", new List<CatalogueStatus> { CatalogueStatus.FOR_SALE }, new List<Source>()),
                    new PageCursor(100, 0)
                );

                actual.Count.Should().Be(1);
                actual.Items.Should().HaveCount(1);
                actual.Items.Should().Contain(items[1]);
            }

            [Fact]
            public void FilterByManyCatalogueStatus()
            {
                var items = new List<Item> {
                    new Item(
                        ItemId.From("1"),
                        ItemName.From("acoustic guitar"),
                        CatalogueStatus.NOT_FOR_SALE,
                        Source.CRAFTING,
                        Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                        null
                    ),
                    new Item(
                        ItemId.From("1"),
                        ItemName.From("formal paper"),
                        CatalogueStatus.FOR_SALE,
                        Source.CRAFTING,
                        Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                        null
                    )
                };
                var service = new InMemoryItemService(items);

                var actual = service.List(
                    new ItemFilter("", new List<CatalogueStatus> { CatalogueStatus.FOR_SALE, CatalogueStatus.NOT_FOR_SALE }, new List<Source>()),
                    new PageCursor(100, 0)
                );

                actual.Count.Should().Be(2);
                actual.Items.Should().HaveCount(2);
                actual.Items.Should().Contain(items);
            }

            [Fact]
            public void FilterByOneSource()
            {
                var items = new List<Item> {
                    new Item(
                        ItemId.From("1"),
                        ItemName.From("acoustic guitar"),
                        CatalogueStatus.FOR_SALE,
                        Source.CRAFTING,
                        Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                        null
                    ),
                    new Item(
                        ItemId.From("1"),
                        ItemName.From("formal paper"),
                        CatalogueStatus.FOR_SALE,
                        Source.NOOKS_CRANNY,
                        Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                        null
                    )
                };
                var service = new InMemoryItemService(items);

                var actual = service.List(
                    new ItemFilter("", new List<CatalogueStatus>(), new List<Source> { Source.NOOKS_CRANNY }),
                    new PageCursor(100, 0)
                );

                actual.Count.Should().Be(1);
                actual.Items.Should().HaveCount(1);
                actual.Items.Should().Contain(items[1]);
            }

            [Fact]
            public void FilterByManySource()
            {
                var items = new List<Item> {
                    new Item(
                        ItemId.From("1"),
                        ItemName.From("acoustic guitar"),
                        CatalogueStatus.FOR_SALE,
                        Source.CRAFTING,
                        Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                        null
                    ),
                    new Item(
                        ItemId.From("1"),
                        ItemName.From("formal paper"),
                        CatalogueStatus.FOR_SALE,
                        Source.NOOKS_CRANNY,
                        Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                        null
                    )
                };
                var service = new InMemoryItemService(items);

                var actual = service.List(
                    new ItemFilter("", new List<CatalogueStatus>(), new List<Source> { Source.NOOKS_CRANNY, Source.CRAFTING }),
                    new PageCursor(100, 0)
                );

                actual.Count.Should().Be(2);
                actual.Items.Should().HaveCount(2);
                actual.Items.Should().Contain(items);
            }
        }

        [Theory]
        [InlineData("acoustic")]
        [InlineData("guitar")]
        [InlineData("ACOUSTIC GUITAR")]
        public void FilterBySearch(string search)
        {
            var items = new List<Item> {
                    new Item(
                        ItemId.From("1"),
                        ItemName.From("acoustic guitar"),
                        CatalogueStatus.FOR_SALE,
                        Source.CRAFTING,
                        Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                        null
                    ),
                    new Item(
                        ItemId.From("1"),
                        ItemName.From("formal paper"),
                        CatalogueStatus.FOR_SALE,
                        Source.NOOKS_CRANNY,
                        Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                        null
                    )
                };
            var service = new InMemoryItemService(items);

            var actual = service.List(
                new ItemFilter(search, new List<CatalogueStatus>(), new List<Source>()),
                new PageCursor(100, 0)
            );

            actual.Count.Should().Be(1);
            actual.Items.Should().HaveCount(1);
            actual.Items.Should().Contain(items[0]);
        }
    }

    [Trait("Category", "Unit")]
    public class GroupByVariant
    {
        [Fact]
        public void ItemWithoutVariants()
        {
            var item = new Item(
                ItemId.From("1"),
                ItemName.From("acoustic guitar"),
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

            actual.Should().ContainKeys(item.Id);
            actual[item.Id].Should().HaveCount(1);
            actual[item.Id][0].Id.Should().Be(item.Id);
        }

        [Fact]
        public void ItemWithVariants()
        {
            var item1 = new Item(
                ItemId.From("1"),
                ItemName.From("acoustic guitar"),
                CatalogueStatus.NOT_FOR_SALE,
                Source.CRAFTING,
                Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                new ItemVariant(
                    ItemVariantId.From("0_0"),
                    ItemVariantName.From("Natural")
                )
            );
            var item2 = new Item(
                ItemId.From("1"),
                ItemName.From("acoustic guitar"),
                CatalogueStatus.NOT_FOR_SALE,
                Source.CRAFTING,
                Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                new ItemVariant(
                    ItemVariantId.From("1_0"),
                    ItemVariantName.From("Natural")
                )
            );
            var items = new List<Item>{
                item1,
                item2
            };
            var service = new InMemoryItemService(items);

            var actual = service.GroupByVariants();

            actual.Should().ContainKeys(item1.Id);
            actual[item1.Id].Should().HaveCount(2);
            actual[item1.Id][0].Id.Should().Be(item1.Id);
            actual[item1.Id][0].Id.Should().Be(item2.Id);
        }
    }


    [Trait("Category", "Unit")]
    public class Get
    {
        [Fact]
        public void ItemWithoutVariant()
        {
            var item = new Item(
                ItemId.From("1"),
                ItemName.From("acoustic guitar"),
                CatalogueStatus.NOT_FOR_SALE,
                Source.CRAFTING,
                Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                null
            );
            var items = new List<Item>{
                    item
                };
            var service = new InMemoryItemService(items);

            var actual = service.Get(item.Id, null);

            actual.Should().Be(item);
        }

        [Fact]
        public void ItemWithVariant()
        {
            var item = new Item(
                ItemId.From("1"),
                ItemName.From("acoustic guitar"),
                CatalogueStatus.NOT_FOR_SALE,
                Source.CRAFTING,
                Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                new ItemVariant(
                    ItemVariantId.From("1_0"),
                    ItemVariantName.From("Natural")
                )
            );
            var items = new List<Item>{
                    item
                };
            var service = new InMemoryItemService(items);

            var actual = service.Get(item.Id, item.Variant?.Id);

            actual.Should().Be(item);
        }

        [Fact]
        public void ItemNotExists()
        {
            var item = new Item(
                ItemId.From("1"),
                ItemName.From("acoustic guitar"),
                CatalogueStatus.NOT_FOR_SALE,
                Source.CRAFTING,
                Image.From("https://acnhcdn.com/latest/FtrIcon/FtrAcorsticguitar_Remake_0_0.png"),
                new ItemVariant(
                    ItemVariantId.From("1_0"),
                    ItemVariantName.From("Natural")
                )
            );
            var items = new List<Item>{
                    item
                };
            var service = new InMemoryItemService(items);


            FluentActions
                .Invoking(() => service.Get(ItemId.From("1234"), null))
                .Should().Throw<InvalidOperationException>();
        }
    }
}