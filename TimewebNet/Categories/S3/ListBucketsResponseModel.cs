using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TimewebNet.Models;

namespace TimewebNet.Categories.S3;

public class ListBucketsResponseModel : BaseResponseModel
{
    [JsonProperty("buckets", Required = Required.Always)]
    public BucketModel[] Buckets { get; set; }

    public ListBucketsResponseModel(string responseId, BucketModel[] buckets)
        : base(responseId)
    {
        this.Buckets = buckets;
    }
}
