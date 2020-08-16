using System.Collections.Generic;

namespace Core.Shared
{
    public struct Page<T>
    {
        public readonly IList<T> Items;
        public readonly uint Count;

        public Page(IList<T> items, uint count)
        {
            this.Items = items;
            this.Count = count;
        }
    }
}