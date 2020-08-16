using System.Collections.Generic;
using System.Linq;
using Core.Shared;

namespace Core.Catalogue
{
    public interface IItemService
    {
        List<Item> List(ItemFilter filter, PageCursor cursor);
    }

    public class InMemoryItemService : IItemService
    {
        private readonly List<Item> items;

        public InMemoryItemService(List<Item> items)
        {
            this.items = items;
        }

        public List<Item> List(ItemFilter filter, PageCursor cursor)
        {
            return items
                .Where(_ => filter.CatalogueStatuses.Count == 0 || filter.CatalogueStatuses.Contains(_.catalogueStatus))
                .Where(_ => filter.Sources.Count == 0 || filter.Sources.Contains(_.source))
                .Where(_ => filter.Search.Count() == 0 || _.name.ToString().ToLower().Contains(filter.Search.ToLower()))
                .Skip((int)cursor.Offset)
                .Take((int)cursor.Limit)
                .ToList();
        }

        public Dictionary<Item.Id, List<Item>> GroupByVariants()
        {
            return items
                .GroupBy(_ => _.id)
                .ToDictionary(_ => _.Key, _ => _.ToList());
        }

        public Item Get(Item.Id id, Item.Variant.Id? variantId)
        {
            return items.Single(item =>
            {
                var hasMatchingId = id.Equals(item.id);
                var hasNoVariant = !variantId.HasValue && !item.variant.HasValue;
                var hasMatchingVariant = hasNoVariant || variantId
                    .Select(_ => _.Equals(item.variant?.id))
                    .GetValueOrDefault(false);

                return hasMatchingId && hasMatchingVariant;
            });
        }

        public List<Item> Get(Item.Id id)
        {
            return GroupByVariants()[id];
        }

        private Dictionary<Item.Id, List<Item>> GroupByVariants(List<Item> items)
        {
            return items
                .GroupBy(_ => _.id)
                .ToDictionary(_ => _.Key, _ => _.ToList());
        }

        public bool IsAKeeper(Item item)
        {
            var isCraftable = item.source == Source.CRAFTING;
            var isForSale = item.catalogueStatus == CatalogueStatus.FOR_SALE;

            return !isCraftable && !isForSale;
        }
    }
}