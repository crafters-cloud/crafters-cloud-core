using FluentAssertions;
using NUnit.Framework;

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

        fixedNowFirstCall.Should().Be(fixedNowSecondCall);
        nowFirstCall.Should().NotBe(nowSecondCall);
        nowSecondCall.Should().BeAfter(nowFirstCall);
    }

    [Test]
    public void SetNow_SetsBothUtcNowAndFixedNow()
    {
        var now = new DateTimeOffset();
        var timeProvider = new TestTimeProvider();
        timeProvider.SetNow(now);
        
        timeProvider.FixedUtcNow.Should().Be(now);
        timeProvider.UtcNow.Should().Be(now);
    }
}