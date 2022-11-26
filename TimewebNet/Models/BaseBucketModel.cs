using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimewebNet.Models;

public class BaseBucketModel
{
    [JsonProperty(Required = Required.Always)]
    public long Id { get; set; }

    /// <summary>
    /// [имяаккаунта]-[указанноеимя]
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public string Name { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string Region { get; set; }

    [JsonProperty(Required = Required.Always)]
    public long Preset_id { get; set; }

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

    public BaseBucketModel(long id, string name, string region, long preset_id, string status, string type)
    {
        Id = id;
        Name = name;
        Region = region;
        Preset_id = preset_id;
        Status = status;
        Type = type;
    }
}
