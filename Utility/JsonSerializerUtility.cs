/*
IPBanPro SDK - https://ipban.com | https://github.com/DigitalRuby/IPBanProSDK
IPBan and IPBan Pro Copyright(c) 2012 Digital Ruby, LLC
support@ipban.com

The MIT License(MIT)

Copyright(c) 2012 Digital Ruby, LLC
*/

#nullable enable

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Shared System.Text.Json options for SDK serializers. KeyValuePair constructors lose parameter
    /// names under PublishTrimmed / ILLink, so reflection-based STJ fails without a custom converter.
    /// </summary>
    public static class JsonSerializerUtility
    {
        /// <summary>
        /// Options used by JsonDeflateSerializer and UncompressedJsonSerializer
        /// </summary>
        public static JsonSerializerOptions Options { get; } = CreateOptions();

        private static JsonSerializerOptions CreateOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            options.Converters.Add(new KeyValuePairStringObjectJsonConverter());
            options.Converters.Add(new ObjectValueJsonConverter());
            return options;
        }
    }

    /// <summary>
    /// Trim-safe converter for KeyValuePair&lt;string, object&gt; (Message.Parameters, etc.)
    /// </summary>
    public sealed class KeyValuePairStringObjectJsonConverter : JsonConverter<KeyValuePair<string, object>>
    {
        /// <inheritdoc />
        public override KeyValuePair<string, object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected start of KeyValuePair object");
            }

            string? key = null;
            object? value = null;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new KeyValuePair<string, object>(key!, value!);
                }
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected property name in KeyValuePair object");
                }

                string? propertyName = reader.GetString();
                if (!reader.Read())
                {
                    throw new JsonException("Unexpected end of KeyValuePair object");
                }

                if (string.Equals(propertyName, "Key", StringComparison.OrdinalIgnoreCase))
                {
                    key = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                }
                else if (string.Equals(propertyName, "Value", StringComparison.OrdinalIgnoreCase))
                {
                    value = ObjectValueJsonConverter.ReadValue(ref reader);
                }
                else
                {
                    reader.Skip();
                }
            }

            throw new JsonException("Unexpected end of KeyValuePair object");
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, KeyValuePair<string, object> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Key", value.Key);
            writer.WritePropertyName("Value");
            ObjectValueJsonConverter.WriteValue(writer, value.Value, options);
            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// Converter for object properties so polymorphic payloads survive trimming.
    /// </summary>
    public sealed class ObjectValueJsonConverter : JsonConverter<object>
    {
        /// <inheritdoc />
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => ReadValue(ref reader);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
            => WriteValue(writer, value, options);

        internal static object? ReadValue(ref Utf8JsonReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.String:
                    return reader.GetString();
                case JsonTokenType.Number:
                    if (reader.TryGetInt64(out long l))
                    {
                        return l;
                    }
                    if (reader.TryGetDouble(out double d))
                    {
                        return d;
                    }
                    return reader.GetDecimal();
                default:
                    using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
                    {
                        return doc.RootElement.Clone();
                    }
            }
        }

        internal static void WriteValue(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }
            if (value is JsonElement element)
            {
                element.WriteTo(writer);
                return;
            }
            if (value is string s)
            {
                writer.WriteStringValue(s);
                return;
            }
            if (value is bool b)
            {
                writer.WriteBooleanValue(b);
                return;
            }
            if (value is byte or sbyte or short or ushort or int or uint or long or ulong)
            {
                writer.WriteNumberValue(Convert.ToInt64(value));
                return;
            }
            if (value is float or double or decimal)
            {
                writer.WriteNumberValue(Convert.ToDouble(value));
                return;
            }

            // Prefer runtime type so Message.Data and similar payloads keep their shape
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}

#nullable restore
