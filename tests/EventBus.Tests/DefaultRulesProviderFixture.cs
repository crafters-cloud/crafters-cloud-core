using Azure.Messaging.ServiceBus.Administration;
using CraftersCloud.Core.EventBus.ServiceBus.Rules;
using CraftersCloud.Core.IntegrationEvents;
using FluentAssertions;
using JetBrains.Annotations;
using NUnit.Framework;

namespace CraftersCloud.Core.EventBus.Tests;

[Category("unit")]
public class DefaultRulesProviderFixture
{
    private static DefaultRulesProvider _optionsProvider = null!;
    private const string SomeEventName = "eventName";
    private const string SubscriptionClientName = "TxfBackendSubscriptionClient";

    [SetUp]
    public void Setup() => _optionsProvider = new DefaultRulesProvider(SubscriptionClientName);

    [Test]
    public void GivenHandlerWithoutAttribute_GetRulesFor_ReturnsRulesWithSqlFilter()
    {
        var rules = GetRules<TestClasses.TestHandler>();

        rules.Count.Should().Be(1);

        AssertRuleContainsSqlExpression(rules.First(),
            "sys.Label in ('eventName') and user.OriginatorId != 'TxfBackendSubscriptionClient'");
    }

    [Test]
    public void GivenHandlerWithOnlyFromSelfAttribute_GetRulesFor_ReturnsRulesWithSqlFilter()
    {
        var rules = GetRules<TestClasses.TestHandlerOnlyProcessFromSelf>();

        rules.Count.Should().Be(1);

        AssertRuleContainsSqlExpression(rules.First(),
            "sys.Label in ('eventName')");
    }
    
    [Test]
    public void GivenHandlerWithoutEmptyClients_GetRulesFor_ReturnsRulesWithSqlFilter()
    {
        var rules = GetRules<TestClasses.TestHandlerWithEmptyClient>();

        rules.Count.Should().Be(1);

        AssertRuleContainsSqlExpression(rules.First(),
            "sys.Label in ('eventName') and user.OriginatorId != 'TxfBackendSubscriptionClient'");
    }

    [Test]
    public void GivenHandlerWithoutEmptyClientsAndProcessFromSelf_GetRulesFor_ReturnsRulesWithSqlFilter()
    {
        var rules = GetRules<TestClasses.TestHandlerWithEmptyClientAndProcessFromSelf>();

        rules.Count.Should().Be(1);

        AssertRuleContainsSqlExpression(rules.First(), "sys.Label in ('eventName')");
    }

    [Test]
    public void GivenHandlerWithSingleClient_GetRulesFor_ReturnsRulesWithSqlFilter()
    {
        var rules = GetRules<TestClasses.TestHandlerWithSingleClient>();

        rules.Count.Should().Be(1);

        AssertRuleContainsSqlExpression(rules.First(),
            "sys.Label in ('eventName') and user.OriginatorId != 'TxfBackendSubscriptionClient' and user.OriginatorId in ('SomeClientName')");
    }

    [Test]
    public void GivenHandlerWithSingleClientAndProcessFromSelf_GetRulesFor_ReturnsRulesWithSqlFilter()
    {
        var rules = GetRules<TestClasses.TestHandlerWithSingleClientAndProcessFromSelf>();

        rules.Count.Should().Be(1);

        AssertRuleContainsSqlExpression(rules.First(),
            "sys.Label in ('eventName') and user.OriginatorId in ('SomeClientName')");
    }
    
    [Test]
    public void GivenHandlerWithMultipleClients_GetRulesFor_ReturnsRulesWithSqlFilter()
    {
        var rules = GetRules<TestClasses.TestHandlerWithMultipleClients>();

        rules.Count.Should().Be(1);

        AssertRuleContainsSqlExpression(rules.First(),
            "sys.Label in ('eventName') and user.OriginatorId != 'TxfBackendSubscriptionClient' and user.OriginatorId in ('SomeClientName', 'AnotherClientName')");
    }

