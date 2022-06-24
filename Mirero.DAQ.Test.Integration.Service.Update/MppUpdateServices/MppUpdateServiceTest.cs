using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using MppUpdateService = Mirero.DAQ.Domain.Update.Protos.V1.MppUpdateService;

namespace Mirero.DAQ.Test.Integration.Service.Update.MppUpdateServices;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("UpdateIntegrationTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class MppUpdateServiceTest
{
    private readonly ApiServiceFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly IServiceScope _scope;
    private readonly MppUpdateService.MppUpdateServiceClient _mppUpdateServiceClient;
    
    public MppUpdateServiceTest(UpdateApiServiceFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _scope = fixture.Host!.Services.CreateScope();
        _mppUpdateServiceClient = new MppUpdateService.MppUpdateServiceClient(_fixture.GrpcChannel);
    }
    
    private async Task<CallOptions> _GetAuthAsync()
    {
        Assert.True(await _fixture.SignInAsync("administrator", "Mirero2816!"));

        return _fixture.OptionsWithAuthHeader();
    }
}