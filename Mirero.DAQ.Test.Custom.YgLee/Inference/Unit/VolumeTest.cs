using Grpc.Net.Client;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Test.Custom.YgLee.Inference.Unit;
public class VolumeTest
{
    private static VolumeService.VolumeServiceClient _client;
    public static void Test()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:5002");
        _client = new VolumeService.VolumeServiceClient(channel);

        //TestCreateVolume();
        TestListVolumes();
        //TestGetVolume();
        //TestUpdateVolume();
        //TestDeleteVolume();
    }

    private static void TestDeleteVolume()
    {
        var deleteVolume = _client.DeleteVolume(new DeleteVolumeRequest
        {
            VolumeId = "ml_volume_1"
        });

        Utils.ToString(deleteVolume);
    }

    private static void TestCreateVolume()
    {
        _client.CreateVolume(new CreateVolumeRequest
        {
            Id = "ml_volume_1",
            Title = "ML_Volume1",
            Type = "classification",
            Uri = "D:\\workspace\\daq-server\\Src\\dataset_file_storage\\ml_volume1",
            Capacity = 100000000
        });
    }

    private static void TestListVolumes()
    {
        var response = _client.ListVolumes(new ListVolumesRequest
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

        var volumes = response.Volumes;
        var list = volumes.AsEnumerable();

        foreach (var volume in list)
        {
            Utils.ToString(volume);
        }
    }

    //private static void TestGetVolume()
    //{
    //    var response = _client.GetVolume(new GetVolumeRequest
    //    {
    //        VolumeId = "ml_volume_1"
    //    });

    //    Utils.ToString(response);
    //}

    private static void TestUpdateVolume()
    {
        var response = _client.UpdateVolume(new UpdateVolumeRequest
        {
            Id = "ml_volume_1",
            Title = "ML_Volume1",
            Type = "test",
            Uri = "D:\\workspace\\daq-server\\Src\\test_data\\machine_learning_file_storage\\ml_volume1_updated",
        });

        Utils.ToString(response);
    }
}
