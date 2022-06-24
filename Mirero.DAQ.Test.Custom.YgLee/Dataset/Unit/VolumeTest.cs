// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class VolumeTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     private static readonly string DataPath = Path.Combine(
//         Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName!,
//         "test_data\\dataset_file_storage");
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//
//         TestCreateVolume();
//         // TestListVolumes();
//         //TestGetVolume();
//         // TestGetVolumeByName();
//         // TestUpdateVolume();
//         // TestDeleteVolume();
//     }
//
//     private static void TestDeleteVolume()
//     {
//          var deleteVolume = _client.DeleteVolume(new DeleteVolumeRequest
//          {
//              VolumeId = "volume1"
//          });
//          
//          Utils.ToString(deleteVolume);
//     }
//
//     private static void TestCreateVolume()
//     {
//         _client.CreateVolume(new CreateVolumeRequest
//         {
//             Id = "volume1",
//             Title = "Volume1",
//             Type = "image",
//             Uri = Path.Combine(DataPath, "volume1"),
//             Capacity = 100000000
//         });
//     }
//
//     private static void TestListVolumes()
//     {
//         var response = _client.ListVolumes(new ListVolumesRequest
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
//         var volumes = response.Volumes;
//         var list = volumes.AsEnumerable();
//     
//         foreach (var volume in list)
//         {
//             Utils.ToString(volume);
//         }
//     }
//     
//     private static void TestGetVolume()
//     {
//         var response = _client.GetVolume(new GetVolumeRequest
//         {
//             VolumeId = "volume1"
//         });
//
//         Utils.ToString(response);
//     }
//     
//     // private static void TestGetVolumeByName()
//     // {
//     //     var response = _client.GetVolumeByName(new GetVolumeByNameRequest
//     //     {
//     //         Name = "volume2"
//     //     });
//     //
//     //     Console.WriteLine($"{response.Id}, {response.Name}, {response.Type}, {response.Uri}, {response.Usage}, {response.Capacity}");
//     // }
//     
//     private static void TestUpdateVolume()
//     {
//         var response = _client.UpdateVolume(new UpdateVolumeRequest
//         {
//             Id = "volume2_updated",
//             Type = "test",
//             Uri = Path.Combine(DataPath, "volume2_updated"),
//         });
//
//         Utils.ToString(response);
//     }
//     
// }