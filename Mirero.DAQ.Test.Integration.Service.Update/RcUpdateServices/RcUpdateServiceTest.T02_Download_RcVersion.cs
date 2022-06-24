using Mirero.DAQ.Domain.Update.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Update.RcUpdateServices;

public partial class RcUpdateServiceTest
{
    [Fact]
    public async Task T02_Download_RcVersion()
    {
        var superUserOptions = await _GetAuthAsync();

        const int chunkSize = 32 * 1024;
        var downloadMppStreamRequest = new DownloadRcSetupVersionRequest
        {
            RcSetupVersionId = "2022.03.08.1.daq60.srd.default",
            ChunkSize = chunkSize
        };

        var downloadMppStreamResponse = _rcUpdateServiceClient.DownloadRcSetupVersion(downloadMppStreamRequest, superUserOptions);
        var responseStream = downloadMppStreamResponse.ResponseStream;
        await responseStream.MoveNext(CancellationToken.None);
        var fileLength = responseStream.Current.TotalSize;
        var currentLength = responseStream.Current.ChunkSize;
        var saveUri = Path.Combine("D:\\dev_mirero\\workspace\\DAQ60\\update_test\\download\\rc", 
            "RecipeCreator_" + downloadMppStreamRequest.RcSetupVersionId + "." +"exe");
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
