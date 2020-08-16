using Core.Catalogue;
using Microsoft.AspNetCore.Components;

namespace Web.Shared.Index
{
    public partial class ItemCard
    {
        [Parameter]
        public Item Item { get; set; }
    }
}