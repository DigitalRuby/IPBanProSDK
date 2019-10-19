using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

using DigitalRuby.IPBanCore;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DigitalRuby.IPBanProSDK
{
    /// <summary>
    /// Serialize / deserialize an object
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="bytes">Bytes to deserialize</param>
        /// <param name="type">Type of object to deserialize to</param>
        /// <returns>Deserialized object</returns>
        object Deserialize(byte[] bytes, Type type);

        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>Serialized bytes</returns>
        byte[] Serialize(object obj);

        /// <summary>
        /// Get a description for the serializer
        /// </summary>
        string Description { get; }
    }

    /// <summary>
    /// Get a default serializer
    /// </summary>
    public static class DefaultSerializer
    {
        /// <summary>
        /// Get the default serializer
        /// </summary>
        public static ISerializer Instance { get; } = new CompressedJsonSerializer();
    }

    /// <summary>
    /// Compressed json serializer
    /// </summary>
    public class CompressedJsonSerializer : ISerializer
    {
        /// <summary>
        /// Deserialize an object from compressed json bytes
        /// </summary>
        /// <param name="bytes">Compressed json bytes</param>
        /// <param name="type">Type of object</param>
        /// <returns>Object</returns>
        public object Deserialize(byte[] bytes, Type type)
        {
            StreamReader reader = new StreamReader(new DeflateStream(new MemoryStream(bytes), CompressionMode.Decompress), Encoding.UTF8);
            JsonTextReader jsonReader = new JsonTextReader(reader);
            JsonSerializer serializer = IPBanProSDKExtensionMethods.GetJsonSerializer();
            return serializer.Deserialize(jsonReader, type);
        }

        /// <summary>
        /// Serialize an object to compressed json bytes
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Compressed json bytes</returns>
        public byte[] Serialize(object obj)
        {
            MemoryStream ms = new MemoryStream();
            using (DeflateStream deflater = new DeflateStream(ms, CompressionLevel.Optimal, true))
            using (StreamWriter textWriter = new StreamWriter(deflater, IPBanExtensionMethods.Utf8EncodingNoPrefix))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(textWriter))
            {
                if (obj is byte[] bytes)
                {
                    jsonWriter.WritePropertyName("ValueBinary");
                    jsonWriter.WriteValue(Convert.ToBase64String(bytes));
                }
                else if (obj is string text)
                {
                    jsonWriter.WritePropertyName("ValueString");
                    jsonWriter.WriteValue(text);
                }
                else if (obj is JToken token)
                {
                    jsonWriter.WriteRaw(token.ToString(Formatting.None));
                }
                else
                {
                    JsonSerializer serializer = IPBanProSDKExtensionMethods.GetJsonSerializer();
                    serializer.Serialize(jsonWriter, obj);
                }
            }
            return (ms.GetBuffer().AsSpan(0, (int)ms.Length).ToArray());
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; } = "json-deflate";

        /// <summary>
        /// Singleton instance (thread safe)
        /// </summary>
        public static CompressedJsonSerializer Instance { get; } = new CompressedJsonSerializer();
    }

    /// <summary>
    /// Uncompressed json serializer
    /// </summary>
    public class UncompressedJsonSerializer : ISerializer
    {
        /// <summary>
        /// Deserialize from uncompressed json bytes
        /// </summary>
        /// <param name="bytes">Uncompressed json bytes</param>
        /// <param name="type">Type of object</param>
        /// <returns>Deserialized object</returns>
        public object Deserialize(byte[] bytes, Type type)
        {
            StreamReader reader = new StreamReader(new MemoryStream(bytes), Encoding.UTF8);
            JsonTextReader jsonReader = new JsonTextReader(reader);
            JsonSerializer serializer = IPBanProSDKExtensionMethods.GetJsonSerializer();
            return serializer.Deserialize(jsonReader, type);
        }

        /// <summary>
        /// Serialize an object to uncompressed json bytes
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Json bytes</returns>
        public byte[] Serialize(object obj)
        {
            MemoryStream ms = new MemoryStream();
            using (StreamWriter textWriter = new StreamWriter(ms, IPBanExtensionMethods.Utf8EncodingNoPrefix))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(textWriter))
            {
                if (obj is byte[] bytes)
                {
                    jsonWriter.WritePropertyName("ValueBinary");
                    jsonWriter.WriteValue(Convert.ToBase64String(bytes));
                }
                else if (obj is string text)
                {
                    jsonWriter.WritePropertyName("ValueString");
                    jsonWriter.WriteValue(text);
                }
                else if (obj is JToken token)
                {
                    jsonWriter.WriteRaw(token.ToString(Formatting.None));
                }
                else
                {
                    JsonSerializer serializer = IPBanProSDKExtensionMethods.GetJsonSerializer();
                    serializer.Serialize(jsonWriter, obj);
                }
            }
            return (ms.GetBuffer().AsSpan(0, (int)ms.Length).ToArray());
        }

        public string Description { get; } = "json";
    }
}
