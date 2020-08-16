using System;
using System.Collections.Generic;
using Core.Catalogue;
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
            Items = ItemService.List(filter);
        }
    }
}