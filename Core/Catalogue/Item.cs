using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Catalogue
{
    [JsonConverter(typeof(ItemConverter))]
    public struct Item
    {
        public readonly Id id;
        public readonly Name name;
        public readonly CatalogueStatus catalogueStatus;
        public readonly Image image;
        public readonly Variant? variant;

        public Item(Id id, Name name, CatalogueStatus catalogueStatus, Image image, Variant? variant)
        {
            this.id = id;
            this.name = name;
            this.catalogueStatus = catalogueStatus;
            this.image = image;
            this.variant = variant;
        }

        [JsonConverter(typeof(IdConverter))]
        public struct Id
        {
            private string value { get; }

            private Id(string value)
            {
                this.value = value;
            }

            public static Id From(string input)
            {
                return new Id(input);
            }

            override public string ToString()
            {
                return value;
            }
        }

        [JsonConverter(typeof(NameConverter))]
        public struct Name
        {
            private string value { get; }

            private Name(string value)
            {
                this.value = value;
            }

            public static Name From(string input)
            {
                return new Name(input);
            }

            override public string ToString()
            {
                return value;
            }
        }

        public class IdConverter : JsonConverter<Id>
        {
            public override Id Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return Id.From(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, Id value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        public class NameConverter : JsonConverter<Name>
        {
            public override Name Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return Name.From(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, Name value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        public class ItemConverter : JsonConverter<Item>
        {
            public override Item Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, Item value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("id");
                JsonSerializer.Serialize(writer, value.id, options);

                writer.WritePropertyName("name");
                JsonSerializer.Serialize(writer, value.name, options);

                writer.WritePropertyName("catalogueStatus");
                JsonSerializer.Serialize(writer, value.catalogueStatus, options);

                writer.WritePropertyName("image");
                JsonSerializer.Serialize(writer, value.image, options);

                writer.WritePropertyName("variant");
                JsonSerializer.Serialize(writer, value.variant, options);

                writer.WriteEndObject();
            }
        }


        [JsonConverter(typeof(VariantConverter))]
        public struct Variant
        {
            public readonly Id id;
            public readonly Name name;

            public Variant(Id id, Name name)
            {
                this.id = id;
                this.name = name;
            }

            [JsonConverter(typeof(IdConverter))]
            public struct Id
            {
                private readonly string value;

                private Id(string value)
                {
                    this.value = value;
                }

                public static Id From(string input)
                {
                    return new Id(input);
                }

                override public string ToString()
                {
                    return value;
                }
            }


            [JsonConverter(typeof(NameConverter))]
            public struct Name
            {
                private readonly string value;

                private Name(string value)
                {
                    this.value = value;
                }

                public static Name From(string input)
                {
                    return new Name(input);
                }

                override public string ToString()
                {
                    return value;
                }
            }

            public class IdConverter : JsonConverter<Id>
            {
                public override Id Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    return Id.From(reader.GetString());
                }

                public override void Write(Utf8JsonWriter writer, Id value, JsonSerializerOptions options)
                {
                    writer.WriteStringValue(value.ToString());
                }
            }

            public class NameConverter : JsonConverter<Name>
            {
                public override Name Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    return Name.From(reader.GetString());
                }

                public override void Write(Utf8JsonWriter writer, Name value, JsonSerializerOptions options)
                {
                    writer.WriteStringValue(value.ToString());
                }
            }

            public class VariantConverter : JsonConverter<Variant>
            {
                public override Variant Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    if (reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException();
                    }

                    Id? id = null;
                    Name? name = null;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject)
                        {
                            return new Variant((Id)id, (Name)name);
                        }

                        if (reader.TokenType != JsonTokenType.PropertyName)
                        {
                            throw new JsonException();
                        }

                        if (reader.GetString() == "id")
                        {
                            id = JsonSerializer.Deserialize<Id>(ref reader, options);
                        }

                        if (reader.GetString() == "name")
                        {
                            name = JsonSerializer.Deserialize<Name>(ref reader, options);
                        }
                    }

                    throw new JsonException();
                }

                public override void Write(Utf8JsonWriter writer, Variant value, JsonSerializerOptions options)
                {
                    writer.WriteStartObject();

                    writer.WritePropertyName("id");
                    JsonSerializer.Serialize(writer, value.id, options);

                    writer.WritePropertyName("name");
                    JsonSerializer.Serialize(writer, value.name, options);

                    writer.WriteEndObject();
                }
            }
        }
    }
}