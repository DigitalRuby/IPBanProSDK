/*

IPBanPro SDK - https://ipban.com | https://github.com/DigitalRuby/IPBanProSDK
IPBan and IPBan Pro Copyright(c) 2012 Digital Ruby, LLC
support@digitalruby.com

The MIT License(MIT)

Copyright(c) 2012 Digital Ruby, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

using DigitalRuby.IPBanCore;
using K4os.Compression.LZ4.Streams;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProtoBuf;

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
            using (StreamWriter textWriter = new StreamWriter(deflater, ExtensionMethods.Utf8EncodingNoPrefix))
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
            using (StreamWriter textWriter = new StreamWriter(ms, ExtensionMethods.Utf8EncodingNoPrefix))
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

#nullable enable

    /// <summary>
    /// Serialize and deserialize bytes using protobuf, then apply LZ4 compression. This class is thread safe.
    /// </summary>
    public class ProtobufLZ4Serializer : ISerializer
    {
        /// <summary>
        /// Shared instance
        /// </summary>
        public static ProtobufLZ4Serializer Instance { get; } = new ProtobufLZ4Serializer();

        /// <summary>
        /// Description
        /// </summary>
        public string Description => GetType().Name;

        /// <inheritdoc />
        public object? Deserialize(byte[] bytes, Type type)
        {
            if (bytes is null)
            {
                return null;
            }
            MemoryStream input = new MemoryStream(bytes);
            Stream lz4DecoderStream = LZ4Stream.Decode(input, leaveOpen: true);
            return Serializer.Deserialize(type, lz4DecoderStream);
        }

        /// <inheritdoc />
        public byte[] Serialize(object obj)
        {
            MemoryStream ms = new MemoryStream();
            {
                using Stream lz4EncoderStream = LZ4Stream.Encode(ms, leaveOpen: true);
                Serializer.Serialize(lz4EncoderStream, obj);
            }
            return ms.ToArray();
        }
    }

#nullable restore

}
