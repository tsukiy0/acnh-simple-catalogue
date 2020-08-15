using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Shared;

namespace Core.Catalogue
{
    public class ItemConverter : JsonConverter<Item>
    {
        public override Item Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = JsonSerializer.Deserialize<ItemJson>(ref reader, options);
            return json.To();
        }

        public override void Write(Utf8JsonWriter writer, Item value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, ItemJson.From(value), options);
        }

        public struct ItemJson
        {
            public string id { get; set; }
            public string name { get; set; }
            public CatalogueStatus catalogueStatus { get; set; }
            public string image { get; set; }
            public VariantJson? variant { get; set; }

            public static ItemJson From(Item input)
            {
                return new ItemJson()
                {
                    id = input.id.ToString(),
                    name = input.name.ToString(),
                    catalogueStatus = input.catalogueStatus,
                    image = input.image.ToString(),
                    variant = input.variant.Select(VariantJson.From)
                };
            }

            public Item To()
            {
                return new Item(
                    Item.Id.From(id),
                    Item.Name.From(name),
                    catalogueStatus,
                    Image.From(image),
                    variant.Select(_ => _.To())
                );
            }

            public struct VariantJson
            {
                public string id { get; set; }
                public string name { get; set; }

                public static VariantJson From(Item.Variant input)
                {
                    return new VariantJson()
                    {
                        id = input.id.ToString(),
                        name = input.name.ToString(),
                    };
                }

                public Item.Variant To()
                {
                    return new Item.Variant(
                        Item.Variant.Id.From(id),
                        Item.Variant.Name.From(name)
                    );
                }
            }
        }
    }
}