using System.Diagnostics.CodeAnalysis;
using System.IO;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using Google.Protobuf;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Gds.Constants;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.GdsTest.Gds;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("GdsIntegrationTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class GdsServiceTest
{
    private readonly ApiServiceFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly IServiceScope _scope;
    private readonly Mirero.DAQ.Domain.Gds.Protos.V1.GdsService.GdsServiceClient _client;
    private readonly Mirero.DAQ.Domain.Gds.Protos.V1.VolumeService.VolumeServiceClient _volume;

    public GdsServiceTest(GdsApiServiceFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _scope = fixture.Host!.Services.CreateScope();
        _client = new Mirero.DAQ.Domain.Gds.Protos.V1.GdsService.GdsServiceClient(_fixture.GrpcChannel);
        _volume = new Mirero.DAQ.Domain.Gds.Protos.V1.VolumeService.VolumeServiceClient(_fixture.GrpcChannel);
    }

    private async Task<CallOptions> _GetAuthAsync()
    {
        Assert.True(await _fixture.SignInAsync("administrator", "mirero2816!"));

        return _fixture.OptionsWithAuthHeader();
    }
    
    public async Task<ListGdsesResponse> List_Gds()
    {
        var superUserOption = await _GetAuthAsync();
    
        var listGdsesRequest = new ListGdsesRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10
            }
        };

        var listGdses = await _client.ListGdsesAsync(listGdsesRequest, superUserOption);

        return listGdses;
    }
    
    public async Task UploadGdsStream()
    {
        var superUserOption = await _GetAuthAsync();
        var tempDir = $"{@"C:\mirero\volumes"}/{Path.GetRandomFileName()}";
        var createVolumeRequest = new CreateVolumeRequest
        {
            Id = "GdsServiceTest",
            Title = "GdsServiceTestVolume",
            Type = "gds",
            Uri = tempDir,
            Capacity = 100000000,
            Description = "Test Volume"
        };

        var targetVolume = _volume.CreateVolume(createVolumeRequest, superUserOption);

        var createGdsRequest = new CreateGdsRequest
        {
            VolumeId = "GdsServiceTest",
            Filename = "UploadGdsTestFile",
            Extension = "gds",
            Properties = "1",
            Description = "UploadStreamGds"
        };

        var targetGds = _client.CreateGds(createGdsRequest, superUserOption);

        var uri = $"{TestDataPath}/{createGdsRequest.Filename + "." + createGdsRequest.Extension}";

        await using var fs = new FileStream(uri, FileMode.Open, FileAccess.Read);
        var fileSize = fs.Length;
        using var call = _client.UploadGdsStream();
        const int chunkSize = 64 * 1024;
        var fileChunk = new byte[chunkSize];

        var readBytes = 0;
        var sentBytes = 0;
        var chunkNum = 0;
        while ((readBytes = fs.Read(fileChunk, 0, chunkSize)) != 0)
        {
            await call.RequestStream.WriteAsync(new UploadGdsStreamRequest()
            {
                GdsId = targetGds.GdsId,
                DataInfo = new DataInfo
                {
                    ChunkNum = chunkNum,
                    ChunkSize = readBytes,
                    FileSize = fileSize,
                    Filename = createGdsRequest.Filename
                },
                Buffer = ByteString.CopyFrom(fileChunk, 0, readBytes),
            });
            chunkNum++;
            sentBytes += readBytes;

            _output.WriteLine("{0}/{1},  {2}%", sentBytes, fileSize, GdsStatus.Process(sentBytes, fileSize));
        }

        await call.RequestStream.CompleteAsync();
        await call;
    }
}