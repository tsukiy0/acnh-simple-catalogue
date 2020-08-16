using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Catalogue;
using Core.Shared;
using Microsoft.AspNetCore.Components;

namespace Web.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        private IItemService ItemService { get; set; }

        private ItemFilter Filter = new ItemFilter("", new List<CatalogueStatus>(), new List<Source>());

        private List<Item> Items = new List<Item>();

        private void OnChangeFilter(ItemFilter filter)
        {
            Filter = filter;
            Items = ItemService.List(Filter, new PageCursor(100, 0));
        }
        protected override void OnInitialized()
        {
            Items = ItemService.List(Filter, new PageCursor(100, 0));
        }
    }
}