using System.IO;
using System.Linq;
using System.Threading;
using Google.Protobuf;
using Grpc.Core;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using GdsStatus = Mirero.DAQ.Domain.Gds.Constants.GdsStatus;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.GdsTest.Gds;

public partial class GdsServiceTest
{
    private const string DownloadTestPath = "D:/workspace/DownloadTestPath";

    [Fact]
    public async void T04_DownloadGdsStream_01()
    {
        if (!Directory.Exists(DownloadTestPath))
        {
            Directory.CreateDirectory(DownloadTestPath);
        }

        var superUserOption = await _GetAuthAsync();
        var tempDir = $"{@"C:\mirero\volumes"}/{Path.GetRandomFileName()}";
        var createVolumeRequest = new CreateVolumeRequest
        {
            Id = "DownloadStreamGdsVolumeId",
            Title = "DownloadGdsTestVolume",
            Type = "gds",
            Uri = tempDir,
            Capacity = 100000000,
            Description = "Test Volume"
        };

        var targetVolume = _volume.CreateVolume(createVolumeRequest, superUserOption);

        var createGdsRequest = new CreateGdsRequest
        {
            VolumeId = "DownloadStreamGdsVolumeId",
            Filename = "DownGdsTestFile",
            Extension = "gds",
            Properties = "1",
            Description = "DownStreamGds"
        };

        var targetGds = _client.CreateGds(createGdsRequest, superUserOption);

        var uri = $"{TestDataPath}/{createGdsRequest.Filename + "." + createGdsRequest.Extension}";
        await using var fs = new FileStream(uri, FileMode.Open, FileAccess.Read);
        var fileSize = fs.Length;
        using var call = _client.UploadGdsStream();
        const int chunkSize = 64 * 1024;
        var fileChunk = new byte[chunkSize];

        int readBytes;
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
        var uploadFileUri = Path.Combine(targetVolume.Uri, targetGds.GdsId.ToString(), createGdsRequest.Filename);
        Assert.True(File.Exists(uploadFileUri));

        uri = $"{DownloadTestPath}/{createGdsRequest.Filename + "." + createGdsRequest.Extension}";
        if (File.Exists(uri))
        {
            File.Delete(uri);
        }

        var downloadFs = new FileStream(uri, FileMode.CreateNew);

        var listGdsesResponse = await List_Gds();
        var gds = listGdsesResponse.Gdses.SingleOrDefault(x => x.Filename == createGdsRequest.Filename);
        var downLoadGdsRequest = new DownloadGdsStreamRequest
        {
            GdsId = gds.Id,
            ChunkSize = chunkSize,
            LockTimeoutSec = 3
        };
        using var stream = _client.DownloadGdsStream(downLoadGdsRequest);
        while (await stream.ResponseStream.MoveNext())
        {
            await downloadFs
                    .WriteAsync(
                        stream.ResponseStream.Current.ToByteArray(),
                        CancellationToken.None
                    )
                ;
        }
    }
}