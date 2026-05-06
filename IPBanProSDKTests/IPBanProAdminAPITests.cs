using System.Threading.Tasks;

using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class IPBanProAdminAPITests
{
    [Test]
    public async Task GetSettings_HitsExpectedPath()
    {
        var fake = new FakeHttpRequestMaker { Response = FakeHttpRequestMaker.Json(new { Message = "ok" }) };
        using var api = new IPBanProAdminAPI { RequestMaker = fake };
        await api.GetSettings();
        Assert.That(fake.LastUri.AbsolutePath, Is.EqualTo("/api/Settings"));
    }

    [Test]
    public async Task UpdateSettings_PostsBody()
    {
        var fake = new FakeHttpRequestMaker { Response = FakeHttpRequestMaker.Json(new { Message = "ok" }) };
        using var api = new IPBanProAdminAPI { RequestMaker = fake };
        await api.UpdateSettings(new SettingsUpdateModel());
        Assert.That(fake.LastUri.AbsolutePath, Is.EqualTo("/api/Settings"));
        Assert.That(fake.LastPostJson, Is.Not.Null);
    }
}
