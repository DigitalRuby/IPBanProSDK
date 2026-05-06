using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using DigitalRuby.IPBanCore;

namespace DigitalRuby.IPBanProSDKTests;

/// <summary>
/// In-memory IHttpRequestMaker that records the last request and returns a canned response.
/// </summary>
internal sealed class FakeHttpRequestMaker : IHttpRequestMaker
{
    public Uri LastUri { get; private set; }
    public byte[] LastPostJson { get; private set; }
    public List<KeyValuePair<string, object>> LastHeaders { get; private set; }
    public string LastMethod { get; private set; }
    public int CallCount { get; private set; }

    public byte[] Response { get; set; } = Array.Empty<byte>();
    public Func<Uri, byte[], byte[]> ResponseFactory { get; set; }
    public Exception ToThrow { get; set; }

    public Task<byte[]> MakeRequestAsync(Uri uri,
        byte[] postJson = null,
        IEnumerable<KeyValuePair<string, object>> headers = null,
        string method = null,
        CancellationToken cancelToken = default)
    {
        CallCount++;
        LastUri = uri;
        LastPostJson = postJson;
        LastHeaders = headers?.ToList();
        LastMethod = method;

        if (ToThrow is not null)
        {
            throw ToThrow;
        }
        var resp = ResponseFactory is not null ? ResponseFactory(uri, postJson) : Response;
        return Task.FromResult(resp);
    }

    public string GetHeader(string name)
    {
        if (LastHeaders is null)
        {
            return null;
        }
        var match = LastHeaders.FirstOrDefault(h => string.Equals(h.Key, name, StringComparison.OrdinalIgnoreCase));
        return match.Value?.ToString();
    }

    public static byte[] Json(object obj) => Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(obj));
}
