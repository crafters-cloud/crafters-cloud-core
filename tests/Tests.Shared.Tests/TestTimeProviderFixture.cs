using CraftersCloud.Core.Tests.Shared;

namespace CraftersCloud.Core.TestUtilities.Tests;

[Category("unit")]
public class TestTimeProviderFixture
{
    [Test]
    public async Task GetNow()
    {
        var timeProvider = new TestTimeProvider();
        var fixedNowFirstCall = timeProvider.FixedUtcNow;
        var nowFirstCall = timeProvider.UtcNow;

        await Task.Delay(500);
        var fixedNowSecondCall = timeProvider.FixedUtcNow;
        var nowSecondCall = timeProvider.UtcNow;

        fixedNowFirstCall.ShouldBe(fixedNowSecondCall);
        nowFirstCall.ShouldNotBe(nowSecondCall);
        nowSecondCall.ShouldBeGreaterThan(nowFirstCall);
    }

    [Test]
    public void SetNow_SetsBothUtcNowAndFixedNow()
    {
        var now = new DateTimeOffset();
        var timeProvider = new TestTimeProvider();
        timeProvider.SetNow(now);

        timeProvider.FixedUtcNow.ShouldBe(now);
        timeProvider.UtcNow.ShouldBe(now);
    }
}