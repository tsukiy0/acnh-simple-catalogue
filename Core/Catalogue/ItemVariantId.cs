namespace Core.Catalogue
{
    public struct ItemVariantId
    {
        private readonly string value;

        private ItemVariantId(string value)
        {
            this.value = value;
        }

        public static ItemVariantId From(string input)
        {
            return new ItemVariantId(input);
        }

        override public string ToString()
        {
            return value;
        }
    }
}