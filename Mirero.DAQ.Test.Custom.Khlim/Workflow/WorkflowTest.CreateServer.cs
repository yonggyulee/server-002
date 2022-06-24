using Grpc.Core;
using Grpc.Net.Client;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Update.Protos.V1;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Test.Custom.Khlim.Workflow;

public static class WorkflowTest
{
    public static void Test_CreateServer()
    {
        var signInChannel = GrpcChannel.ForAddress("http://localhost:5010");
        var signInClient = new SignInService.SignInServiceClient(signInChannel);
     
        var signInResponse = signInClient.SignIn(new SignInRequest
        {
            UserId = "administrator",
            Password = "mirero2816!"
        });
        
        var headers = new Metadata();
        headers.Add("Authorization", $"Bearer {signInResponse.AccessToken}");
        
        var channel = GrpcChannel.ForAddress("http://localhost:5020");
        var client = new ServerService.ServerServiceClient(channel);
       
        var response =  client.CreateServer(new CreateServerRequest()
        {
            Id = "Server1",
            Address = "192.168.70.32",
            OsType = "OS",
            OsVersion = "1.0",
            CpuCount = 100,
            CpuMemory = 10000000,
            GpuName = "Gpu",
            GpuCount = 100,
        }, headers);
        
    }
}