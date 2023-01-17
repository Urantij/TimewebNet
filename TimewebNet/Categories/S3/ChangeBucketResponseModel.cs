using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TimewebNet.Models;

namespace TimewebNet.Categories.S3;

public class ChangeBucketResponseModel : BaseResponseModel
{
    [JsonProperty("bucket", Required = Required.Always)]
    public BucketModel Bucket { get; set; }

    public ChangeBucketResponseModel(string responseId, BucketModel bucket)
        : base(responseId)
    {
        Bucket = bucket;
    }
}
