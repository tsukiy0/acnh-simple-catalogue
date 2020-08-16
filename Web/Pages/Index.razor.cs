using System.Collections.Generic;
using Core.Catalogue;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Web.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        private IItemService itemService { get; set; }

        private ItemFilter filter;

        private List<Item> items = new List<Item>();

        private void OnChangeFilter(ItemFilter filter)
        {
            this.filter = filter;
        }

        private void OnApplyFilters(EditContext editContext)
        {
            items = itemService.List(filter);
        }
    }
}