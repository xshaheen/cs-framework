// Copyright (c) Mahmoud Shaheen. All rights reserved.

namespace Framework.Serializer.Converters;

/// <summary>Json collection converter.</summary>
/// <typeparam name="TDatatype">Type of item to convert.</typeparam>
/// <typeparam name="TConverterType">Converter to use for individual items.</typeparam>
public sealed class CollectionItemJsonConverter<TDatatype, TConverterType> : JsonConverter<IEnumerable<TDatatype>?>
    where TConverterType : JsonConverter
{
    public override bool HandleNull => true;

    public override IEnumerable<TDatatype>? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        JsonSerializerOptions serializerOptions = new(options);
        serializerOptions.Converters.Clear();
        serializerOptions.Converters.Add(Activator.CreateInstance<TConverterType>());

        List<TDatatype> returnValue = [];

        while (reader.TokenType is not JsonTokenType.EndArray)
        {
            if (reader.TokenType is not JsonTokenType.StartArray)
            {
                returnValue.Add(JsonSerializer.Deserialize<TDatatype>(ref reader, serializerOptions)!);
            }

            reader.Read();
        }

        return returnValue;
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<TDatatype>? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();

            return;
        }

        JsonSerializerOptions serializerOptions = new(options);
        serializerOptions.Converters.Clear();
        serializerOptions.Converters.Add(Activator.CreateInstance<TConverterType>());

        writer.WriteStartArray();

        foreach (var data in value)
        {
            JsonSerializer.Serialize(writer, data, serializerOptions);
        }

        writer.WriteEndArray();
    }
}
