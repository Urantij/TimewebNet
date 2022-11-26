using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimewebNet.Models;

public class ChangeBucketResponseModel
{
    [JsonProperty(Required = Required.Always)]
    public BaseBucketModel Bucket { get; set; }

    public ChangeBucketResponseModel(BaseBucketModel bucket)
    {
        Bucket = bucket;
    }
}
