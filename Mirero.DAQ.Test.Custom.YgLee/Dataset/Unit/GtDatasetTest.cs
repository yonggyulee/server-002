// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class GtDatasetTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     private static readonly string DownloadFolder = DatasetClientTest.DownloadFolder;
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//         
//         // TestCreateGtDataset();
//         TestListGtDatasets();
//         // TestGetDataset();
//         // TestGetDatasetByName();
//         // TestUpdateClassificationGtDataset();
//         // TestDeleteClassificationGtDataset();
//     }
//
//     private static void TestListGtDatasets()
//     {
//         // var testPath = Path.Combine(DownloadFolder, listFolderName);
//         // if (!Directory.Exists(testPath))
//         // {
//         //     Directory.CreateDirectory(testPath);
//         // }
//         
//         var response = _client.ListGtDatasets(new ListGtDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10,
//                 Where = "Title.Contains(\"Classification\")"
//             }
//         });
//         
//         Console.WriteLine($"PageIndex : {response.PageResult.PageIndex}");
//         Console.WriteLine($"PageSize : {response.PageResult.PageSize}");
//         Console.WriteLine($"Count : {response.PageResult.Count}");
//     
//         var datasets = response.GtDatasets;
//         var list = datasets.AsEnumerable();
//     
//         var i = 1;
//         foreach (var dataset in list)
//         {
//             // using var fs = new FileStream(Path.Combine(testPath, "thumb" + i + ".jpg"), FileMode.Create);
//             // fs.Write(dataset.ThumbnailBuffer.ToByteArray());
//             // dataset.ThumbnailBuffer = null;
//             Utils.ToString(dataset);
//             i++;
//         }
//     }
// }