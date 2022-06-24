using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Update.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Update.MppUpdateServices;

public partial class MppUpdateServiceTest
{
    [Fact]
    public async Task T02_Download_MppVersion()
    {
        var superUserOptions = await _GetAuthAsync();

        const int chunkSize = 32 * 1024;
        var downloadMppStreamRequest = new DownloadMppSetupVersionRequest
        {
            MppSetupVersionId = "2022.05.23.1.daq50.memory.full",
            ChunkSize = chunkSize
        };

        var downloadMppStreamResponse = _mppUpdateServiceClient.DownloadMppSetupVersion(downloadMppStreamRequest, superUserOptions);
        var responseStream = downloadMppStreamResponse.ResponseStream;
        await responseStream.MoveNext(CancellationToken.None);
        var fileLength = responseStream.Current.TotalSize;
        var currentLength = responseStream.Current.ChunkSize;
        var saveUri = Path.Combine("D:\\dev_mirero\\workspace\\DAQ60\\update_test\\download\\mpp", 
            "MPP_" + downloadMppStreamRequest.MppSetupVersionId + "." +"exe");
        if (File.Exists(saveUri))
        {
            _output.WriteLine("============= File exist. =============");
        }
        else
        {
            await using var fs = new FileStream(saveUri, FileMode.Create);
            _output.WriteLine($"Download Start... {saveUri}");
            do
            {
                var chunkLength = (int) responseStream.Current.ChunkSize;
                await fs.WriteAsync(responseStream.Current.Buffer.ToByteArray(), 0, chunkLength);
                currentLength += chunkLength;
                _output.WriteLine($"Download : {currentLength} / {fileLength} Bytes ... {((float)currentLength / fileLength) * 100}%");
            } while (await responseStream.MoveNext(CancellationToken.None));
        }
    }
}