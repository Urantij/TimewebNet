using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimewebNet.Models
{
    public class CreateBucketResponseModel
    {
        [JsonProperty(Required = Required.Always)]
        public BaseBucketModel Bucket { get; set; }

        public CreateBucketResponseModel(BaseBucketModel bucket)
        {
            Bucket = bucket;
        }
    }
}