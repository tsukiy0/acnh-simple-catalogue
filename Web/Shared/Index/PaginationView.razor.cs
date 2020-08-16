using Core.Shared;
using Microsoft.AspNetCore.Components;

namespace Web.Shared.Index
{
    public partial class PaginationView : ComponentBase
    {
        [Parameter]
        public PageCursor Cursor { get; set; }

        [Parameter]
        public uint Count { get; set; }

        [Parameter]
        public EventCallback<PageCursor> OnChange { get; set; }

        private int GetCurrent()
        {
            return (int)(Cursor.Offset / Cursor.Limit);
        }

        private int GetTotal()
        {
            return (int)(Count / Cursor.Limit);
        }

        private void OnChangePage(int page)
        {
            OnChange.InvokeAsync(new PageCursor(
                Cursor.Limit,
                (uint)page
            ));
        }
    }
}