using System.Collections.Generic;

namespace Core.Catalogue
{
    public struct ItemFilter
    {
        public readonly string Search;
        public readonly IList<CatalogueStatus> CatalogueStatuses;
        public readonly IList<Source> Sources;

        public ItemFilter(string search, IList<CatalogueStatus> catalogueStatuses, IList<Source> sources)
        {
            this.Search = search;
            this.CatalogueStatuses = catalogueStatuses;
            this.Sources = sources;
        }
    }
}