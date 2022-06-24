using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Constants;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Test.Integration.Service.TestEnv;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T15_Create_Upload_WorkflowVersion()
    {
        #region signIn
        var user = await _fixture.SignInAsync("administrator", "mirero2816!");
        #endregion
        
        var creteRequest = new CreateWorkflowVersionRequest()
        {
            WorkflowId = 2,
            Version = "1.0",
            FileName = "uploadTest.py",
        };

        var createdWorkflowVersion = await _workflowServiceClient.CreateWorkflowVersionAsync(creteRequest, _fixture.OptionsWithAuthHeader());
        Assert.NotNull(createdWorkflowVersion);
        Assert.NotEqual(0, createdWorkflowVersion.Id);
        Assert.Equal(creteRequest.WorkflowId, createdWorkflowVersion.WorkflowId);
        Assert.Equal(creteRequest.Version, createdWorkflowVersion.Version);
        Assert.Equal(creteRequest.FileName, createdWorkflowVersion.FileName);
        Assert.Equal(_fixture.CurrentTestUser.Name, createdWorkflowVersion.CreateUser);

        //기존에 있는거 삭제
        var existDir = Path.Combine(_fixture.VolumeBaseUri
            , "volume1" 
            , createdWorkflowVersion.WorkflowId.ToString(),
            createdWorkflowVersion.Id.ToString());
        if (Directory.Exists(existDir))
        {
            Directory.Delete(existDir, true);
        }
        
        var testDataDir = Path.Combine(_fixture.FileDataGenerator.GetCurrentPath() ?? string.Empty, "seed_data");
        var testData = Directory.GetFiles(testDataDir).FirstOrDefault();
        Assert.NotNull(testData);

        var streamReader = _workflowServiceClient.UploadWorkflowVersion();
        await using(var stream = File.OpenRead(testData))
        {
            var chunkSize = 1000L;
            var chunkIndex = 0;
            var sentBytes = 0L;
            var buffer = new byte[chunkSize];
            var totalLength = stream.Length;
        
            var readBytes = 0;
            while ((readBytes = await stream.ReadAsync(buffer.AsMemory(0, (int)chunkSize), CancellationToken.None)) != 0)
            {
                await streamReader.RequestStream.WriteAsync(new IdentifiedStreamBuffer()
                {
                    Id = createdWorkflowVersion.Id,
                    TotalSize = stream.Length,
                    ChunkSize = readBytes,
                    ChunkIndex = chunkIndex,
                    Buffer = ByteString.CopyFrom(buffer, 0, readBytes)
                });
                
                chunkIndex++;
                sentBytes += readBytes;
                _output.WriteLine($"Upload : {sentBytes} / {totalLength} Bytes ... {((float)sentBytes / totalLength) * 100}%");
            }
            
            await streamReader.RequestStream.CompleteAsync();
            await streamReader;
        }

        
        var listRequest = new ListWorkflowVersionsRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Where = $"Id={createdWorkflowVersion.Id}"
            },
            WorkflowId = createdWorkflowVersion.WorkflowId
        };
        var workflowVersions = (await _workflowServiceClient.ListWorkflowVersionsAsync(listRequest)).WorkflowVersions;
        Assert.NotEmpty(workflowVersions);
        
        var target = workflowVersions.FirstOrDefault();
        Assert.NotNull(target);
        Assert.Equal(target.DataStatus, DataStatus.Success);
    }
}