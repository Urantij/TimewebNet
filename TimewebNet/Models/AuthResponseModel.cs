using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimewebNet.Models
{
    public class AuthResponseModel
    {
        [JsonProperty(Required = Required.Always)]
        public string Access_token { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Refresh_token { get; set; }

        [JsonProperty(Required = Required.Always)]
        public long Expires_in { get; set; }

        public AuthResponseModel(string access_token, string refresh_token, long expires_in)
        {
            Access_token = access_token;
            Refresh_token = refresh_token;
            Expires_in = expires_in;
        }
    }
}