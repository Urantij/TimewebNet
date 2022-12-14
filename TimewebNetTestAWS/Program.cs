// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using Amazon.Runtime;
using Amazon.S3;
using TimewebNet.Models;
using TimeWebNet;

Console.WriteLine("Hello, World!");

const string configPath = "testconfig.txt";

string[] configLines = (await File.ReadAllLinesAsync(configPath)).Where(l => !string.IsNullOrEmpty(l)).ToArray();

Console.WriteLine("Hello, World!");

string refreshToken = configLines[0];

TimeWebApi api = new();

if (configLines.Length == 1)
{
    System.Console.WriteLine("Берём токен.");

    var auth = await api.GetTokenAsync(refreshToken);

    refreshToken = auth.Refresh_token;

    System.Console.WriteLine("Сохраняем токен.");

    await File.WriteAllLinesAsync(configPath, new string[]
    {
        auth.Refresh_token,
        auth.Access_token
    });
}
else
{
    api.SetAccessToken(configLines[1]);
}

System.Console.WriteLine("Смотрим вёдра.");
await api.S3Bucket.ListBucketsAsync();

System.Console.WriteLine("Создаём ведро.");

var createBucketResponse = await api.S3Bucket.CreateBucketAsync("testbucket", true, S3ServiceType.Promo);

ListBucketsResponseModel.BucketModel bucketStorage;
{
    int attempts = 0;

    while (true)
    {
        await Task.Delay(5000);

        attempts++;

        System.Console.WriteLine($"Сосём вёдра... {attempts}");

        var listResponse = await api.S3Bucket.ListBucketsAsync();

        var ourBucket = listResponse.Buckets.FirstOrDefault(b => b.Id == createBucketResponse.Bucket.Id);

        if (ourBucket != null)
        {
            bucketStorage = ourBucket;
            break;
        }
    }
}

AmazonS3Config configsS3 = new()
{
    ServiceURL = "https://s3.timeweb.com",
};

AmazonS3Client s3Client = new(bucketStorage.Access_key, bucketStorage.Secret_key, configsS3);

const string objectName = "test.txt";

System.Console.WriteLine("Кладём мусор...");
byte[] bullshit = RandomNumberGenerator.GetBytes(100);
{
    using MemoryStream ms = new(bullshit);

    await s3Client.PutObjectAsync(new Amazon.S3.Model.PutObjectRequest()
    {
        BucketName = bucketStorage.Name,
        InputStream = ms,
        AutoCloseStream = false,
        AutoResetStreamPosition = false,
        Key = objectName
    });

    System.Console.WriteLine("Положили.");
}


System.Console.WriteLine("Берём обратно.");
{
    using MemoryStream ms = new();

    var responseObject = await s3Client.GetObjectAsync(new Amazon.S3.Model.GetObjectRequest()
    {
        BucketName = bucketStorage.Name,
        Key = objectName,
    });

    await responseObject.ResponseStream.CopyToAsync(ms);

    var bytes = ms.ToArray();

    if (bytes.SequenceEqual(bullshit))
    {
        System.Console.WriteLine("Всё как надо.");
    }
    else
    {
        throw new Exception("Контент разный...");
    }
}

System.Console.WriteLine("Меняем ведро...");

await api.S3Bucket.ChangeBucketAsync(createBucketResponse.Bucket.Id, S3ServiceType.Lite);

System.Console.WriteLine("Изменили.");

System.Console.WriteLine("Окей... удаляем ведро.");
await api.S3Bucket.DeleteBucketAsync(createBucketResponse.Bucket.Id);

System.Console.WriteLine("Удалили.");

s3Client.Dispose();