using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimewebNet.Models
{
    public class ListBucketsResponseModel
    {
        public class BucketModel : BaseBucketModel
        {
            [JsonProperty(Required = Required.Always)]
            public string Password { get; set; }

            public BucketModel(long id, string name, string region, long preset_id, string status, string type, string password) : base(id, name, region, preset_id, status, type)
            {
                Password = password;
            }
        }

        [JsonProperty(Required = Required.Always)]
        public BucketModel[] Buckets { get; set; }

        public ListBucketsResponseModel(BucketModel[] buckets)
        {
            this.Buckets = buckets;
        }
    }
}