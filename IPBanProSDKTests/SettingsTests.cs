using System;
using System.Collections.Generic;

using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class SettingsTests
{
    [Test]
    public void SmtpEnableSslBool_FlipsBit1()
    {
        var s = new Settings();
        Assert.That(s.SmtpEnableSslBool, Is.False);
        s.SmtpEnableSslBool = true;
        Assert.That(s.SmtpEnableSslBool, Is.True);
        Assert.That((s.SmtpEnableSsl & 1) == 1, Is.True);
        s.SmtpEnableSslBool = false;
        Assert.That(s.SmtpEnableSslBool, Is.False);
    }

    [Test]
    public void SmtpSslSelfSignedCertificateBool_FlipsBit2()
    {
        var s = new Settings();
        s.SmtpSslSelfSignedCertificateBool = true;
        Assert.That(s.SmtpSslSelfSignedCertificateBool, Is.True);
        s.SmtpSslSelfSignedCertificateBool = false;
        Assert.That(s.SmtpSslSelfSignedCertificateBool, Is.False);
    }

    [Test]
    public void EnableListsBool_GetSet()
    {
        var s = new Settings();
        Assert.That(s.EnableListsBool, Is.True); // EnableLists default = 1
        s.EnableListsBool = false;
        Assert.That(s.EnableListsBool, Is.False);
        Assert.That(s.EnableLists, Is.EqualTo(0));
    }

    [Test]
    public void CountryBlacklistFirstFailedLoginBool_GetSet()
    {
        var s = new Settings();
        Assert.That(s.CountryBlacklistFirstFailedLoginBool, Is.True);
        s.CountryBlacklistFirstFailedLoginBool = false;
        Assert.That(s.CountryBlacklistFirstFailedLoginBool, Is.False);
    }

    [Test]
    public void AggregateBanUserNamesAllowBlankBool_GetSet()
    {
        var s = new Settings();
        Assert.That(s.AggregateBanUserNamesAllowBlankBool, Is.False);
        s.AggregateBanUserNamesAllowBlankBool = true;
        Assert.That(s.AggregateBanUserNamesAllowBlankBool, Is.True);
    }

    [Test]
    public void CountryBlacklistInvertBool_GetSet()
    {
        var s = new Settings();
        s.CountryBlacklistInvertBool = true;
        Assert.That(s.CountryBlacklistInvertBool, Is.True);
        s.CountryBlacklistInvertBool = false;
        Assert.That(s.CountryBlacklistInvertBool, Is.False);
    }

    [Test]
    public void CountryBlacklistPreciseBool_GetSet()
    {
        var s = new Settings();
        s.CountryBlacklistPreciseBool = true;
        Assert.That(s.CountryBlacklistPreciseBool, Is.True);
        s.CountryBlacklistPreciseBool = false;
        Assert.That(s.CountryBlacklistPreciseBool, Is.False);
    }

    [Test]
    public void AsnBlacklistWhitelistBool_GetSet()
    {
        var s = new Settings();
        Assert.That(s.AsnBlacklistWhitelistBool, Is.False);
        s.AsnBlacklistWhitelistBool = true;
        Assert.That(s.AsnBlacklistWhitelistBool, Is.True);
    }

    [Test]
    public void BanRangeThreshold_GetSet()
    {
        var s = new Settings();
        s.BanRangeThreshold = "5,10";
        Assert.That(s.BanRangeThreshold, Is.EqualTo("5,10"));
    }

    [Test]
    public void LogsMachineId_DefaultsToZero()
    {
        var s = new Settings();
        Assert.That(s.LogsMachineId, Is.EqualTo(0));
        s.LogsMachineId = 42;
        Assert.That(s.LogsMachineId, Is.EqualTo(42));
    }

    [Test]
    public void LogsLevel_DefaultIsErrorAndCachesAfterSet()
    {
        var s = new Settings();
        Assert.That(s.LogsLevel, Is.EqualTo(DigitalRuby.IPBanCore.LogLevel.Error));
        s.LogsLevel = DigitalRuby.IPBanCore.LogLevel.Info;
        Assert.That(s.LogsLevel, Is.EqualTo(DigitalRuby.IPBanCore.LogLevel.Info));
        // hits the cached path
        Assert.That(s.LogsLevel, Is.EqualTo(DigitalRuby.IPBanCore.LogLevel.Info));
    }

    [Test]
    public void LogsTimestamp_DefaultsToNowAndRoundTrips()
    {
        var s = new Settings();
        Assert.That(s.LogsTimestamp, Is.Not.EqualTo(default(DateTime)));
        var t = new DateTime(2023, 5, 1, 12, 0, 0, DateTimeKind.Utc);
        s.LogsTimestamp = t;
        Assert.That(s.LogsTimestamp.Date, Is.EqualTo(t.Date));
    }

    [Test]
    public void LogsMaxCount_DefaultsAnd_RoundTrips()
    {
        var s = new Settings();
        Assert.That(s.LogsMaxCount, Is.EqualTo(10000));
        s.LogsMaxCount = 50;
        Assert.That(s.LogsMaxCount, Is.EqualTo(50));
    }

    [Test]
    public void DefaultWhitelistBlacklistDurationDays_RoundTrip()
    {
        var s = new Settings();
        Assert.That(s.DefaultWhitelistBlacklistDurationDays, Is.EqualTo(0));
        s.DefaultWhitelistBlacklistDurationDays = 30;
        Assert.That(s.DefaultWhitelistBlacklistDurationDays, Is.EqualTo(30));
    }

    [Test]
    public void Properties_DictionaryRoundTrip()
    {
        var s = new Settings();
        s.Properties = new Dictionary<string, string> { ["a"] = "1", ["b"] = "2" };
        Assert.That(s.Properties.Count, Is.EqualTo(2));
        Assert.That(s.Properties["a"], Is.EqualTo("1"));
    }

    [Test]
    public void Properties_EmptyJsonReturnsEmptyDict()
    {
        var s = new Settings();
        Assert.That(s.Properties.Count, Is.EqualTo(0));
    }

    [Test]
    public void SmtpToAddresses_SplitsOnCommaAndSemicolon()
    {
        var s = new Settings { SmtpTo = "a@b.com,c@d.com;e@f.com" };
        Assert.That(s.SmtpToAddresses.Count, Is.EqualTo(3));
    }

    [Test]
    public void ConfigXml_DefaultEmpty()
    {
        var s = new Settings { ConfigXml = string.Empty };
        Assert.That(s.ConfigXml, Is.EqualTo(string.Empty));
    }
}
