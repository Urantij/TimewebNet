using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimewebNet.Categories.S3;

public class BucketModel
{
    [JsonProperty("id", Required = Required.Always)]
    public long Id { get; set; }

    /// <summary>
    /// [имяаккаунта]-[указанноеимя]
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    public string Name { get; set; }

    /// <summary>
    /// private public
    /// </summary>
    [JsonProperty("type", Required = Required.Always)]
    public string Type { get; set; }

    [JsonProperty("preset_id", Required = Required.Always)]
    public long PresetId { get; set; }

    /// <summary>
    /// Должно быть created
    /// </summary>
    [JsonProperty("status", Required = Required.Always)]
    public string Status { get; set; }

    [JsonProperty("location", Required = Required.Always)]
    public string Location { get; set; }

    [JsonProperty("hostname", Required = Required.Always)]
    public string Hostname { get; set; }

    /// <summary>
    /// Логин аккаунта
    /// </summary>
    [JsonProperty("access_key", Required = Required.Always)]
    public string AccessKey { get; set; }

    /// <summary>
    /// Пароль аккаунта
    /// </summary>
    [JsonProperty("secret_key", Required = Required.Always)]
    public string SecretKey { get; set; }

    public BucketModel(long id, string name, string type, long presetId, string status, string location, string hostname, string accessKey, string secretKey)
    {
        Id = id;
        Name = name;
        Type = type;
        PresetId = presetId;
        Status = status;
        Location = location;
        Hostname = hostname;
        AccessKey = accessKey;
        SecretKey = secretKey;
    }
}
