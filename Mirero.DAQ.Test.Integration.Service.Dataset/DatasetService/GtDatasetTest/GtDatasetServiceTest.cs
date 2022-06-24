using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Xunit;
using Xunit.Abstractions;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("DatasetIntegrationTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class GtDatasetServiceTest
{
    private readonly ApiServiceFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly IServiceScope _scope;
    private readonly GtDatasetService.GtDatasetServiceClient _client;
    //private readonly AccountService.AccountServiceClient _accountServiceClientclient;
    private readonly VolumeService.VolumeServiceClient _volumeServiceClient;
    private readonly ImageDatasetService.ImageDatasetServiceClient _imageDatasetServiceClient;
    private readonly ClassCodeService.ClassCodeServiceClient _classCodeServiceClient;

    public GtDatasetServiceTest(DatasetApiServiceFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _scope = fixture.Host!.Services.CreateScope();
        //_accountServiceClientclient = _fixture.AccountService;
        _client = new GtDatasetService.GtDatasetServiceClient(_fixture.GrpcChannel);
        _volumeServiceClient = new VolumeService.VolumeServiceClient(_fixture.GrpcChannel);
        _imageDatasetServiceClient = new ImageDatasetService.ImageDatasetServiceClient(_fixture.GrpcChannel);
        _classCodeServiceClient = new ClassCodeService.ClassCodeServiceClient(_fixture.GrpcChannel);
        //_CreateNeedData();
    }

    private async Task<CallOptions> _GetAuthAsync()
    {
        Assert.True(await _fixture.SignInAsync("administrator", "mirero2816!"));

        return _fixture.OptionsWithAuthHeader();
    }
}