    [Test]
    public void GivenHandlerWithMultipleClientsAndProcessFromSelf_GetRulesFor_ReturnsRulesWithSqlFilter()
    {
        var rules = GetRules<TestClasses.TestHandlerWithMultipleClientsAndProcessFromSelf>();

        rules.Count.Should().Be(1);

        AssertRuleContainsSqlExpression(rules.First(),
            "sys.Label in ('eventName') and user.OriginatorId in ('SomeClientName', 'AnotherClientName')");
    }

    [Test]
    public void GivenHandlerEventNameAttribute_GetRulesFor_ReturnsRulesWithSqlFilter()
    {
        var rules = GetRules<TestClasses.TestHandlerWithEventNameAttribute>();

        rules.Count.Should().Be(1);

        AssertRuleContainsSqlExpression(rules.First(),
            "sys.Label in ('SomeEventName') and user.OriginatorId != 'TxfBackendSubscriptionClient'");
    }

    [Test]
    public void GivenHandlerEventNameAttributeAndProcessFromSelf_GetRulesFor_ReturnsRulesWithSqlFilter()
    {
        var rules = GetRules<TestClasses.TestHandlerWithEventNameAttributeAndProcessFromSelf>();

        rules.Count.Should().Be(1);

        AssertRuleContainsSqlExpression(rules.First(), "sys.Label in ('SomeEventName')");
    }

    private static List<CreateRuleOptions> GetRules<T>() where T : IIntegrationEventHandler =>
        _optionsProvider.GetRulesForEvent<T>(SomeEventName).ToList();

    private static void AssertRuleContainsSqlExpression(CreateRuleOptions rule, string expectedSqlExpression)
    {
        rule.Filter.Should().BeOfType<SqlRuleFilter>();
        var sqlFilter = (SqlRuleFilter) rule.Filter;
        sqlFilter.SqlExpression.Should().Be(expectedSqlExpression);
    }


    private static class TestClasses
    {
        [UsedImplicitly]
        internal class TestHandler : IIntegrationEventHandler;

        [ProcessMessagesSentFromSelf]
        [UsedImplicitly]
        internal class TestHandlerOnlyProcessFromSelf : IIntegrationEventHandler;

        [ProcessServiceBusMessagesFrom]
        [UsedImplicitly]
        internal class TestHandlerWithEmptyClient : IIntegrationEventHandler;

        [ProcessServiceBusMessagesFrom]
        [ProcessMessagesSentFromSelf]
        [UsedImplicitly]
        internal class TestHandlerWithEmptyClientAndProcessFromSelf : IIntegrationEventHandler;

        [ProcessServiceBusMessagesFrom("SomeClientName")]
        [UsedImplicitly]
        internal class TestHandlerWithSingleClient : IIntegrationEventHandler;

        [ProcessServiceBusMessagesFrom("SomeClientName")]
        [ProcessMessagesSentFromSelf]
        [UsedImplicitly]
        internal class TestHandlerWithSingleClientAndProcessFromSelf : IIntegrationEventHandler;

        [ProcessServiceBusMessagesFrom("SomeClientName", "AnotherClientName")]
        [UsedImplicitly]
        internal class TestHandlerWithMultipleClients : IIntegrationEventHandler;

        [ProcessServiceBusMessagesFrom("SomeClientName", "AnotherClientName")]
        [ProcessMessagesSentFromSelf]
        [UsedImplicitly]
        internal class TestHandlerWithMultipleClientsAndProcessFromSelf : IIntegrationEventHandler;

        [EventName("SomeEventName")]
        [UsedImplicitly]
        internal class TestHandlerWithEventNameAttribute : IIntegrationEventHandler;

        [EventName("SomeEventName")]
        [ProcessMessagesSentFromSelf]
        [UsedImplicitly]
        internal class TestHandlerWithEventNameAttributeAndProcessFromSelf : IIntegrationEventHandler;
    }
}