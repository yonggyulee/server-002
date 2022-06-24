using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Xunit;
using Xunit.Abstractions;

namespace Mirero.DAQ.Test.Integration.Service.Inference.InferenceService.ModelTest;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("InferenceIntegrationTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class ModelServiceTest
{
    private readonly ApiServiceFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly IServiceScope _scope;
    private readonly Domain.Inference.Protos.V1.ModelService.ModelServiceClient _client;

    public ModelServiceTest(InferenceApiServiceFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _scope = fixture.Host!.Services.CreateScope();
        _client = new Domain.Inference.Protos.V1.ModelService.ModelServiceClient(_fixture.GrpcChannel);
    }

    private async Task<CallOptions> _GetAuthAsync()
    {
        Assert.True(await _fixture.SignInAsync("administrator", "mirero2816!"));

        return _fixture.OptionsWithAuthHeader();
    }
}