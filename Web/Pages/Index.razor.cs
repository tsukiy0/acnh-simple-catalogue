using System.Collections.Generic;
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

        private PageCursor Cursor = new PageCursor(36, 0);

        private Page<Item> Page = new Page<Item>(
            new List<Item>(),
            0
        );

        private void OnChangeFilter(ItemFilter filter)
        {
            Filter = filter;
            Page = ItemService.List(Filter, Cursor);
        }

        private void OnChangeCursor(PageCursor cursor)
        {
            Cursor = cursor;
            Page = ItemService.List(Filter, Cursor);
        }

        protected override void OnInitialized()
        {
            Page = ItemService.List(Filter, Cursor);
        }
    }
}