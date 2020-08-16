namespace Core.Catalogue
{
    public partial struct ItemVariant
    {
        public readonly ItemVariantId Id;
        public readonly ItemVariantName Name;

        public ItemVariant(ItemVariantId id, ItemVariantName name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}