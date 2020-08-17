using System.Collections.Generic;
using System.Linq;
using Core.Catalogue;
using Microsoft.AspNetCore.Components;

namespace Web.Shared.Index
{
    public partial class ItemFilterView : ComponentBase
    {
        [Parameter]
        public ItemFilter Model { get; set; }

        [Parameter]
        public EventCallback<ItemFilter> OnChange { get; set; }

        private void OnChangeSearch(string input)
        {
            OnChange.InvokeAsync(new ItemFilter(
                input,
                Model.CatalogueStatuses,
                Model.Sources
            ));
        }
    }
}