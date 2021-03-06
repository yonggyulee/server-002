//using Grpc.Net.Client;
//using Mirero.DAQ.Domain.Common.Protos;
//using Mirero.DAQ.Domain.MachineLearning.Protos.V1;

//namespace Mirero.DAQ.Test.Custom.YgLee.MachineLearning.Unit;

//public class ModelTest
//{
//    private static MachineLearningService.MachineLearningServiceClient _client;
//    public static void Test()
//    {
//        var channel = GrpcChannel.ForAddress("http://localhost:5002");
//        _client = new MachineLearningService.MachineLearningServiceClient(channel);

//        //TestCreateModel();
//        TestListModels();
//        //TestGetModel();
//        //TestUpdateModel();
//        //TestDeleteModel();
//    }

//    private static void TestDeleteModel()
//    {
//        var deleteModel = _client.DeleteModel(new DeleteModelRequest
//        {
//            ModelId = 4
//        });

//        Utils.ToString(deleteModel);
//    }

//    private static void TestCreateModel()
//    {
//        _client.CreateModel(new Model
//        {
//            VolumeId = "ml_volume_2",
//            TaskName = "image_classification",
//            NetworkName = "resnet",
//            ModelName = "image_classification_model_resnet_1"
//        });
//    }

//    private static void TestListModels()
//    {
//        var response = _client.ListModels(new ListModelsRequest
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

//        var models = response.Models;
//        var list = models.AsEnumerable();

//        foreach (var model in list)
//        {
//            Utils.ToString(model);
//        }
//    }

//    //private static void TestGetModel()
//    //{
//    //    var response = _client.GetModel(new GetModelRequest
//    //    {
//    //        ModelId = 4
//    //    });

//    //    Utils.ToString(response);
//    //}

//    private static void TestUpdateModel()
//    {
//        var response = _client.UpdateModel(new Model
//        {
//            Id = 4,
//            VolumeId = "ml_volume_3",
//            TaskName = "image_classification",
//            NetworkName = "denesnet",
//            ModelName = "image_classification_model_resnet_1"
//        });

//        Utils.ToString(response);
//    }
//}