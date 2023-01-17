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

    public async Task<CreateBucketResponseModel> CreateBucketAsync(string name, bool @private, S3ServiceType presetId)
    {
        var body = new
        {
            name = name,
            type = @private ? "private" : "public",
            preset_id = (int)presetId
        };

        var response = await api.CallAsync<CreateBucketResponseModel>("/api/v1/storages/buckets", HttpMethod.Post, body);

        return response;
    }

    public async Task<ListBucketsResponseModel> ListBucketsAsync()
    {
        var response = await api.CallAsync<ListBucketsResponseModel>("/api/v1/storages/buckets", HttpMethod.Get);

        return response;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="presetId">Новый презет_айди должен быть больше старого. В гигабайтах больше.</param>
    /// <returns></returns>
    public async Task<ChangeBucketResponseModel> ChangeBucketAsync(long id, S3ServiceType presetId)
    {
        var body = new
        {
            preset_id = (int)presetId
        };

        return await api.CallAsync<ChangeBucketResponseModel>($"/api/v1/storages/buckets/{id}", HttpMethod.Patch, body);
    }

    public async Task DeleteBucketAsync(long id)
    {
        await api.CallAsync($"/api/v1/storages/buckets/{id}", HttpMethod.Delete);
    }
}
