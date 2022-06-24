using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Test.Integration.Service;
using Xunit;
using Xunit.Abstractions;

namespace Mirero.DAQ.Test.Integration.Worker.Gds.ClipService;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("GdsWorkerIntegrationTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class ClipServiceTest
{
    private readonly GrpcChannel _channel;
    private readonly Domain.Gds.Protos.V1.Worker.ClipService.ClipServiceClient _client;

    public ClipServiceTest()
    {
        _channel = GrpcChannel.ForAddress("http://192.168.70.32:50050", new GrpcChannelOptions
        {
            Credentials = Grpc.Core.ChannelCredentials.Insecure
        });

        _client = new Domain.Gds.Protos.V1.Worker.ClipService.ClipServiceClient(_channel);
    }
}