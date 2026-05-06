using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

using DigitalRuby.IPBanCore;
using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class IPBanProSDKExtensionMethodsTests
{
    [Test]
    public void BinarySearch_FindsItem()
    {
        IReadOnlyList<int> list = new[] { 1, 3, 5, 7, 9, 11 };
        int idx = list.BinarySearch(5, (k, v) => k.CompareTo(v));
        Assert.That(idx, Is.EqualTo(2));
    }

    [Test]
    public void BinarySearch_MissReturnsBitwiseInsertionPoint()
    {
        IReadOnlyList<int> list = new[] { 1, 3, 5, 7, 9, 11 };
        int idx = list.BinarySearch(4, (k, v) => k.CompareTo(v));
        Assert.That(idx, Is.LessThan(0));
        Assert.That(~idx, Is.EqualTo(2));
    }

    [Test]
    public void BinarySearch_EmptyList()
    {
        IReadOnlyList<int> list = Array.Empty<int>();
        int idx = list.BinarySearch(1, (k, v) => k.CompareTo(v));
        Assert.That(~idx, Is.EqualTo(0));
    }

    [Test]
    public void Truncate_NoChangeWhenShorter()
    {
        Assert.That("abc".Truncate(10), Is.EqualTo("abc"));
    }

    [Test]
    public void Truncate_AddsEllipsis()
    {
        var truncated = "abcdefghij".Truncate(5);
        Assert.That(truncated, Does.EndWith("…"));
        Assert.That(truncated.Length, Is.LessThanOrEqualTo(5));
    }

    [Test]
    public void Truncate_NullReturnsNull()
    {
        Assert.That(((string)null).Truncate(5), Is.Null);
    }

    [Test]
    public void Truncate_PadAddsSpaces()
    {
        string padded = "ab".Truncate(5, padding: true);
        Assert.That(padded, Is.EqualTo("ab   "));
    }

    [Test]
    public void Truncate_HandlesCJK()
    {
        // CJK weighted as 5/3 chars each
        string s = "你好世界";
        var truncated = s.Truncate(3);
        Assert.That(truncated, Does.EndWith("…"));
    }

    [Test]
    public void IsCJK_TrueForChinese()
    {
        Assert.That('你'.IsCJK(), Is.True);
    }

    [Test]
    public void IsCJK_FalseForLatin()
    {
        Assert.That('a'.IsCJK(), Is.False);
    }

    [Test]
    public void SplitWithNoEmptyEntries_RemovesEmpty()
    {
        var parts = "a,,b,c".SplitWithNoEmptyEntries(',');
        Assert.That(parts, Is.EquivalentTo(new[] { "a", "b", "c" }));
    }

    [Test]
    public void SplitWithNoEmptyEntries_NullReturnsNull()
    {
        Assert.That(((string)null).SplitWithNoEmptyEntries(','), Is.Null);
    }
}
