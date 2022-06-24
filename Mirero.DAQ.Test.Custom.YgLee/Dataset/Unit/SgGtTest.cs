// using Google.Protobuf;
// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class SgGtTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     private static readonly string DownloadFolder = DatasetClientTest.DownloadFolder;
//     private static readonly string GtPath = Path.Combine(DatasetClientTest.GtPath, "Segmentation");
//
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//
//         // TestCreateSegmentationGt();
//         //TestListSegmentationGts();
//         //TestGetSegmentationGt();
//         //TestUpdateSegmentationGt();
//         TestDeleteSegmentationGt();
//     }
//     
//     private static void TestCreateSegmentationGt()
//     {
//         var gtList = new List<SegmentationGt>();
//
//         const string localFilename = "2092.png";
//         
//         const string filename = "image19_segmentation_gt1.png";
//
//         var uri = Path.Combine(GtPath, localFilename);
//         
//         using var fs = new FileStream(uri, FileMode.Open, FileAccess.Read);
//         var buffer = new byte[fs.Length];
//         fs.Read(buffer, 0, buffer.Length);
//
//         var response = _client.CreateSegmentationGt(
//             new CreateSegmentationGtRequest
//             {
//                 Filename = filename,
//                 Extension = "png",
//                 Buffer = ByteString.CopyFrom(buffer),
//                 // Properties = "{ \"key\" : \"test_properties3\" }",
//                 // Description = "Descriptions",
//                 DatasetId = 3,
//                 ImageId = 20,
//                 LockTimeoutSec = 2,
//             });
//         
//         Utils.ToString(response);
//     }
//
//     private static void TestListSegmentationGts(string listFolderName = "test_segmentation_gt_data1")
//     {
//         var testPath = Path.Combine(DownloadFolder, listFolderName);
//         Directory.CreateDirectory(testPath);
//         
//         var response = _client.ListSegmentationGts(new ListSegmentationGtsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10
//             },
//             LockTimeoutSec = 2,
//         });
//         
//         Console.WriteLine($"PageIndex : {response.PageResult.PageIndex}");
//         Console.WriteLine($"PageSize : {response.PageResult.PageSize}");
//         Console.WriteLine($"Count : {response.PageResult.Count}");
//         
//         var gts = response.SegmentationGts;
//         var list = gts.AsEnumerable();
//     
//         var i = 1;
//         foreach (var gt in list)
//         {
//             using var fs = new FileStream(Path.Combine(testPath, "segmentation_image" + i + "." + gt.Extension), FileMode.Create);
//             fs.Write(gt.Buffer.ToByteArray());
//             gt.Buffer = null;
//             Utils.ToString(gt);
//             i++;
//         }
//     }
//     
//     private static void TestGetSegmentationGt(string resultFolderName = "get_segmentation_gt_test")
//     {
//         var testPath = Path.Combine(DownloadFolder, resultFolderName);
//         
//         Directory.CreateDirectory(testPath);
//         
//         var response = _client.GetSegmentationGt(new GetSegmentationGtRequest
//         {
//             SegmentationGtId = 46,
//             LockTimeoutSec = 2,
//         });
//         
//         using var fs = new FileStream(Path.Combine(testPath, "segmentation_image1." + response.Extension), FileMode.Create);
//         fs.Write(response.Buffer.ToByteArray());
//         response.Buffer = null;
//         
//         Utils.ToString(response);
//     }
//     
//     // private static void TestGetImageDatasetByName()
//     // {
//     //     var response = _client.GetImageDatasetByName(new GetImageDatasetByNameRequest
//     //     {
//     //         Name = "dataset2"
//     //     });
//     //
//     //     Utils.ToString(response);
//     // }
//     
//     private static void TestUpdateSegmentationGt()
//     {
//         const string extension = "png";
//         const string localFilename = "3096." + extension;
//         const string filename = "image18_segmentation_gt1_updated1." + extension;
//
//         var fileUri = Path.Combine(GtPath, localFilename);
//         
//         using var fs = new FileStream(fileUri, FileMode.Open, FileAccess.Read);
//         var buffer = new byte[fs.Length];
//         fs.Read(buffer, 0, buffer.Length);
//         
//         var response = _client.UpdateSegmentationGt(
//             new UpdateSegmentationGtRequest
//             {
//                 Id = 46,
//                 Filename = filename,
//                 Extension = extension,
//                 Buffer = ByteString.CopyFrom(buffer),
//                 Description = "updated",
//                 DatasetId = 18,
//                 ImageId = 154,
//                 LockTimeoutSec = 2
//             });
//
//         Utils.ToString(response);
//     }
//     
//     private static void TestDeleteSegmentationGt()
//     {
//         var response = _client.DeleteSegmentationGt(
//             new DeleteSegmentationGtRequest
//             {
//                 SegmentationGtId = 46,
//                 LockTimeoutSec = 2,
//             });
//         
//         Utils.ToString(response);
//     }
// }