using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimewebNet.Models
{
    public class ListBucketsResponseModel
    {
        public class StorageModel
        {
            [JsonProperty(Required = Required.Always)]
            public long Id { get; set; }

            /// <summary>
            /// [имяаккаунта]-[указанноеимя]
            /// </summary>
            [JsonProperty(Required = Required.Always)]
            public string Name { get; set; }

            [JsonProperty(Required = Required.Always)]
            public string Password { get; set; }

            [JsonProperty(Required = Required.Always)]
            public string Region { get; set; }

            [JsonProperty(Required = Required.Always)]
            public long Service_type { get; set; }

            /// <summary>
            /// Должно быть created
            /// </summary>
            [JsonProperty(Required = Required.Always)]
            public string Status { get; set; }

            /// <summary>
            /// private public
            /// </summary>
            [JsonProperty(Required = Required.Always)]
            public string Type { get; set; }

            public StorageModel(long id, string name, string password, string region, long service_type, string status, string type)
            {
                Id = id;
                Name = name;
                Password = password;
                Region = region;
                Service_type = service_type;
                Status = status;
                Type = type;
            }
        }

        [JsonProperty(Required = Required.Always)]
        public StorageModel[] Storages { get; set; }

        public ListBucketsResponseModel(StorageModel[] storages)
        {
            this.Storages = storages;
        }
    }
}