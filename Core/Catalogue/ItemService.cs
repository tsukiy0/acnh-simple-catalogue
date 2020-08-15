using System.Collections.Generic;
using System.Linq;
using Core.Shared;

namespace Core.Catalogue
{
    public interface IItemService
    {
    }

    public class InMemoryItemService : IItemService
    {
        private readonly List<Item> items;

        public InMemoryItemService(List<Item> items)
        {
            this.items = items;
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
    }
}