namespace Core.Catalogue
{
    public struct ItemId
    {
        private string value { get; }

        private ItemId(string value)
        {
            this.value = value;
        }

        public static ItemId From(string input)
        {
            return new ItemId(input);
        }

        override public string ToString()
        {
            return value;
        }
    }
}