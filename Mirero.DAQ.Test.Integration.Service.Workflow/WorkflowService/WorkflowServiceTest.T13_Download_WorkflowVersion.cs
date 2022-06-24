using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T13_Download_WorkflowVersion()
    {
        _fixture.FileDataGenerator.GenerateTestFileData();
        
        const int chunkSize = 1000;
        var downloadRequest = new DownloadWorkflowVersionRequest()
        {
            WorkflowVersionId = 1,
            ChunkSize = chunkSize,
        };

        var downloadStreamResponse = _workflowServiceClient.DownloadWorkflowVersion(downloadRequest);
        var responseStream = downloadStreamResponse.ResponseStream;
        await responseStream.MoveNext(CancellationToken.None);
        var fileSize = responseStream.Current.TotalSize;
        var currentLength = 0;

        var downloadFilaPath = @"D:\DownloadTest\downTest.py";
        if (File.Exists(downloadFilaPath))
        {
            //기존에있는건 먼저 지움
            File.Delete(downloadFilaPath);
        }
        else if (!Directory.Exists(Path.GetDirectoryName(downloadFilaPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(downloadFilaPath)!);
        }
        
        await using var fs = new FileStream(downloadFilaPath, FileMode.Create);
        do
        {
            var chunkLength = (int) responseStream.Current.ChunkSize;
            await fs.WriteAsync(responseStream.Current.Buffer.ToByteArray(), 0, chunkLength);
          
            currentLength += chunkLength;
            _output.WriteLine($"Download : {currentLength} / {fileSize} Bytes ... {((float)currentLength / fileSize) * 100}%");
        } while (await responseStream.MoveNext(CancellationToken.None));

        Assert.Equal(((float)currentLength / fileSize) * 100, 100);
    }
}