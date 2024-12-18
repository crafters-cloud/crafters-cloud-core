﻿using System.Net;
using System.Text.Json;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace CraftersCloud.Core.AspNetCore.TestUtilities.Http;

[PublicAPI]
public static class HttpResponseMessageAssertionsExtensions
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static AndConstraint<HttpResponseMessageAssertions> BeBadRequest(
        this HttpResponseMessageAssertions constraints, string because = "", params object[] becauseArgs) =>
        constraints.HaveStatusCode(HttpStatusCode.BadRequest, because, becauseArgs);

    public static AndConstraint<HttpResponseMessageAssertions> BeNotFound(
        this HttpResponseMessageAssertions constraints, string because = "", params object[] becauseArgs) =>
        constraints.HaveStatusCode(HttpStatusCode.NotFound, because, becauseArgs);

    public static AndConstraint<HttpResponseMessageAssertions> ContainValidationError(
        this HttpResponseMessageAssertions constraints
        , string fieldName,
        string expectedValidationMessage = "", string because = "", params object[] becauseArgs)
    {
        var responseContent = constraints.Subject.Content.ReadAsStringAsync().Result;
        var errorFound = false;
        try
        {
            var json = JsonSerializer.Deserialize<ValidationProblemDetails>(responseContent, Options);

            if (json != null && json.Errors.TryGetValue(fieldName, out var errorsField))
            {
                errorFound = string.IsNullOrEmpty(expectedValidationMessage)
                    ? errorsField.Length != 0
                    : errorsField.Any(msg =>
                        msg.Contains(expectedValidationMessage, StringComparison.OrdinalIgnoreCase));
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        var assertion = Execute.Assertion;
        var assertionScope = assertion.ForCondition(errorFound).BecauseOf(because, becauseArgs);
        string message;
        object[] failArgs;
        if (string.IsNullOrEmpty(expectedValidationMessage))
        {
            message = "Expected response to have validation message with key: {0}{reason}, but found {1}.";
            failArgs = new object[] { fieldName, responseContent };
        }
        else
        {
            message =
                "Expected response to have validation message with key: {0} and message: {1} {reason}, but found {2}.";
            failArgs = new object[] { fieldName, expectedValidationMessage, responseContent };
        }

        _ = assertionScope.FailWith(message, failArgs);
        return new AndConstraint<HttpResponseMessageAssertions>(constraints);
    }
}