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
            /// <summary>
            /// Логин аккаунта
            /// </summary>
            public string Access_key { get; set; }
            /// <summary>
            /// Пароль аккаунта
            /// </summary>
            public string Secret_key { get; set; }

            public BucketModel(long id, string name, string region, long preset_id, string status, string type, string access_key, string secret_key)
                : base(id, name, region, preset_id, status, type)
            {
                Access_key = access_key;
                Secret_key = secret_key;
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