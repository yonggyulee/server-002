// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class ClassCodeSetTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     private static readonly string DownloadFolder = DatasetClientTest.DownloadFolder;
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//
//         //TestCreateClassCodeSet();
//         // TestListClassCodeSets();
//         // TestGetClassCodeSet();
//         //TestUpdateClassCodeSet();
//         TestDeleteClassCodeSet();
//     }
//
//     private static void TestCreateClassCodeSet()
//     {
//         var response = _client.CreateClassCodeSet(new CreateClassCodeSetRequest
//         {
//             Title = "ClassCodeSet11111",
//             DirectoryName = "class_code_set11111",
//             Task = "Classification",
//             CreateUser = "이용규",
//             // Properties = "{ \"key\" : \"test_properties3\" }",
//             // Description = "Descriptions",
//             VolumeId = "volume1",
//         });
//         
//         Utils.ToString(response);
//     }
//
//     private static void TestListClassCodeSets(string listFolderName = "image_classcodeset_thumb1")
//     {
//         var testPath = Path.Combine(DownloadFolder, listFolderName);
//         if (!Directory.Exists(testPath))
//         {
//             Directory.CreateDirectory(testPath);
//         }
//         
//         var response = _client.ListClassCodeSets(new ListClassCodeSetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10
//             }
//         });
//         
//         Console.WriteLine($"PageIndex : {response.PageResult.PageIndex}");
//         Console.WriteLine($"PageSize : {response.PageResult.PageSize}");
//         Console.WriteLine($"Count : {response.PageResult.Count}");
//     
//         var classCodeSets = response.ClassCodeSets;
//         var list = classCodeSets.AsEnumerable();
//     
//         var i = 1;
//         foreach (var classCodeSet in list)
//         {
//             using var fs = new FileStream(Path.Combine(testPath, "thumb" + i + ".jpg"), FileMode.Create);
//             // fs.Write(classCodeSet.ThumbnailBuffer.ToByteArray());
//             // classCodeSet.ThumbnailBuffer = null;
//             Utils.ToString(classCodeSet);
//             i++;
//         }
//     }
//     
//     private static void TestGetClassCodeSet()
//     {
//         var response = _client.GetClassCodeSet(new GetClassCodeSetRequest
//         {
//             ClassCodeSetId = 1
//         });
//
//         Utils.ToString(response);
//     }
//     
//     private static void TestUpdateClassCodeSet()
//     {
//         var response = _client.UpdateClassCodeSet(new UpdateClassCodeSetRequest
//         {
//             Id = 10,
//             Title = "ClassCodeSet1_updated",
//             VolumeId = "volume2",
//             UpdateUser = "김철민",
//             Description = "move_rename",
//             DirectoryName = "class_code_set1_updated",
//             LockTimeoutSec = 2,
//         });
//
//         Utils.ToString(response);
//     }
//     
//     private static void TestDeleteClassCodeSet()
//     {
//         var response = _client.DeleteClassCodeSet(new DeleteClassCodeSetRequest
//         {
//             ClassCodeSetId = 16,
//             LockTimeoutSec = 2,
//         });
//         
//         Utils.ToString(response);
//     }
// }