using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Inference.Protos.V1;
using Xunit;
using Xunit.Abstractions;

namespace Mirero.DAQ.Test.Integration.Service.Inference.InferenceService.VolumeTest;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("InferenceIntegrationTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class VolumeServiceTest
{
    private readonly ApiServiceFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly IServiceScope _scope;
    private readonly VolumeService.VolumeServiceClient _client;

    public VolumeServiceTest(InferenceApiServiceFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _scope = fixture.Host!.Services.CreateScope();
        _client = new VolumeService.VolumeServiceClient(_fixture.GrpcChannel);
    }
}