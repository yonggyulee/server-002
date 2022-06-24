// using Google.Protobuf;
// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class SampleTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     private static readonly string DownloadFolder = DatasetClientTest.DownloadFolder;
//     private static readonly string ImagePath = Path.Combine(DatasetClientTest.TestDataPath, "TestImages");
//
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//
//         // TestCreateSample();
//         TestListSamples();
//         //TestGetSampleById();
//         //TestUpdateSample();
//         //TestDeleteSample();
//     }
//     
//     private static void TestCreateSample()
//     {
//         var imgList = new List<Image>();
//         const string imagePath = "D:/workspace/daq-server/TestImages";
//
//         for (int i = 1; i <= 3; i++)
//         {
//             var filename = "image1_" + i + ".jpg";
//             var uri = Path.Combine(imagePath, filename);
//
//             using var fs = new FileStream(uri, FileMode.Open, FileAccess.Read);
//             var buffer = new byte[fs.Length];
//             fs.Read(buffer, 0, buffer.Length);
//             imgList.Add(new Image
//             {
//                 SampleId = 1,
//                 DatasetId = 4,
//                 Filename = "image" + i + ".jpg",
//                 Extension = "jpg",
//                 ImageCode = "right",
//                 Buffer = ByteString.CopyFrom(buffer),
//             });
//         }
//
//         _client.CreateSample(new CreateSampleRequest
//         {
//             Sample = new Sample
//             {
//                 Id = 5,
//                 DatasetId = 13,
//                 Images = { imgList }
//             },
//             LockTimeoutSec = 2,
//         });
//     }
//
//     private static void TestListSamples(string listFolderName = "test_sample_image_list1")
//     {
//         var testPath = Path.Combine(DownloadFolder, listFolderName);
//         if (!Directory.Exists(testPath))
//         {
//             Directory.CreateDirectory(testPath);
//         }
//         
//         var response = _client.ListSamples(new ListSamplesRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10
//             },
//             WithBuffer = true,
//             LockTimeoutSec = 2,
//         });
//         
//         Console.WriteLine($"PageIndex : {response.PageResult.PageIndex}");
//         Console.WriteLine($"PageSize : {response.PageResult.PageSize}");
//         Console.WriteLine($"Count : {response.PageResult.Count}");
//     
//         var samples = response.Samples;
//         var list = samples.AsEnumerable();
//     
//         foreach (var sample in list)
//         {
//             Console.WriteLine("Sample:");
//             Console.WriteLine(sample.Id);
//             Console.WriteLine(sample.DatasetId);
//             Console.WriteLine(sample.Images.Count);
//             Console.WriteLine("Images:");
//             foreach (var image in sample.Images)
//             {
//                 Console.WriteLine(image.Id);
//                 Console.WriteLine(image.Filename);
//                 using var fs = new FileStream(Path.Combine(testPath, image.Filename), FileMode.Create);
//                 fs.Write(image.Buffer.ToByteArray());
//             }
//         }
//     }
//
//     private static void TestGetSampleById(string test_sample_images_path = "get_sample_test1")
//     {
//         var path = Path.Combine(DownloadFolder, test_sample_images_path);
//         Directory.CreateDirectory(path);
//         
//         var response = _client.GetSample(new GetSampleRequest
//         {
//            SampleId = 1,
//            DatasetId = 15,
//            LockTimeoutSec = 2,
//         });
//
//         Console.WriteLine(response.Id);
//         Console.WriteLine(response.DatasetId);
//         Console.WriteLine(response.Images.Count);
//
//         foreach (var image in response.Images)
//         {
//             using var fs = new FileStream(Path.Combine(path, image.Filename), FileMode.Create);
//             fs.Write(image.Buffer.ToByteArray());
//         }
//     }
//
//     private static void TestUpdateSample(int imgCount = 2, int imgNum = 15)
//     {
//         var files = Directory.EnumerateFiles(ImagePath).ToList();
//         var imgList = new List<Image>();
//
//         for (var k = imgNum - imgCount; k < imgNum; k++)
//         {
//             var fileUri = files[(k + 0) % files.Count];
//             using var fs = new FileStream(fileUri, FileMode.Open, FileAccess.Read);
//             var buffer = new byte[fs.Length];
//             fs.Read(buffer, 0, buffer.Length);
//             imgList.Add(new Image
//             {
//                 // SampleId = j,
//                 // DatasetId = d.Id,
//                 Filename = "image" + (k + 42) + "_updated" + ".jpg",
//                 Extension = "jpg",
//                 ImageCode = "right",
//                 // Uri = d.Uri + "\\image" + i + ".jpg",
//                 Buffer = ByteString.CopyFrom(buffer)
//             });
//         }
//         
//         var response = _client.UpdateSample(new UpdateSampleRequest
//         {
//             Sample = new Sample
//             {
//                 Id = 1,
//                 DatasetId = 18,
//                 Description = "updated",
//                 Images = { imgList },
//             },
//             LockTimeoutSec = 2,
//         });
//
//         Utils.ToString(response);
//     }
//     
//     private static void TestDeleteSample()
//     {
//         var response = _client.DeleteSample(new DeleteSampleRequest
//         {
//             SampleId = 1,
//             DatasetId = 18,
//             LockTimeoutSec = 2,
//         });
//         
//         Utils.ToString(response);
//     }
// }