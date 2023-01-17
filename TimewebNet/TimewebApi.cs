using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TimewebNet.Categories.S3;
using TimewebNet.Exceptions;
using TimewebNet.Models;

namespace TimeWebNet;

public class TimeWebApi : IDisposable
{
    readonly string token;

    readonly Uri baseUri = new("https://api.timeweb.cloud");

    public S3Bucket S3Bucket { get; private set; }

    public readonly HttpClient client;
    readonly bool disposeClient = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="token">https://timeweb.cloud/my/api-keys</param>
    /// <param name="baseUri"></param>
    public TimeWebApi(string token, Uri? baseUri = null)
        : this(token, new HttpClient(), baseUri)
    {
        disposeClient = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="token">https://timeweb.cloud/my/api-keys</param>
    /// <param name="client">Не забудь его задиспоузить.</param>
    public TimeWebApi(string token, HttpClient client, Uri? baseUri = null)
    {
        this.token = token;
        this.client = client;

        if (baseUri != null)
        {
            this.baseUri = baseUri;
        }

        S3Bucket = new S3Bucket(this);
    }

    public async Task CallAsync(string apiPath, HttpMethod method, object body)
    {
        Uri uri = new(baseUri, apiPath);

        using var message = new HttpRequestMessage(method, uri);
        message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        using var httpContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        message.Content = httpContent;

        var response = await client.SendAsync(message);

        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new BadCodeException(response.StatusCode, responseContent);
        }
    }

    public async Task<T> CallAsync<T>(string apiPath, HttpMethod method, object body)
    {
        Uri uri = new(baseUri, apiPath);

        using var message = new HttpRequestMessage(method, uri);
        message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        using var httpContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        message.Content = httpContent;

        var response = await client.SendAsync(message);

        string responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var deserialized = JsonConvert.DeserializeObject<T>(responseContent)!;

            return deserialized;
        }
        else
        {
            throw new BadCodeException(response.StatusCode, responseContent);
        }
    }

    public async Task CallAsync(string apiPath, HttpMethod method)
    {
        Uri uri = new(baseUri, apiPath);

        using var message = new HttpRequestMessage(method, uri);
        message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await client.SendAsync(message);

        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new BadCodeException(response.StatusCode, responseContent);
        }
    }

    public async Task<T> CallAsync<T>(string apiPath, HttpMethod method)
    {
        Uri uri = new(baseUri, apiPath);

        using var message = new HttpRequestMessage(method, uri);
        message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await client.SendAsync(message);

        string responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var deserialized = JsonConvert.DeserializeObject<T>(responseContent)!;

            return deserialized;
        }
        else
        {
            throw new BadCodeException(response.StatusCode, responseContent);
        }
    }

    public void Dispose()
    {
        if (disposeClient)
            client.Dispose();
    }
}