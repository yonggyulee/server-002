// using Google.Protobuf;
// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class TrainingDataTest
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
//         //TestClassificationDataStream();
//         TestObjectDetectionDataStream();
//         //TestSegmentationDataStream();
//     }
//
//     private static void TestListSamplesStream(string streamListFolderName = "test_sample_image_stream_list5")
//     {
//         var path = Path.Combine(DownloadFolder, streamListFolderName);
//         Directory.CreateDirectory(path);
//
//         using var stream = _client.ListSamplesStream(new ListSamplesStreamRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 4,
//                 PageSize = 10,
//                 Where = "DatasetId = 1",
//                 OrderBy = "Id"
//             },
//             TotalCount = 30,
//             WithBuffer = true,
//             LockTimeoutSec = 2,
//         });
//
//         var responseStream = stream.ResponseStream;
//
//         while (responseStream.MoveNext(CancellationToken.None).Result)
//         {
//             var msg = responseStream.Current;
//             Console.WriteLine($"PageIndex : {msg.PageResult.PageIndex}");
//             Console.WriteLine($"PageSize : {msg.PageResult.PageSize}");
//             Console.WriteLine($"Count : {msg.Samples.Count}");
//             Console.WriteLine($"TotalCount : {msg.PageResult.Count}");
//
//             foreach (var sample in msg.Samples)
//             {
//                 foreach (var image in sample.Images)
//                 {
//                     var saveUri = Path.Combine(path, image.Filename);
//                     using var fs = new FileStream(saveUri, FileMode.Create);
//                     fs.Write(image.ToByteArray());
//                     image.Buffer = null;
//                     //Console.WriteLine($"Image : {image.Filename}");
//                 }
//                 Utils.ToString(sample);
//             }
//         }
//     }
//
//     private static void TestClassificationDataStream(string streamTrainingFolderName = "test_training_data")
//     {
//         var trainingDataPath = Path.Combine(DownloadFolder, streamTrainingFolderName);
//         Directory.CreateDirectory(trainingDataPath);
//
//         var classificationDataPath = Path.Combine(trainingDataPath, "classification");
//         Directory.CreateDirectory(classificationDataPath);
//
//         var imagePath = Path.Combine(classificationDataPath, "images");
//         //var gtPath = Path.Combine(classificationDataPath, "gts");
//         Directory.CreateDirectory(imagePath);
//
//         using var stream = _client.ListClassificationDataStream(new ListClassificationDataStreamRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 4,
//                 PageSize = 10,
//                 Where = "DatasetId = 87",
//                 OrderBy = "Id"
//             },
//             TotalCount = 30,
//             WithBuffer = true,
//             LockTimeoutSec = 2,
//         });
//
//         var responseStream = stream.ResponseStream;
//
//         while (responseStream.MoveNext(CancellationToken.None).Result)
//         {
//             var msg = responseStream.Current;
//             Console.WriteLine($"PageIndex : {msg.PageResult.PageIndex}");
//             Console.WriteLine($"PageSize : {msg.PageResult.PageSize}");
//             Console.WriteLine($"Count : {msg.Images.Count}");
//             Console.WriteLine($"AccumulateCount : {msg.PageResult.Count}");
//
//             var imgs = msg.Images.ToList();
//             var gts = msg.ClassificationGts.ToList();
//
//             for (var i = 0; i < msg.Images.Count; i++)
//             {
//                 using (var fs = new FileStream(Path.Combine(imagePath, imgs[i].Filename), FileMode.Create))
//                 {
//                     fs.Write(imgs[i].Buffer.ToByteArray());
//                     imgs[i].Buffer = null;
//                 }
//
//                 //using (var fs = new FileStream(Path.Combine(gtPath, gts[i].Filename), FileMode.Create))
//                 //{
//                 //    fs.Write(gts[i].Buffer.ToByteArray());
//                 //    gts[i].Buffer = null;
//                 //}
//
//                 Utils.ToString(imgs[i]);
//                 Utils.ToString(gts[i]);
//             }
//         }
//
//     }
//
//     private static void TestObjectDetectionDataStream(string streamTrainingFolderName = "test_training_data")
//     {
//         var trainingDataPath = Path.Combine(DownloadFolder, streamTrainingFolderName);
//         Directory.CreateDirectory(trainingDataPath);
//
//         var ObjectDetectionDataPath = Path.Combine(trainingDataPath, "object_detection");
//         Directory.CreateDirectory(ObjectDetectionDataPath);
//
//         var imagePath = Path.Combine(ObjectDetectionDataPath, "images");
//         var gtPath = Path.Combine(ObjectDetectionDataPath, "gts");
//         Directory.CreateDirectory(imagePath);
//         Directory.CreateDirectory(gtPath);
//
//         using var stream = _client.ListObjectDetectionDataStream(new ListObjectDetectionDataStreamRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 4,
//                 PageSize = 10,
//                 Where = "DatasetId = 124 or DatasetId = 127",
//                 OrderBy = "Id"
//             },
//             TotalCount = 30,
//             WithBuffer = true,
//             LockTimeoutSec = 2,
//         });
//
//         var responseStream = stream.ResponseStream;
//
//         while (responseStream.MoveNext(CancellationToken.None).Result)
//         {
//             var msg = responseStream.Current;
//             Console.WriteLine($"PageIndex : {msg.PageResult.PageIndex}");
//             Console.WriteLine($"PageSize : {msg.PageResult.PageSize}");
//             Console.WriteLine($"Count : {msg.Images.Count}");
//             Console.WriteLine($"AccumulateCount : {msg.PageResult.Count}");
//
//             var imgs = msg.Images.ToList();
//             var gts = msg.ObjectDetectionGts.ToList();
//
//             for (var i = 0; i < msg.Images.Count; i++)
//             {
//                 using (var fs = new FileStream(Path.Combine(imagePath, imgs[i].Filename), FileMode.Create))
//                 {
//                     fs.Write(imgs[i].Buffer.ToByteArray());
//                     imgs[i].Buffer = null;
//                 }
//
//                 using (var fs = new FileStream(Path.Combine(gtPath, gts[i].Filename), FileMode.Create))
//                 {
//                     fs.Write(gts[i].Buffer.ToByteArray());
//                     gts[i].Buffer = null;
//                 }
//
//                 Utils.ToString(imgs[i]);
//                 Utils.ToString(gts[i]);
//             }
//         }
//     }
//
//     private static void TestSegmentationDataStream(string streamTrainingFolderName = "test_training_data")
//     {
//         var trainingDataPath = Path.Combine(DownloadFolder, streamTrainingFolderName);
//         Directory.CreateDirectory(trainingDataPath);
//
//         var SegmentationDataPath = Path.Combine(trainingDataPath, "segmentation");
//         Directory.CreateDirectory(SegmentationDataPath);
//
//         var imagePath = Path.Combine(SegmentationDataPath, "images");
//         var gtPath = Path.Combine(SegmentationDataPath, "gts");
//         Directory.CreateDirectory(imagePath);
//         Directory.CreateDirectory(gtPath);
//
//         using var stream = _client.ListSegmentationDataStream(new ListSegmentationDataStreamRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 4,
//                 PageSize = 10,
//                 Where = "DatasetId = 125",
//                 OrderBy = "Id"
//             },
//             TotalCount = 30,
//             WithBuffer = true,
//             LockTimeoutSec = 2,
//         });
//
//         var responseStream = stream.ResponseStream;
//
//         while (responseStream.MoveNext(CancellationToken.None).Result)
//         {
//             var msg = responseStream.Current;
//             Console.WriteLine($"PageIndex : {msg.PageResult.PageIndex}");
//             Console.WriteLine($"PageSize : {msg.PageResult.PageSize}");
//             Console.WriteLine($"Count : {msg.Images.Count}");
//             Console.WriteLine($"AccumulateCount : {msg.PageResult.Count}");
//
//             var imgs = msg.Images.ToList();
//             var gts = msg.SegmentationGts.ToList();
//
//             for (var i = 0; i < msg.Images.Count; i++)
//             {
//                 using (var fs = new FileStream(Path.Combine(imagePath, imgs[i].Filename), FileMode.Create))
//                 {
//                     fs.Write(imgs[i].Buffer.ToByteArray());
//                     imgs[i].Buffer = null;
//                 }
//
//                 using (var fs = new FileStream(Path.Combine(gtPath, gts[i].Filename), FileMode.Create))
//                 {
//                     fs.Write(gts[i].Buffer.ToByteArray());
//                     gts[i].Buffer = null;
//                 }
//
//                 Utils.ToString(imgs[i]);
//                 Utils.ToString(gts[i]);
//             }
//         }
//     }
// }