using System;
using System.IO;
using System.Text;

using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class SerializerTests
{
    [Test]
    public void DefaultSerializer_IsJsonDeflate()
    {
        Assert.That(DefaultSerializer.Instance, Is.InstanceOf<JsonDeflateSerializer>());
        Assert.That(DefaultSerializer.Instance.Description, Is.EqualTo("json-deflate"));
    }

    [Test]
    public void JsonDeflate_RoundTrip()
    {
        var s = JsonDeflateSerializer.Instance;
        var msg = new Message { Id = "id1", Name = "hello" };
        var bytes = s.Serialize(msg);
        Assert.That(bytes, Is.Not.Null);
        var roundtrip = (Message)s.Deserialize(bytes, typeof(Message));
        Assert.That(roundtrip.Id, Is.EqualTo("id1"));
        Assert.That(roundtrip.Name, Is.EqualTo("hello"));
    }

    [Test]
    public void JsonDeflate_DeserializeMemoryStream()
    {
        ISerializer s = JsonDeflateSerializer.Instance;
        var bytes = s.Serialize(new Message { Name = "abc" });
        // need publiclyVisible so GetBuffer() works
        using var ms = new MemoryStream(bytes.Length);
        ms.Write(bytes, 0, bytes.Length);
        ms.Position = 0;
        var msg = (Message)s.Deserialize(ms, typeof(Message));
        Assert.That(msg.Name, Is.EqualTo("abc"));
    }

    [Test]
    public void JsonDeflate_NullSerializesToNull()
    {
        Assert.That(JsonDeflateSerializer.Instance.Serialize(null), Is.Null);
    }

    [Test]
    public void JsonDeflate_EmptyDeserializesToNull()
    {
        Assert.That(JsonDeflateSerializer.Instance.Deserialize(ReadOnlySpan<byte>.Empty, typeof(Message)), Is.Null);
    }

    [Test]
    public void Uncompressed_RoundTrip()
    {
        var s = new UncompressedJsonSerializer();
        var msg = new Message { Name = "ping" };
        var bytes = s.Serialize(msg);
        var rt = (Message)s.Deserialize(bytes, typeof(Message));
        Assert.That(rt.Name, Is.EqualTo("ping"));
        Assert.That(s.Description, Is.EqualTo("json"));
    }

    [Test]
    public void Uncompressed_NullEmptyHandled()
    {
        var s = new UncompressedJsonSerializer();
        Assert.That(s.Serialize(null), Is.Null);
        Assert.That(s.Deserialize(ReadOnlySpan<byte>.Empty, typeof(Message)), Is.Null);
    }

    [Test]
    public void ProtobufLZ4_DescriptionAndStaticInstance()
    {
        var s = ProtobufLZ4Serializer.Instance;
        Assert.That(s.Description, Does.Contain("ProtobufLZ4"));
        Assert.That(s, Is.SameAs(ProtobufLZ4Serializer.Instance));
    }

    [Test]
    public void ProtobufLZ4_RoundTripWithMachine()
    {
        // Machine has DataContract + DataMember with unique orders; protobuf-net handles it.
        // (Message.Data is `object`, which protobuf can't reflect on, so use a simpler model.)
        var s = ProtobufLZ4Serializer.Instance;
        var m = new Machine { Id = 7, FQDN = "host.example.com", Status = "OK" };
        var bytes = s.Serialize(m);
        Assert.That(bytes, Is.Not.Null.And.Not.Empty);
        var rt = (Machine)s.Deserialize(bytes, typeof(Machine));
        Assert.That(rt.Id, Is.EqualTo(7));
        Assert.That(rt.FQDN, Is.EqualTo("host.example.com"));
        Assert.That(rt.Status, Is.EqualTo("OK"));
    }

    [Test]
    public void ProtobufLZ4_NullEmptyHandled()
    {
        var s = ProtobufLZ4Serializer.Instance;
        Assert.That(s.Serialize(null), Is.Null);
        Assert.That(s.Deserialize(ReadOnlySpan<byte>.Empty, typeof(Message)), Is.Null);
    }
}
