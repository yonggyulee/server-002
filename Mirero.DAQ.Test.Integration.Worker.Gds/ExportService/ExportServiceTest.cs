using System.Diagnostics.CodeAnalysis;
using Grpc.Net.Client;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Worker.Gds.ExportService;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("GdsWorkerIntegrationTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class ExportServiceTest
{
    private readonly GrpcChannel _channel;
    private readonly Domain.Gds.Protos.V1.Worker.ExportService.ExportServiceClient _client;

    public ExportServiceTest()
    {
        _channel = GrpcChannel.ForAddress("http://192.168.70.32:50050", new GrpcChannelOptions
        {
            Credentials = Grpc.Core.ChannelCredentials.Insecure
        });

        _client = new Domain.Gds.Protos.V1.Worker.ExportService.ExportServiceClient(_channel);
    }
}