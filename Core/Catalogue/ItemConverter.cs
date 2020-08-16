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
            public Source source { get; set; }
            public string image { get; set; }
            [JsonConverter(typeof(NullableStructSerializerFactory))]
            public VariantJson? variant { get; set; }

            public static ItemJson From(Item input)
            {
                return new ItemJson()
                {
                    id = input.Id.ToString(),
                    name = input.Name.ToString(),
                    catalogueStatus = input.catalogueStatus,
                    source = input.Source,
                    image = input.Image.ToString(),
                    variant = input.Variant.Select(_ => VariantJson.From(_))
                };
            }

            public Item To()
            {
                return new Item(
                    ItemId.From(id),
                    ItemName.From(name),
                    catalogueStatus,
                    source,
                    Image.From(image),
                    variant.Select(_ => _.To())
                );
            }

            public struct VariantJson
            {
                public string id { get; set; }
                public string name { get; set; }

                public static VariantJson From(ItemVariant input)
                {
                    return new VariantJson()
                    {
                        id = input.Id.ToString(),
                        name = input.Name.ToString(),
                    };
                }

                public ItemVariant To()
                {
                    return new ItemVariant(
                        ItemVariantId.From(id),
                        ItemVariantName.From(name)
                    );
                }
            }
        }
    }
}