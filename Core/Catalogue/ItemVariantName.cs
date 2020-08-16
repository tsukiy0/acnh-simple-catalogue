namespace Core.Catalogue
{
    public struct ItemVariantName
    {
        private readonly string value;

        private ItemVariantName(string value)
        {
            this.value = value;
        }

        public static ItemVariantName From(string input)
        {
            return new ItemVariantName(input);
        }

        override public string ToString()
        {
            return value;
        }
    }
}