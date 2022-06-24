//using Grpc.Net.Client;
//using Mirero.DAQ.Domain.Common.Protos;
//using Mirero.DAQ.Domain.MachineLearning.Protos.V1;

//namespace Mirero.DAQ.Test.Custom.YgLee.MachineLearning.Unit;
//public class ServerTest
//{
//    private static MachineLearningService.MachineLearningServiceClient _client;
//    public static void Test()
//    {
//        var channel = GrpcChannel.ForAddress("http://localhost:5002");
//        _client = new MachineLearningService.MachineLearningServiceClient(channel);

//        //TestCreateServer();
//        //TestListServers();
//        //TestGetServer();
//        //TestUpdateServer();
//        TestDeleteServer();
//    }

//    private static void TestDeleteServer()
//    {
//        var deleteServer = _client.DeleteServer(new DeleteServerRequest
//        {
//            ServerId = "ml_server_3"
//        });

//        Utils.ToString(deleteServer);
//    }

//    private static void TestCreateServer()
//    {
//        _client.CreateServer(new Server
//        {
//            Id = "ml_server_1",
//            Address = "192.168.100.208",
//            OsType = "CentOS",
//            OsVersion = "7",
//            CpuCount = 1,
//            CpuMemory = 10000
//        });
//    }

//    private static void TestListServers()
//    {
//        var response = _client.ListServers(new ListServersRequest
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 10
//            }
//        });

//        Console.WriteLine($"PageIndex : {response.PageResult.PageIndex}");
//        Console.WriteLine($"PageSize : {response.PageResult.PageSize}");
//        Console.WriteLine($"Count : {response.PageResult.Count}");

//        var servers = response.Servers;
//        var list = servers.AsEnumerable();

//        foreach (var server in list)
//        {
//            Utils.ToString(server);
//        }
//    }

//    //private static void TestGetServer()
//    //{
//    //    var response = _client.GetServer(new GetServerRequest
//    //    {
//    //        ServerId = "ml_server_1"
//    //    });

//    //    Utils.ToString(response);
//    //}

//    private static void TestUpdateServer()
//    {
//        var response = _client.UpdateServer(new Server
//        {
//            Id = "ml_server_3",
//            Address = "192.168.100.208",
//            OsType = "Ubuntu",
//            OsVersion = "20.04",
//            CpuCount = 2,
//            CpuMemory = 1024000000
//        });

//        Utils.ToString(response);
//    }
//}