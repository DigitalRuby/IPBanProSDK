using System.Threading;
using System.Threading.Tasks;

using DigitalRuby.IPBanProSDK;

using NUnit.Framework;

namespace DigitalRuby.IPBanProSDKTests;

[TestFixture]
public class SynchronizationContextRemoverTests
{
    [Test]
    public async Task Awaitable_NoContextIsCompleted()
    {
        SynchronizationContext.SetSynchronizationContext(null);
        var awaiter = new SynchronizationContextRemover().GetAwaiter();
        Assert.That(awaiter.IsCompleted, Is.True);
        awaiter.GetResult();
        await new SynchronizationContextRemover();
    }

    [Test]
    public void OnCompleted_NullContext_RunsContinuationDirectly()
    {
        SynchronizationContext.SetSynchronizationContext(null);
        bool ran = false;
        new SynchronizationContextRemover().OnCompleted(() => ran = true);
        Assert.That(ran, Is.True);
    }

    [Test]
    public void OnCompleted_NonNullContext_RunsContinuationAndRestores()
    {
        var prev = new SynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(prev);
        try
        {
            bool ran = false;
            new SynchronizationContextRemover().OnCompleted(() => ran = true);
            Assert.That(ran, Is.True);
            Assert.That(SynchronizationContext.Current, Is.SameAs(prev));
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext(null);
        }
    }
}
