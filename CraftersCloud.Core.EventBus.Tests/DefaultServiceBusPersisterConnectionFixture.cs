using CraftersCloud.Core.EventBus.ServiceBus;
using FluentAssertions;
using NUnit.Framework;

namespace CraftersCloud.Core.EventBus.Tests;

[Category("unit")]
public class DefaultServiceBusPersisterConnectionFixture
{
    [TestCase(
        "Endpoint=sb://txf-tmd-prod.servicebus.windows.net/;SharedAccessKeyName=DevEnvironmentAccessKeys;SharedAccessKey=secretSharedAccessKey;EntityPath=dev-entities",
        "dev-entities", TestName = "Connection string with EntityPath")]
    [TestCase(
        "Endpoint=sb://txf-tmd-prod.servicebus.windows.net/;SharedAccessKeyName=DevEnvironmentAccessKeys;SharedAccessKey=secretSharedAccessKey",
        null, TestName = "Connection string without EntityPath")]
    public void TestExtractTopicNameFromServiceBusConnectionString(string connectionString,
        string expectedTopicName)
    {
        var topicName = DefaultServiceBusPersisterConnection.ExtractTopicName(connectionString);
        topicName.Should().Be(expectedTopicName);
    }

    [TestCase(
        null,
        "", typeof(ArgumentNullException))]
    [TestCase(
        "",
        "", typeof(ArgumentException))]
    public void GivenConnectionStringIsInvalid_ExtractTopicName_ShouldThrowException(string connectionString,
        string expectedTopicName, Type expectedExceptionType) =>
        Assert.Throws(expectedExceptionType,
            () => { DefaultServiceBusPersisterConnection.ExtractTopicName(connectionString); });
}