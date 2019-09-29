using System;
using System.Collections.Generic;
using System.Text;

using DigitalRuby.IPBan;

using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;

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
    }

    /// <summary>
    /// MessagePack serializer that uses LZ4MessagePackSerializer
    /// </summary>
    public class MessagePackSerializer : ISerializer
    {
        /// <summary>
        /// Deserialize lz4 compressed message pack bytes
        /// </summary>
        /// <param name="bytes">Bytes</param>
        /// <param name="type">Type</param>
        /// <returns>Object</returns>
        public object Deserialize(byte[] bytes, Type type)
        {
            return LZ4MessagePackSerializer.NonGeneric.Deserialize(type, bytes, ContractlessStandardResolver.Instance);
        }

        /// <summary>
        /// Serialize an object to message pack lz4 compressed bytes
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Bytes</returns>
        public byte[] Serialize(object obj)
        {
            return LZ4MessagePackSerializer.Serialize(obj, ContractlessStandardResolver.Instance);
        }

        /// <summary>
        /// Singleton instance (thread safe)
        /// </summary>
        public static MessagePackSerializer Instance { get; } = new MessagePackSerializer();
    }
}
