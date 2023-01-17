using System.Security.Cryptography;
using Minio;
using TimewebNet.Categories.S3;
using TimewebNet.Models;
using TimeWebNet;

const string configPath = "testconfig.txt";

string token = File.ReadAllLines(configPath)[0];

Console.WriteLine("Hello, World!");

TimeWebApi api = new(token);

System.Console.WriteLine("Смотрим вёдра.");
await api.S3Bucket.ListBucketsAsync();

System.Console.WriteLine("Создаём ведро.");

var createBucketResponse = await api.S3Bucket.CreateBucketAsync("testbucket", true, S3ServiceType.Promo);

BucketModel bucketStorage;
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

MinioClient s3Client = new MinioClient().WithCredentials(bucketStorage.AccessKey, bucketStorage.SecretKey)
                                        .WithEndpoint(bucketStorage.Hostname)
                                        .WithRegion(bucketStorage.Location)
                                        .WithSSL()
                                        .Build();

const string objectName = "test.txt";

System.Console.WriteLine("Кладём мусор...");
byte[] bullshit = RandomNumberGenerator.GetBytes(100);
{
    using MemoryStream ms = new(bullshit);

    await s3Client.PutObjectAsync(new PutObjectArgs().WithBucket(bucketStorage.Name)
                                                     .WithStreamData(ms)
                                                     .WithObjectSize(bullshit.Length)
                                                     .WithObject(objectName));

    System.Console.WriteLine("Положили.");
}


System.Console.WriteLine("Берём обратно.");
{
    using MemoryStream ms = new();

    var responseObject = await s3Client.GetObjectAsync(new GetObjectArgs().WithBucket(bucketStorage.Name)
                                                                          .WithObject(objectName)
                                                                          .WithCallbackStream(stream => stream.CopyTo(ms)));

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