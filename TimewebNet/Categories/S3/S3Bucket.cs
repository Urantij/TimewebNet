using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimewebNet.Models;
using TimeWebNet;

namespace TimewebNet.Categories.S3;

public class S3Bucket : CategoryApi
{
    public S3Bucket(TimeWebApi api)
        : base(api)
    {
    }

    public async Task<CreateBucketResponseModel> CreateBucketAsync(string name, bool @private, S3ServiceType serviceType)
    {
        var body = new
        {
            name = name,
            type = @private ? "private" : "public",
            service_type = (int)serviceType
        };

        var response = await api.CallAsync<CreateBucketResponseModel>("https://public-api.timeweb.com/api/v1/storages/buckets", HttpMethod.Post, body);

        return response;
    }

    public async Task<ListBucketsResponseModel> ListBucketsAsync()
    {
        var response = await api.CallAsync<ListBucketsResponseModel>("https://public-api.timeweb.com/api/v1/storages/buckets", HttpMethod.Get);

        return response;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newServiceType">Новый сервистайп должен быть больше старого. В гигабайтах больше.</param>
    /// <returns></returns>
    public async Task ChangeBucketAsync(long id, S3ServiceType newServiceType)
    {
        var body = new
        {
            service_type = (int)newServiceType
        };

        await api.CallAsync($"https://public-api.timeweb.com/api/v1/storages/buckets/{id}", HttpMethod.Patch, body);
    }

    public async Task DeleteBucketAsync(long id)
    {
        await api.CallAsync($"https://public-api.timeweb.com/api/v1/storages/buckets/{id}", HttpMethod.Delete);
    }
}
