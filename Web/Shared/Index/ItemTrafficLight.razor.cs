using Core.Catalogue;
using Microsoft.AspNetCore.Components;

namespace Web.Shared.Index
{
    public partial class ItemTrafficLight : ComponentBase
    {
        [Parameter]
        public Item Item { get; set; }
    }
}