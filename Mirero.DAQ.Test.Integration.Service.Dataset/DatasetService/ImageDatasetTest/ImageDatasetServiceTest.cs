using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Xunit;
using Xunit.Abstractions;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.ImageDatasetTest;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("DatasetIntegrationTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class ImageDatasetServiceTest
{
    private readonly ApiServiceFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly IServiceScope _scope;
    private readonly ImageDatasetService.ImageDatasetServiceClient _client;
    //private readonly AccountService.AccountServiceClient _accountServiceClientclient;

    public ImageDatasetServiceTest(DatasetApiServiceFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _scope = fixture.Host!.Services.CreateScope();
        //_accountServiceClientclient = _fixture.AccountService;
        _client = new ImageDatasetService.ImageDatasetServiceClient(_fixture.GrpcChannel);
    }

    private async Task<CallOptions> _GetAuthAsync()
    {
        Assert.True(await _fixture.SignInAsync("administrator", "mirero2816!"));

        return _fixture.OptionsWithAuthHeader();
    }
}