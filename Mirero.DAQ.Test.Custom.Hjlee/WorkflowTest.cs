using Grpc.Net.Client;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Serilog;
//using Mirero.DAQ.Application.Services;

namespace Mirero.DAQ.Test.Custom.Hjlee;

public class WorkflowTest
{
    private static WorkflowService.WorkflowServiceClient _workflowServiceClient;
    private static Serilog.ILogger _logger = Log.Logger;
    private const string MyWorkspace = @"D:\WorkflowTest";

    public WorkflowTest()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:5000");
        _workflowServiceClient = new WorkflowService.WorkflowServiceClient(channel);
    }

    public void Test()
    {
        /*
         * 1. Volume 생성
         * 2. Workflow 생성
         * 3. WorkflowVersion 생성
         * 4. BatchJob 생성
         */

        //ListVolumes();
        //CreateVolume();
        //UpdateVolume();
        //DeleteVolume();
        //ListServers();
        //CreateServer();
        //UpdateServer();
        //DeleteServer();
        //CreateRecipeWorker();
        //StartRecipeWorker();
        //StopRecipeWorker();
        //RemoveRecipeWorker();
    }

    private void ListVolumes()
    {
        _logger.Information("Show Volume List");

        try
        {
            var response = _workflowServiceClient.ListVolumes(new ListVolumesRequest()
            {
                QueryParameter = new Domain.Common.Protos.QueryParameter()
            }) ;

            foreach (var volume in response.Volumes)
            {
                Console.WriteLine(volume.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    private void CreateVolume()
    {
        _logger.Information("Create Volume");

        try
        {
            var volumeName = "Volume1";
            var response = _workflowServiceClient.CreateVolume(new Volume() 
            {
                Id = volumeName,
                Title = $"Title_{volumeName}",
                Uri = Path.Combine(MyWorkspace, volumeName),
                Usage = 1,
                Capacity = 100000000
            });

            Console.WriteLine(response.Id);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    private void UpdateVolume()
    {
        _logger.Information("Update Volume");

        try
        {
            var listVolumesResponse = _workflowServiceClient.ListVolumes(new ListVolumesRequest()
            {
                QueryParameter = new Domain.Common.Protos.QueryParameter()
            });

            var updateTarget = listVolumesResponse.Volumes[0];
            var response = _workflowServiceClient.UpdateVolume(new Volume()
            {
                Id = updateTarget.Id,
                Title = $"{updateTarget.Title}_Updated",
                Uri = updateTarget.Uri,
                Usage = 1,
                Capacity = updateTarget.Capacity  + 1
            }) ;

            Console.WriteLine($"[{response.Id}] Title: {response.Title}, Capacity: {response.Capacity}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    private void DeleteVolume()
    {
        _logger.Information("Delete Volume");

        try
        {
            var listVolumesResponse = _workflowServiceClient.ListVolumes(new ListVolumesRequest()
            {
                QueryParameter = new Domain.Common.Protos.QueryParameter()
            });

            var deleteTarget = listVolumesResponse.Volumes[0];

            var response = _workflowServiceClient.DeleteVolume(new DeleteVolumeRequest()
            {
                VolumeId = deleteTarget.Id
            });

            Console.WriteLine(response.Id);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    private void ListServers()
    {
        _logger.Information("Show Server List");

        try
        {
            var response = _workflowServiceClient.ListServers(new ListServersRequest()
            {
                QueryParameter = new Domain.Common.Protos.QueryParameter()
            });

            foreach (var volume in response.Servers)
            {
                Console.WriteLine(volume.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    private void CreateServer()
    {
        _logger.Information("Create Server");

        try
        {
            var serverName = "Server1";
            var response = _workflowServiceClient.CreateServer(new Server()
            {
                Id = serverName,
                Address = "192.168.100.208",
                OsType = "CentOS",
                OsVersion = "7",
                CpuCount = 1,
                CpuMemory = 10000
            });

            Console.WriteLine(response.Id);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    private void UpdateServer()
    {
        _logger.Information("Update Server");

        try
        {
            var listServersResponse = _workflowServiceClient.ListServers(new ListServersRequest()
            {
                QueryParameter = new Domain.Common.Protos.QueryParameter()
            });

            var updateTarget = listServersResponse.Servers[0];
            var response = _workflowServiceClient.UpdateServer(new Server()
            {
                Id = updateTarget.Id,
                CpuCount = 3,
                CpuMemory = 20000
            });

            Console.WriteLine($"[{response.Id}] CpuCount: {response.CpuCount}, CpuMemory: {response.CpuMemory}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    private void DeleteServer()
    {
        _logger.Information("Delete Server");

        try
        {
            var listServersResponse = _workflowServiceClient.ListServers(new ListServersRequest()
            {
                QueryParameter = new Domain.Common.Protos.QueryParameter()
            });

            var deleteTarget = listServersResponse.Servers[0];

            var response = _workflowServiceClient.DeleteServer(new DeleteServerRequest()
            {
                ServerId = deleteTarget.Id
            });

            Console.WriteLine(response.Id);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }
}
