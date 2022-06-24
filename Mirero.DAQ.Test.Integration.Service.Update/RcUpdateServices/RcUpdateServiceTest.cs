using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using RcUpdateService = Mirero.DAQ.Domain.Update.Protos.V1.RcUpdateService;

namespace Mirero.DAQ.Test.Integration.Service.Update.RcUpdateServices;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("UpdateIntegrationTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class RcUpdateServiceTest
{
    private readonly ApiServiceFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly IServiceScope _scope;
    private readonly RcUpdateService.RcUpdateServiceClient _rcUpdateServiceClient;

    public RcUpdateServiceTest(UpdateApiServiceFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _scope = fixture.Host!.Services.CreateScope();
        _rcUpdateServiceClient = new RcUpdateService.RcUpdateServiceClient(_fixture.GrpcChannel);
    }
    
    private async Task<CallOptions> _GetAuthAsync()
    {
        Assert.True(await _fixture.SignInAsync("administrator", "Mirero2816!"));

        return _fixture.OptionsWithAuthHeader();
    }
}