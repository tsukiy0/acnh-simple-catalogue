using System;
using System.Text.Json;
using System.Text.Json.Serialization;

// https://github.com/dotnet/runtime/issues/30843#issuecomment-555395612
public class NullableStructSerializerFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
            return false;

        if (!typeToConvert.IsGenericType || typeToConvert.GetGenericTypeDefinition() != typeof(Nullable<>))
            return false;

        var structType = typeToConvert.GenericTypeArguments[0];
        if (structType.IsPrimitive || structType.Namespace != null && structType.Namespace.StartsWith(nameof(System)) || structType.IsEnum)
            return false;

        return true;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var structType = typeToConvert.GenericTypeArguments[0];

        return (JsonConverter)Activator.CreateInstance(typeof(NullableStructSerializer<>).MakeGenericType(structType));
    }
}

public class NullableStructSerializer<TStruct> : JsonConverter<TStruct?>
    where TStruct : struct
{
    public override TStruct? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        return JsonSerializer.Deserialize<TStruct>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, TStruct? value, JsonSerializerOptions options)
    {
        if (value == null)
            writer.WriteNullValue();
        else
            JsonSerializer.Serialize(writer, value.Value, options);
    }
}
