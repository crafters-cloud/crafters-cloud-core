using System.Globalization;
using System.Text;
using CraftersCloud.Core.Helpers;
using Newtonsoft.Json;

namespace CraftersCloud.Core.AspNetCore.Tests.Http;

public class JsonHttpClient : IDisposable
{
    public HttpClient WrappedClient { get; }

    public JsonHttpClient(HttpClient createClient) => WrappedClient = createClient;

    public void Dispose() => WrappedClient?.Dispose();

    public async Task<T> GetAsync<T>(string uri, params (string key, string value)[] parameters)
    {
        var resourceUri = new Uri(uri, UriKind.Relative).AppendParameters(parameters);

        var response = await WrappedClient.GetAsync(resourceUri);
        return await response.DeserializeWithStatusCodeCheckAsync<T>();
    }

    public async Task<T> GetAsync<T>(Uri uri, params (string key, string value)[] parameters)
    {
        var resourceUri = uri.AppendParameters(parameters);

        var response = await WrappedClient.GetAsync(resourceUri);
        return await response.DeserializeWithStatusCodeCheckAsync<T>();
    }

    public async Task PutAsync<T>(string uri, T content) where T : notnull
    {
        var response = await WrappedClient.PutAsync(uri, CreateJsonContent(content));
        await response.EnsureSuccessStatusCodeAsync();
    }

    public async Task PostAsync<T>(string uri, T content) where T : notnull
    {
        var response = await WrappedClient.PostAsync(uri, CreateJsonContent(content));
        await response.EnsureSuccessStatusCodeAsync();
    }

    public async Task<TResponse> PutAsync<T, TResponse>(string uri, T content) where T : notnull
    {
        var response = await WrappedClient.PutAsync(uri, CreateJsonContent(content));
        return await response.DeserializeWithStatusCodeCheckAsync<TResponse>();
    }

    public async Task<TResponse> PostAsync<TResponse>(string uri)
    {
        var response = await WrappedClient.PostAsync(uri, CreateJsonContent(null));
        return await response.DeserializeWithStatusCodeCheckAsync<TResponse>();
    }

    public async Task<HttpResponseMessage> PostAsync(string uri)
    {
        var response = await WrappedClient.PostAsync(uri, CreateJsonContent(null));
        return response;
    }

    public async Task<TResponse> PostAsync<T, TResponse>(string uri, T content) where T : notnull
    {
        var response = await WrappedClient.PostAsync(uri, CreateJsonContent(content));
        return await response.DeserializeWithStatusCodeCheckAsync<TResponse>();
    }

    public async Task<TResponse> PatchAsync<T, TResponse>(string uri, T content) where T : notnull
    {
        var response = await WrappedClient.PatchAsync(uri, CreateJsonContent(content));
        return await response.DeserializeWithStatusCodeCheckAsync<TResponse>();
    }

    public async Task DeleteAsync(string uri)
    {
        var response = await WrappedClient.DeleteAsync(uri);
        response.EnsureSuccessStatusCode();
    }

    public async Task<TResponse> PostMultiPart<TResponse>(string uri,
        IEnumerable<(string name, string value)> values)
    {
        using var content =
            new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
        foreach (var (name, value) in values)
        {
            if (value.HasContent())
            {
                content.Add(new StringContent(value), name);
            }
        }

        var response =
            await WrappedClient.PostAsync(uri, content);
        return await response.DeserializeWithStatusCodeCheckAsync<TResponse>();
    }

    public async Task UploadFile(string uri, string name, string fileName, byte[] file) =>
        await UploadFile(uri, name, fileName, new MemoryStream(file));

    public async Task UploadFile(string uri, string name, string fileName, Stream stream)
    {
        using var content =
            new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
        content.Add(new StreamContent(stream), name, fileName);

        var response =
            await WrappedClient.PostAsync(uri, content);
        response.EnsureSuccessStatusCode();
    }

    private static HttpContent CreateJsonContent(object? content)
    {
        var json = JsonConvert.SerializeObject(content);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}