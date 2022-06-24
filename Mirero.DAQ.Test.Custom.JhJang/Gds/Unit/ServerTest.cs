using Grpc.Net.Client;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Test.Custom.JhJang.Gds.Unit;

public class ServerTest
{
    private static ServerService.ServerServiceClient _client;
    private static readonly string DataPath = Path.Combine(
        Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName!,
        "test_data\\dataset_file_storage");
    public static void Test()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:5002");
        _client = new ServerService.ServerServiceClient(channel);

        TestCreateServer();
        //TestDeleteVolume();
        //TestListVolumes();
        TestUpdateServer();
    }
    private static void TestCreateServer()
    {
        _client.CreateServer(new CreateServerRequest()
        {
            Id = "Server1",
            Address = "192.168.100.208",
            OsType = "CentOS",
            OsVersion = "7",
            CpuCount = 1,
            CpuMemory = 10000
        });
    }
    //private static void TestDeleteVolume()
    //{
    //    _client.DeleteVolume(new DeleteVolumeRequest
    //    {
    //        VolumeId = "volume1"
    //    });
    //    //var deleteVolume = _client.DeleteVolume()
    //    //{
    //    //    VolumeId = "volume1"
    //    //});

    //    //Utils.ToString(deleteVolume);
    //}



    //private static void TestListVolumes()
    //{
    //    var response = _client.ListVolumes(new ListVolumesRequest
    //    {
    //        QueryParameter = new QueryParameter
    //        {
    //            PageIndex = 0,
    //            PageSize = 10
    //        }
    //    });

    //    Console.WriteLine($"PageIndex : {response.PageResult.PageIndex}");
    //    Console.WriteLine($"PageSize : {response.PageResult.PageSize}");
    //    Console.WriteLine($"Count : {response.PageResult.Count}");

    //    var volumes = response.Volumes;
    //    var list = volumes.AsEnumerable();

    //    foreach (var volume in list)
    //    {
    //        if (volume != null) Utils.ToString(volume);
    //    }
    //}
    private static void TestUpdateServer()
    {
        var response = _client.UpdateServer(new UpdateServerRequest
        {
            Id = "Server1",
            Address = "192.168.200.408",
            OsType = "CentOS",
            OsVersion = "8",
            CpuCount = 3,
            CpuMemory = 30000
        });

        Utils.ToString(response);
    }
    //private static void TestGetVolume()
    //{
    //    var response = _client.GetVolume(new GetVolumeRequest
    //    {
    //        VolumeId = "volume1"
    //    });

    //    Utils.ToString(response);
    //}

    //// private static void TestGetVolumeByName()
    //// {
    ////     var response = _client.GetVolumeByName(new GetVolumeByNameRequest
    ////     {
    ////         Name = "volume2"
    ////     });
    ////
    ////     Console.WriteLine($"{response.Id}, {response.Name}, {response.Type}, {response.Uri}, {response.Usage}, {response.Capacity}");
    //// }
}