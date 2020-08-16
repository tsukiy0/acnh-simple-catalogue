namespace Core.Catalogue
{
    public struct ItemName
    {
        private string value { get; }

        private ItemName(string value)
        {
            this.value = value;
        }

        public static ItemName From(string input)
        {
            return new ItemName(input);
        }

        override public string ToString()
        {
            return value;
        }
    }
}