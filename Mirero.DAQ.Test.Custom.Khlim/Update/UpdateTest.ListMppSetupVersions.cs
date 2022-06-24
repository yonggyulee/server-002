using Grpc.Net.Client;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Update.Protos.V1;
using MppUpdateService = Mirero.DAQ.Domain.Update.Protos.V1.MppUpdateService;

namespace Mirero.DAQ.Test.Custom.Khlim.Update;

public static partial class UpdateTest
{
    public static void ListMppSetupVersions()
    {
        // var signInChannel = GrpcChannel.ForAddress("http://localhost:5010");
        // var signInClient = new SignInService.SignInServiceClient(signInChannel);
        //
        // var signInResponse = signInClient.SignIn(new SignInRequest
        // {
        //     UserId = "administrator",
        //     Password = "mirero2816!"
        // });
        //
        // var headers = new Metadata();
        // headers.Add("Authorization", $"Bearer {signInResponse.AccessToken}");

        var updateChannel = GrpcChannel.ForAddress("http://localhost:5001");
        var updateClient = new MppUpdateService.MppUpdateServiceClient(updateChannel);

        var response = updateClient.ListMppSetupVersions(new ListMppSetupVersionsRequest
        {
            QueryParameter = new QueryParameter {PageIndex = 0, PageSize = 10}
        });

        Console.WriteLine(response);
    }
}