using System.Collections.Generic;

namespace Core.Catalogue
{
    public struct ItemFilter
    {
        public readonly string Search;
        public readonly List<CatalogueStatus> CatalogueStatuses;
        public readonly List<Source> Sources;

        public ItemFilter(string search, List<CatalogueStatus> catalogueStatuses, List<Source> sources)
        {
            this.Search = search;
            this.CatalogueStatuses = catalogueStatuses;
            this.Sources = sources;
        }
    }
}