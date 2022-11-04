using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TimewebNet.Exceptions;
using TimewebNet.Models;

namespace TimeWebNet
{
    public class TimeWebApi
    {
        public string? AccessToken { get; private set; }

        readonly HttpClient client;

        public TimeWebApi()
            : this(new HttpClient())
        {

        }

        public TimeWebApi(HttpClient client)
        {
            this.client = client;
        }

        public void SetAccessToken(string accessToken)
        {
            AccessToken = accessToken;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
        }

        /// <summary>
        /// Возвращает новый рефрештокен.
        /// Использованный токен больше работать не будет, так что новый лучше сохрани.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GetTokenAsync(string refreshToken)
        {
            using var message = new HttpRequestMessage(HttpMethod.Post, "https://public-api.timeweb.com/api/v2/auth");

            using var httpContent = new StringContent(JsonConvert.SerializeObject(new
            {
                refresh_token = refreshToken
            }), Encoding.UTF8, "application/json");
            message.Content = httpContent;

            var response = await client.SendAsync(message);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var auth = JsonConvert.DeserializeObject<AuthResponseModel>(responseContent)!;

                SetAccessToken(auth.Access_token);

                return auth.Refresh_token;
            }
            else
            {
                throw new BadCodeException(response.StatusCode, responseContent, nameof(GetTokenAsync));
            }
        }

        public async Task<long> CreateBucketAsync(string name, S3ServiceType serviceType)
        {
            using var message = new HttpRequestMessage(HttpMethod.Post, "https://public-api.timeweb.com/api/v1/storages/buckets");

            using var httpContent = new StringContent(JsonConvert.SerializeObject(new
            {
                name = name,
                type = "private",
                service_type = (int)serviceType
            }), Encoding.UTF8, "application/json");
            message.Content = httpContent;

            var response = await client.SendAsync(message);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var deserialized = JsonConvert.DeserializeObject<CreateBucketResponseModel>(responseContent)!;

                return deserialized.Storage.Id;
            }
            else
            {
                throw new BadCodeException(response.StatusCode, responseContent, nameof(CreateBucketAsync));
            }
        }

        public async Task<ListBucketsResponseModel.StorageModel[]> ListBucketsAsync()
        {
            using var message = new HttpRequestMessage(HttpMethod.Get, "https://public-api.timeweb.com/api/v1/storages/buckets");

            var response = await client.SendAsync(message);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var deserialized = JsonConvert.DeserializeObject<ListBucketsResponseModel>(responseContent)!;

                return deserialized.Storages;
            }
            else
            {
                throw new BadCodeException(response.StatusCode, responseContent, nameof(ListBucketsAsync));
            }
        }

        public async Task DeleteBucketAsync(long id)
        {
            using var message = new HttpRequestMessage(HttpMethod.Delete, $"https://public-api.timeweb.com/api/v1/storages/buckets/{id}");

            using var httpContent = new StringContent(JsonConvert.SerializeObject(new
            {
                storage_id = id
            }), Encoding.UTF8, "application/json");
            message.Content = httpContent;

            var response = await client.SendAsync(message);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new BadCodeException(response.StatusCode, responseContent, nameof(DeleteBucketAsync));
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}