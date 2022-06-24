using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Inference.Protos.V1;
using Xunit;
using Xunit.Abstractions;

namespace Mirero.DAQ.Test.Integration.Service.Inference.InferenceService.Worker;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("InferenceIntegrationTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class WorkerServiceTest
{
    private readonly ApiServiceFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly IServiceScope _scope;
    private readonly WorkerService.WorkerServiceClient _client;

    public WorkerServiceTest(InferenceApiServiceFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _scope = fixture.Host!.Services.CreateScope();
        _client = new WorkerService.WorkerServiceClient(_fixture.GrpcChannel);
    }
}