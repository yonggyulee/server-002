//using Google.Protobuf;
//using Grpc.Net.Client;
//using Mirero.DAQ.Domain.Common.Protos;
//using Mirero.DAQ.Domain.MachineLearning.Protos.V1;

//namespace Mirero.DAQ.Test.Custom.YgLee.MachineLearning.Unit;

//public class ModelVersionTest
//{
//    private static MachineLearningService.MachineLearningServiceClient _client;
//    private const string DownloadFolder = "D:/workspace/daq-server/Src/Mirero.DAQ.Test.Custom.YgLee/ml_download_data/";
//    private const string TestDataPath = "D:/workspace/TestData/TestModel_mar";

//    public static void Test()
//    {
//        var channel = GrpcChannel.ForAddress("http://localhost:5002");
//        _client = new MachineLearningService.MachineLearningServiceClient(channel);

//        //TestCreateModelVersion();
//        //TestListModelVersions();
//        //TestGetModelVersion();
//        //TestUpdateModelVersion();
//        //TestDeleteModelVersion();
//        //TestUploadModelVersionStream().Wait();
//        TestDownloadModelVersionStream().Wait();
//    }

//    private static void TestDeleteModelVersion()
//    {
//        var deleteModelVersion = _client.DeleteModelVersion(new DeleteModelVersionRequest
//        {
//            ModelVersionId = 2
//        });

//        Utils.ToString(deleteModelVersion);
//    }

//    private static void TestCreateModelVersion()
//    {
//        var uri = Path.Combine(TestDataPath, "fcnn_model_1.mar");
//        using var fs = new FileStream(uri, FileMode.Open, FileAccess.Read);
//        var buffer = new byte[fs.Length];
//        fs.Read(buffer, 0, buffer.Length);

//        _client.CreateModelVersion(new ModelVersion
//        {
//            ModelId = 5,
//            Version = "1",
//            Filename = "fcnn_model_1.mar"
//        });
//    }

//    private static async Task TestUploadModelVersionStream()
//    {
//        var uri = Path.Combine(TestDataPath, "image2_2.jpg");

//        const int chunkSize = 32 * 1024;
//        var offset = 0;

//        await using var fs = new FileStream(uri, FileMode.Open, FileAccess.Read);
//        var fileLength = fs.Length;
//        var fileChunk = new byte[chunkSize];

//        using var call = _client.UploadModelVersionStream();
//        var length = chunkSize;
//        int n;
//        while ((n = fs.Read(fileChunk, 0, length)) != 0)
//        {
//            await call.RequestStream.WriteAsync(new UploadModelVersionRequest
//            {
//                ModelVersionId = 4,
//                Buffer = ByteString.CopyFrom(fileChunk)
//            });

//            offset += length;

//            length = (int)Math.Min(chunkSize, fileLength - offset);
//            Console.WriteLine(n);
//        }

//        await call.RequestStream.CompleteAsync();

//        var response = await call.ResponseAsync;

//        Console.WriteLine(response.ToString());
//    }

//    public static void TestUploadModelVersionFromTrainingJobResultAsync()
//    {
//        var response = _client.UploadModelVersionFromTrainingJobResult(new UploadModelVersionFromTrainingJobResultRequest
//        {
//            ModelVersionId = 5,
//            TrainingJobResultId = 1
//        });

//        Console.WriteLine(response);
//    }

//    public static async Task TestDownloadModelVersionStream()
//    {
//        const int chunkSize = 32 * 1024;
//        using var stream = _client.DownloadModelVersionStream(new DownloadModelVersionRequest
//        {
//            ModelVersionId = 4,
//            ChunkSize = chunkSize
//        });

//        var responseStream = stream.ResponseStream;

//        await responseStream.MoveNext(CancellationToken.None);
//        var fileLength = responseStream.Current.Info.FileSize;
//        var currentLength = responseStream.Current.Info.ChunkSize;
//        var saveUri = Path.Combine(DownloadFolder, Path.GetFileName(responseStream.Current.Info.Filename));
//        await using var fs = new FileStream(saveUri, FileMode.Create);
//        Console.WriteLine($"Download Start... {saveUri}");
//        Console.WriteLine($"Download : {currentLength} / {fileLength} Bytes ... {((float)currentLength / fileLength) * 100}%");
//        await fs.WriteAsync(responseStream.Current.Buffer.ToByteArray(), 0, (int)responseStream.Current.Info.ChunkSize);
//        while (await responseStream.MoveNext(CancellationToken.None))
//        {
//            var chunkLength = (int) responseStream.Current.Info.ChunkSize;
//            await fs.WriteAsync(responseStream.Current.Buffer.ToByteArray(), 0, chunkLength);
//            currentLength += chunkLength;
//            Console.WriteLine($"Download : {currentLength} / {fileLength} Bytes ... {((float)currentLength / fileLength) * 100}%");
//        }
//    }

//    private static void TestListModelVersions()
//    {
//        var testPath = Path.Combine(DownloadFolder, "model_data2");

//        Directory.CreateDirectory(testPath);

//        var response = _client.ListModelVersions(new ListModelVersionsRequest
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 20
//            }
//        });

//        Console.WriteLine($"PageIndex : {response.PageResult.PageIndex}");
//        Console.WriteLine($"PageSize : {response.PageResult.PageSize}");
//        Console.WriteLine($"Count : {response.PageResult.Count}");

//        var modelVersions = response.ModelVersions;
//        var list = modelVersions.AsEnumerable();

//        foreach (var modelVersion in list)
//        {
//            //Directory.CreateDirectory(Path.Combine(testPath, modelVersion.Version));
//            //using var fs = new FileStream(Path.Combine(testPath, modelVersion.Version, modelVersion.Filename), FileMode.Create);
//            //fs.Write(modelVersion.Buffer.ToByteArray());
//            //modelVersion.Buffer = null;
//            Utils.ToString(modelVersion);
//        }
//    }

//    //private static void TestGetModelVersion()
//    //{
//    //    var testPath = Path.Combine(DownloadFolder, "model_data3");

//    //    Directory.CreateDirectory(testPath);

//    //    var response = _client.GetModelVersion(new GetModelVersionRequest
//    //    {
//    //        ModelVersionId = 2
//    //    });

//    //    Directory.CreateDirectory(Path.Combine(testPath, response.Version));
//    //    using var fs = new FileStream(Path.Combine(testPath, response.Version, response.Filename), FileMode.Create);
//    //    fs.Write(response.Buffer.ToByteArray());
//    //    response.Buffer = null;
//    //    Utils.ToString(response);
//    //}

//    private static void TestUpdateModelVersion()
//    {
//        var uri = Path.Combine(TestDataPath, "fcnn_model_1.mar");
//        using var fs = new FileStream(uri, FileMode.Open, FileAccess.Read);
//        var buffer = new byte[fs.Length];
//        fs.Read(buffer, 0, buffer.Length);

//        var response = _client.UpdateModelVersion(new ModelVersion
//        {
//            Id = 2,
//            ModelId = 6,
//            Version = "1_updated",
//            Filename = "fcnn_model_1_updated.mar"
//        });

//        Utils.ToString(response);
//    }
//}