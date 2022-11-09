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
    public string? AccessToken { get; private set; }

    public S3Bucket S3Bucket { get; private set; }

    public readonly HttpClient client;
    readonly bool disposeClient = false;

    public TimeWebApi()
        : this(new HttpClient())
    {
        disposeClient = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client">Не забудь его задиспоузить</param>
    public TimeWebApi(HttpClient client)
    {
        this.client = client;

        S3Bucket = new S3Bucket(this);
    }

    public void SetAccessToken(string accessToken)
    {
        AccessToken = accessToken;
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
    }

    public async Task CallAsync(string url, HttpMethod method, object body)
    {
        using var message = new HttpRequestMessage(method, url);

        using var httpContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        message.Content = httpContent;

        var response = await client.SendAsync(message);

        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new BadCodeException(response.StatusCode, responseContent);
        }
    }

    public async Task<T> CallAsync<T>(string url, HttpMethod method, object body)
    {
        using var message = new HttpRequestMessage(method, url);

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

    public async Task CallAsync(string url, HttpMethod method)
    {
        using var message = new HttpRequestMessage(method, url);

        var response = await client.SendAsync(message);

        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new BadCodeException(response.StatusCode, responseContent);
        }
    }

    public async Task<T> CallAsync<T>(string url, HttpMethod method)
    {
        using var message = new HttpRequestMessage(method, url);

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

    /// <summary>
    /// Возвращает новый рефрештокен.
    /// Использованный токен больше работать не будет, так что новый лучше сохрани.
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<AuthResponseModel> GetTokenAsync(string refreshToken)
    {
        var body = new
        {
            refresh_token = refreshToken
        };

        var response = await CallAsync<AuthResponseModel>("https://public-api.timeweb.com/api/v2/auth", HttpMethod.Post, body);

        SetAccessToken(response.Access_token);

        return response;
    }

    public void Dispose()
    {
        if (disposeClient)
            client.Dispose();
    }
}