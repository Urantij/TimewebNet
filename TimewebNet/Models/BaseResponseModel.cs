using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimewebNet.Models;

public class BaseResponseModel
{
    [JsonProperty("response_id", Required = Required.Always)]
    public string ResponseId { get; set; }

    public BaseResponseModel(string responseId)
    {
        ResponseId = responseId;
    }
}
