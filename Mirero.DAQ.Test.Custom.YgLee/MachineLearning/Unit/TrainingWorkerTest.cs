//using Grpc.Net.Client;
//using Mirero.DAQ.Domain.Common.Protos;
//using Mirero.DAQ.Domain.MachineLearning.Protos.V1;

//namespace Mirero.DAQ.Test.Custom.YgLee.MachineLearning.Unit;

//public class TrainingWorkerTest
//{
//    private static MachineLearningService.MachineLearningServiceClient _client;
//    public static void Test()
//    {
//        var channel = GrpcChannel.ForAddress("http://localhost:5002");
//        _client = new MachineLearningService.MachineLearningServiceClient(channel);

//        //TestCreateTrainingWorker();
//        //TestListTrainingWorkers();
//        //TestGetTrainingWorker();
//        //TestUpdateTrainingWorker();
//        TestDeleteTrainingWorker();
//    }

//    private static void TestDeleteTrainingWorker()
//    {
//        var deleteTrainingWorker = _client.RemoveTrainingWorker(new RemoveTrainingWorkerRequest
//        {
//            WorkerId = "ml_training_worker_1"
//        });

//        Utils.ToString(deleteTrainingWorker);
//    }

//    private static void TestCreateTrainingWorker()
//    {
//        _client.CreateTrainingWorker(new CreateTrainingWorkerRequest
//        {
//            TrainingWorker = new TrainingWorker
//            {
//                Id = "ml_training_worker_1",
//                TrainingType = "classification",
//                ServerId = "ml_server_1",
//                GpuCount = 1,
//                GpuMemory = 256000000,
//            }
//        });
//    }

//    private static void TestListTrainingWorkers()
//    {
//        var response = _client.ListTrainingWorkers(new ListTrainingWorkersRequest
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

//        var trainingWorkers = response.TrainingWorkers;
//        var list = trainingWorkers.AsEnumerable();

//        foreach (var trainingWorker in list)
//        {
//            Utils.ToString(trainingWorker);
//        }
//    }

//    //private static void TestGetTrainingWorker()
//    //{
//    //    var response = _client.GetTrainingWorker(new GetTrainingWorkerRequest
//    //    {
//    //        TrainingWorkerId = "ml_training_worker_1"
//    //    });

//    //    Utils.ToString(response);
//    //}

//    //private static void TestUpdateTrainingWorker()
//    //{
//    //    var response = _client.UpdateTrainingWorker(new TrainingWorker
//    //    {
//    //        Id = "ml_training_worker_1",
//    //        TrainingType = "multi_label_classification",
//    //        ServerId = "ml_server_1",
//    //        GpuCount = 2,
//    //        GpuMemory = 512000000,
//    //    });

//    //    Utils.ToString(response);
//    //}
//}