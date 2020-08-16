namespace Core.Shared
{
    public struct PageCursor
    {
        public readonly uint Limit;
        public readonly uint Offset;

        public PageCursor(uint limit, uint offset)
        {
            this.Limit = limit;
            this.Offset = offset;
        }
    }
}