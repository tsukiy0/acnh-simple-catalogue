using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Catalogue
{
    public class ItemConverter : JsonConverter<Item>
    {
        public override Item Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = JsonSerializer.Deserialize<ItemJson>(ref reader, options);

            return new Item(
                Item.Id.From(json.id),
                Item.Name.From(json.name),
                json.catalogueStatus,
                Image.From(json.image),
                null
            );
        }

        public override void Write(Utf8JsonWriter writer, Item value, JsonSerializerOptions options)
        {
            var json = new ItemJson()
            {
                id = value.id.ToString(),
                name = value.name.ToString(),
                catalogueStatus = value.catalogueStatus,
                image = value.image.ToString(),
                variant = null
            };

            JsonSerializer.Serialize(writer, json, options);
        }

        public struct ItemJson
        {
            public string id { get; set; }
            public string name { get; set; }
            public CatalogueStatus catalogueStatus { get; set; }
            public string image { get; set; }
            public VariantJson? variant { get; set; }

            public struct VariantJson
            {
                public string id { get; set; }
                public string name { get; set; }
            }
        }
    }
}