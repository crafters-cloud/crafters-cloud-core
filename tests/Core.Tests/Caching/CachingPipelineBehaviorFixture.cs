using System.Text.Json;
using UnionJsonConverterFactory = CraftersCloud.Core.Caching.UnionJsonConverterFactory;
using CraftersCloud.Core.Caching.Abstractions;
using CraftersCloud.Core.MediatR.Caching;
using CraftersCloud.Core.Results;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace CraftersCloud.Core.Tests.Caching;

[Category("unit")]
public class CachingPipelineBehaviorFixture
{
    public sealed record Dto(int Id);

    public sealed record GetDto(int Id) : ICachedQuery<QueryResult<Dto>>
    {
        public CachingOptions CachingOptions => CachingOptions.For<GetDto>(Id.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }

    private FusionCache _cache = null!;
    private int _handlerCalls;

    [SetUp]
    public void SetUp()
    {
        _cache = new FusionCache(new FusionCacheOptions());
        _handlerCalls = 0;
    }

    [TearDown]
    public void TearDown() => _cache.Dispose();

    private CachingPipelineBehavior<GetDto, QueryResult<Dto>> CreateBehavior(CacheSettings settings,
        IFusionCache? cache = null) =>
        new(cache ?? _cache,NullLogger<CachingPipelineBehavior<GetDto, QueryResult<Dto>>>.Instance, MsOptions.Create(settings),
            NullLogger<CacheSettings>.Instance);

    private async Task<QueryResult<Dto>> Send(CachingPipelineBehavior<GetDto, QueryResult<Dto>> behavior, GetDto query,
        QueryResult<Dto> response) =>
        await behavior.Handle(query, _ =>
        {
            _handlerCalls++;
            return Task.FromResult(response);
        }, CancellationToken.None);

    [Test]
    public async Task SuccessIsCached()
    {
        var behavior = CreateBehavior(new CacheSettings());

        await Send(behavior, new GetDto(1), new Dto(1));
        var second = await Send(behavior, new GetDto(1), new Dto(1));

        _handlerCalls.ShouldBe(1);
        second.Value.ShouldBe(new Dto(1));
    }

    [Test]
    public async Task UnionsRoundTripThroughTheDistributedCache()
    {
        // two app instances sharing a distributed cache: the second one has to deserialize the entry
        var distributedCache = new MemoryDistributedCache(MsOptions.Create(new MemoryDistributedCacheOptions()));
        var serializer = new FusionCacheSystemTextJsonSerializer(new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Converters = { new UnionJsonConverterFactory() }
        });
        var settings = new CacheSettings
        {
            DefaultFailureCases = { ["NotFoundResult"] = TimeSpan.FromMinutes(1) }
        };
        using var first = new FusionCache(new FusionCacheOptions());
        first.SetupDistributedCache(distributedCache, serializer);
        using var second = new FusionCache(new FusionCacheOptions());
        second.SetupDistributedCache(distributedCache, serializer);

        await Send(CreateBehavior(settings, first), new GetDto(1), new Dto(1));
        await Send(CreateBehavior(settings, first), new GetDto(2), Result.NotFound());
        var found = await Send(CreateBehavior(settings, second), new GetDto(1), new Dto(-1));
        var notFound = await Send(CreateBehavior(settings, second), new GetDto(2), new Dto(-1));

        _handlerCalls.ShouldBe(2);
        found.Value.ShouldBe(new Dto(1));
        notFound.Value.ShouldBeOfType<NotFoundResult>();
    }

    [Test]
    public async Task NotFoundIsNotCachedByDefault()
    {
        var behavior = CreateBehavior(new CacheSettings());

        await Send(behavior, new GetDto(1), Result.NotFound());
        var second = await Send(behavior, new GetDto(1), new Dto(1));

        _handlerCalls.ShouldBe(2);
        second.Value.ShouldBe(new Dto(1));
    }

    [Test]
    public async Task NotFoundIsCachedWhenConfiguredAsDefault()
    {
        var behavior = CreateBehavior(new CacheSettings
        {
            DefaultFailureCases = { ["NotFoundResult"] = TimeSpan.FromMinutes(1) }
        });

        await Send(behavior, new GetDto(1), Result.NotFound());
        var second = await Send(behavior, new GetDto(1), new Dto(1));

        _handlerCalls.ShouldBe(1);
        second.Value.ShouldBeOfType<NotFoundResult>();
    }

    [Test]
    public async Task EntrySettingOverridesTheDefault()
    {
        var key = CachingOptions.For<GetDto>("1").CacheKey.Prefix;
        var behavior = CreateBehavior(new CacheSettings
        {
            DefaultFailureCases = { ["NotFoundResult"] = TimeSpan.FromMinutes(1) },
            Entries =
            [
                new CacheEntrySetting
                {
                    Key = key,
                    Expiration = TimeSpan.FromMinutes(10),
                    LocalCacheExpiration = TimeSpan.FromMinutes(5),
                    FailureCases = { ["NotFoundResult"] = TimeSpan.Zero }
                }
            ]
        });

        await Send(behavior, new GetDto(1), Result.NotFound());
        await Send(behavior, new GetDto(1), new Dto(1));

        _handlerCalls.ShouldBe(2);
    }
}
