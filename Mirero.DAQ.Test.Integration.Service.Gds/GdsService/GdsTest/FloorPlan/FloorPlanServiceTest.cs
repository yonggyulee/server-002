using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.GdsTest.FloorPlan;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("GdsIntegrationTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class FloorPlanServiceTest
{
    private readonly ApiServiceFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly IServiceScope _scope;
    private readonly Mirero.DAQ.Domain.Gds.Protos.V1.GdsService.GdsServiceClient _client;

    public FloorPlanServiceTest(GdsApiServiceFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _scope = fixture.Host!.Services.CreateScope();
        _client = new Mirero.DAQ.Domain.Gds.Protos.V1.GdsService.GdsServiceClient(_fixture.GrpcChannel);
    }

    private async Task<CallOptions> _GetAuthAsync()
    {
        Assert.True(await _fixture.SignInAsync("administrator", "mirero2816!"));

        return _fixture.OptionsWithAuthHeader();
    }
}