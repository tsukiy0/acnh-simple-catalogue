using System.Text.Json.Serialization;

namespace Core.Catalogue
{
    [JsonConverter(typeof(ItemConverter))]
    public partial struct Item
    {
        public readonly ItemId Id;
        public readonly ItemName Name;
        public readonly CatalogueStatus catalogueStatus;
        public readonly Source Source;
        public readonly Image Image;
        public readonly ItemVariant? Variant;

        public Item(ItemId id, ItemName name, CatalogueStatus catalogueStatus, Source source, Image image, ItemVariant? variant)
        {
            this.Id = id;
            this.Name = name;
            this.catalogueStatus = catalogueStatus;
            this.Source = source;
            this.Image = image;
            this.Variant = variant;
        }
    }
}