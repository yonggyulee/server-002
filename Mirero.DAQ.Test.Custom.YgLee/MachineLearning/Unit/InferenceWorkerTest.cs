//using Grpc.Net.Client;
//using Mirero.DAQ.Domain.Common.Protos;
//using Mirero.DAQ.Domain.MachineLearning.Protos.V1;

//namespace Mirero.DAQ.Test.Custom.YgLee.MachineLearning.Unit;
//public class InferenceWorkerTest
//{
//    private static MachineLearningService.MachineLearningServiceClient _client;
//    public static void Test()
//    {
//        var channel = GrpcChannel.ForAddress("http://localhost:5002");
//        _client = new MachineLearningService.MachineLearningServiceClient(channel);

//        //TestCreateInferenceWorker();
//        //TestListInferenceWorkers();
//        //TestGetInferenceWorker();
//        //TestUpdateInferenceWorker();
//        TestDeleteInferenceWorker();
//    }

//    private static void TestDeleteInferenceWorker()
//    {
//        var deleteInferenceWorker = _client.RemoveInferenceWorker(new RemoveInferenceWorkerRequest
//        {
//            WorkerId = "ml_inferenceWorker_8"
//        });

//        Utils.ToString(deleteInferenceWorker);
//    }

//    private static void TestCreateInferenceWorker()
//    {
//        _client.CreateInferenceWorker(new CreateInferenceWorkerRequest
//        {
//            InferenceWorker = new InferenceWorker
//            {
//                Id = "ml_inferenceWorker_1",
//                ServingType = "tf",
//                ServerId = "ml_server_1",
//                CpuCount = 1,
//                CpuMemory = 512000000,
//                GpuCount = 1,
//                GpuMemory = 256000000,
//            }
//        });
//    }

//    private static void TestListInferenceWorkers()
//    {
//        var response = _client.ListInferenceWorkers(new ListInferenceWorkersRequest
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

//        var inferenceWorkers = response.InferenceWorkers;
//        var list = inferenceWorkers.AsEnumerable();

//        foreach (var inferenceWorker in list)
//        {
//            Utils.ToString(inferenceWorker);
//        }
//    }

//    //private static void TestGetInferenceWorker()
//    //{
//    //    var response = _client.GetInferenceWorker(new GetInferenceWorkerRequest
//    //    {
//    //        InferenceWorkerId = "ml_inferenceWorker_1"
//    //    });

//    //    Utils.ToString(response);
//    //}

//    //private static void TestUpdateInferenceWorker()
//    //{
//    //    var response = _client.UpdateInferenceWorker(new InferenceWorker
//    //    {
//    //        Id = "ml_inferenceWorker_1",
//    //        ServingType = "torch",
//    //        ServerId = "ml_server_1",
//    //        GpuCount = 2,
//    //        GpuMemory = 512000000,
//    //    });

//    //    Utils.ToString(response);
//    //}
//}
