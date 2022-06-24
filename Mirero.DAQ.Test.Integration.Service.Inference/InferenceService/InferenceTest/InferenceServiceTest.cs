using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Inference.Protos.V1;
using Xunit;
using Xunit.Abstractions;

namespace Mirero.DAQ.Test.Integration.Service.Inference.InferenceService.InferenceTest;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("InferenceIntegrationTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class InferenceServiceTest
{
    private readonly ApiServiceFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly IServiceScope _scope;
    private readonly Domain.Inference.Protos.V1.InferenceService.InferenceServiceClient _client;
    private readonly ModelService.ModelServiceClient _modelServiceClient;

    private static readonly string? CurrentPath =
        Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;

    public InferenceServiceTest(InferenceApiServiceFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _scope = fixture.Host!.Services.CreateScope();
        _client = new Domain.Inference.Protos.V1.InferenceService.InferenceServiceClient(_fixture.GrpcChannel);
        _modelServiceClient = new ModelService.ModelServiceClient(_fixture.GrpcChannel);
    }

    private async Task<CallOptions> _GetAuthAsync()
    {
        Assert.True(await _fixture.SignInAsync("administrator", "mirero2816!"));

        return _fixture.OptionsWithAuthHeader();
    }
}