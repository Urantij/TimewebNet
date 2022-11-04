using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimewebNet.Models
{
    public class CreateBucketResponseModel
    {
        public class StorageModel
        {
            [JsonProperty(Required = Required.Always)]
            public long Id { get; set; }

            public StorageModel(long id)
            {
                Id = id;
            }
        }

        [JsonProperty(Required = Required.Always)]
        public StorageModel Storage { get; set; }

        public CreateBucketResponseModel(StorageModel storage)
        {
            Storage = storage;
        }
    }
}