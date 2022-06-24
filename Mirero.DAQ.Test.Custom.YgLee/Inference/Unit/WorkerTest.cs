using Grpc.Net.Client;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Test.Custom.YgLee.Inference.Unit;
public class WorkerTest
{
    private static WorkerService.WorkerServiceClient _client;
    public static void Test()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:5002");
        _client = new WorkerService.WorkerServiceClient(channel);

        //TestCreateWorker();
        //TestListWorkers();
        //TestGetWorker();
        //TestUpdateWorker();
        TestDeleteWorker();
    }

    private static void TestDeleteWorker()
    {
        var deleteWorker = _client.RemoveWorker(new RemoveWorkerRequest
        {
            WorkerId = "ml_inferenceWorker_8"
        });

        Utils.ToString(deleteWorker);
    }

    private static void TestCreateWorker()
    {
        _client.CreateWorker(new CreateWorkerRequest
        {
            Id = "ml_inferenceWorker_1",
            ServingType = "tf",
            ServerId = "ml_server_1",
            CpuCount = 1,
            CpuMemory = 512000000,
            GpuCount = 1,
            GpuMemory = 256000000,
        });
    }

    private static void TestListWorkers()
    {
        var response = _client.ListWorkers(new ListWorkersRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10
            }
        });

        Console.WriteLine($"PageIndex : {response.PageResult.PageIndex}");
        Console.WriteLine($"PageSize : {response.PageResult.PageSize}");
        Console.WriteLine($"Count : {response.PageResult.Count}");

        var inferenceWorkers = response.Workers;
        var list = inferenceWorkers.AsEnumerable();

        foreach (var inferenceWorker in list)
        {
            Utils.ToString(inferenceWorker);
        }
    }

    //private static void TestGetWorker()
    //{
    //    var response = _client.GetWorker(new GetWorkerRequest
    //    {
    //        WorkerId = "ml_inferenceWorker_1"
    //    });

    //    Utils.ToString(response);
    //}

    //private static void TestUpdateWorker()
    //{
    //    var response = _client.UpdateWorker(new Worker
    //    {
    //        Id = "ml_inferenceWorker_1",
    //        ServingType = "torch",
    //        ServerId = "ml_server_1",
    //        GpuCount = 2,
    //        GpuMemory = 512000000,
    //    });

    //    Utils.ToString(response);
    //}
}
