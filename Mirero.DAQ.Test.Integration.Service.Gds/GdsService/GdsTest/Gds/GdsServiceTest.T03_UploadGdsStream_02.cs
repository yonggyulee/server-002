using System.IO;
using Google.Protobuf;
using Grpc.Core;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using GdsStatus = Mirero.DAQ.Domain.Gds.Constants.GdsStatus;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.GdsTest.Gds;

public partial class GdsServiceTest
{
    [Fact]
    public async void T03_UploadGdsStream_02()
    {
        var superUserOption = await _GetAuthAsync();
        var tempDir = $"{@"C:\mirero\volumes"}/{Path.GetRandomFileName()}";
        var createVolumeRequest = new CreateVolumeRequest
        {
            Id = "UploadStreamGdsVolumeId_02",
            Title = "UploadGdsTestVolume_02",
            Type = "gds",
            Uri = tempDir,
            Capacity = 100000000,
            Description = "Test Volume"
        };

        var targetVolume = _volume.CreateVolume(createVolumeRequest, superUserOption);

        var createGdsRequest = new CreateGdsRequest
        {
            VolumeId = "UploadStreamGdsVolumeId_02",
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

        await Assert.ThrowsAsync<RpcException>(async () =>
        {
            while ((readBytes = fs.Read(fileChunk, 0, chunkSize)) != 0)
            {
                if (sentBytes > fileSize / 2)
                {
                    break;
                }

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
        });

        var uploadFileUri = Path.Combine(targetVolume.Uri, targetGds.GdsId.ToString(), createGdsRequest.Filename);
        Assert.False(File.Exists(uploadFileUri));
    }
}