// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class QueryTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     private static readonly string DownloadFolder = DatasetClientTest.DownloadFolder;
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//         
//         // EqualsTest();
//         // IsGreaterThanTest();
//         // IsGreaterThanOrEqualsTest();
//         // IsGreaterLessThanTest_Datetime();
//         //IsLikeTest();
//         EtcTest();
//     }
//     
//     private static void EqualsTest()
//     {
//         var response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10,
//                 Where = "Title == \"Dataset5\""
//             }
//         });
//         
//         Utils.ToString(response);
//         
//         response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10,
//                 Where = "Title != \"Dataset5\""
//             }
//         });
//         
//         Utils.ToString(response);
//     }
//     
//     private static void IsGreaterThanTest()
//     {
//         var response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10,
//                 Where = "Id > 2 && Id < 5"
//             }
//         });
//         
//         Utils.ToString(response);
//         
//         response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10,
//                 Where = "Id > 3 and Id < 7"
//             }
//         });
//         
//         Utils.ToString(response);
//     }
//     
//     private static void IsGreaterThanOrEqualsTest()
//     {
//         var response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10,
//                 Where = "Id >= 2 && Id <= 5"
//             }
//         });
//         
//         Utils.ToString(response);
//         
//         response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10,
//                 Where = "Id >= 3 and Id <= 7"
//             }
//         });
//         
//         Utils.ToString(response);
//     }
//     
//     private static void IsGreaterLessThanTest_Datetime()
//     {
//         Console.WriteLine("IsGreaterLessThanTest_Datetime");
//         // var time = DateTime.SpecifyKind(new DateTime(2022, 2, 25, 10, 20, 42), DateTimeKind.Utc).ToTimestamp();
//         // Console.WriteLine(time);
//         // Console.WriteLine(time.ToString());
//         // var response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         // {
//         //     PageIndex = 0,
//         //     PageSize = 20,
//         //     Query = "CreateDate < DateTime.SpecifyKind(new DateTime(2022, 2, 25, 10, 20, 42), DateTimeKind.Utc).ToTimestamp()"
//         // });
//         var response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10,
//                 Where = "(CreateDate > DateTime.SpecifyKind(Convert.ToDateTime(\"2022-02-25 22:07:04.9192538\"), DateTimeKind.Utc)"
//                         + " and Id > 4) or (Id == 1)"
//             }
//         });
//         
//         // var response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         // {
//         //     PageIndex = 0,
//         //     PageSize = 20,
//         //     Query = "CreateDate < DateTime(Fri, 10 May 2019 11:03:17 GMT)"
//         // });
//         
//         // DateTime(Fri, 10 May 2019 11:03:17 GMT)
//
//         Utils.ToString(response);
//     }
//     
//     private static void StartWithsTest()
//     {
//         var response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10,
//                 Where = "Title.StartsWith(\"Dataset5\")"
//             }
//         });
//         
//         Utils.ToString(response);
//     }
//     
//     private static void EndsWithTest()
//     {
//         var response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10,
//                 Where = "Title.EndsWith(\"Dataset5\")"
//                 
//             }
//         });
//         
//         Utils.ToString(response);
//     }
//     
//     private static void IsLikeTest()
//     {
//         var response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10,
//                 Where = "d => EF.Functions.Like(d.Title, \"___test___\")"
//             }
//         });
//         
//         Utils.ToString(response);
//     }
//
//     private static void EtcTest()
//     {
//         var response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10,
//                 Where = "Convert.ToString(Id).StartsWith(\"1\")"
//             }
//         });
//
//         Utils.ToString(response);
//     }
// }