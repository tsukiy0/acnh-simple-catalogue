using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Catalogue
{
    [JsonConverter(typeof(ImageJsonConverter))]
    public struct Image
    {
        private Uri value { get; }

        private Image(Uri value)
        {
            this.value = value;
        }

        public static Image From(Uri input)
        {
            return new Image(input);
        }

        public static Image From(string input)
        {
            return new Image(new Uri(input));
        }

        public Uri ToUri()
        {
            return value;
        }

        override public string ToString()
        {
            return value.ToString();
        }
    }

    public class ImageJsonConverter : JsonConverter<Image>
    {
        public override Image Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Image.From(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, Image value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}