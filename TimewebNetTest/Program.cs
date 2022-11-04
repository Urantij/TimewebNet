using System.Security.Cryptography;
using Minio;
using TimewebNet.Models;
using TimeWebNet;

const string configPath = "testconfig.txt";

string[] configLines = (await File.ReadAllLinesAsync(configPath)).Where(l => !string.IsNullOrEmpty(l)).ToArray();

Console.WriteLine("Hello, World!");

string refreshToken = configLines[0];

TimeWebApi api = new();

if (configLines.Length == 1)
{
    System.Console.WriteLine("Берём токен.");

    refreshToken = await api.GetTokenAsync(refreshToken);

    System.Console.WriteLine("Сохраняем токен.");

    await File.WriteAllLinesAsync(configPath, new string[]
    {
        refreshToken,
        api.AccessToken!
    });
}
else
{
    api.SetAccessToken(configLines[1]);
}

System.Console.WriteLine("Создаём ведро.");

long bucketId = await api.CreateBucketAsync("testbucket", S3ServiceType.Promo);

ListBucketsResponseModel.StorageModel bucketStorage;
{
    int attempts = 0;

    while (true)
    {
        await Task.Delay(5000);

        attempts++;

        System.Console.WriteLine($"Сосём вёдра... {attempts}");

        var buckets = await api.ListBucketsAsync();

        var ourBucket = buckets.FirstOrDefault(b => b.Id == bucketId);

        if (ourBucket != null)
        {
            bucketStorage = ourBucket;
            break;
        }
    }

}

string username = bucketStorage.Name.Split('-')[0];
string secret = bucketStorage.Password;

MinioClient s3Client = new MinioClient().WithCredentials(username, secret)
                                        .WithEndpoint("s3.timeweb.com")
                                        .WithRegion(bucketStorage.Region)
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

System.Console.WriteLine("Окей... удаляем ведро.");
await api.DeleteBucketAsync(bucketId);

System.Console.WriteLine("Удалили.");

s3Client.Dispose();