using System.Text;
using Google.Protobuf;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Redis;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.JobService;

public partial class JobServiceTest
{
    [Fact]
    public async Task T02_Create_BatchJob_Start_Job()
    {
        #region signIn
        await _fixture.SignInAsync("administrator", "mirero2816!");
        #endregion

        var jobTotalCount = 10;
        
        var createRequest = new CreateBatchJobRequest()
        {
            Id = "Test_Lot_2",
            Title = "Test Batch Job",
            TotalCount = jobTotalCount,
            WorkflowType = WorkflowType.RecipeWorkflow
        };
        
        var createdBatchJob = await _jobServiceClient.CreateBatchJobAsync(createRequest, _fixture.OptionsWithAuthHeader());
        Assert.NotNull(createdBatchJob);
        Assert.Equal(createRequest.Id, createdBatchJob.Id);
        Assert.Equal(createRequest.Title, createdBatchJob.Title);
        Assert.Equal(createRequest.TotalCount, createdBatchJob.TotalCount);
        Assert.Equal(createRequest.WorkflowType, createdBatchJob.WorkflowType);
        Assert.Equal(_fixture.CurrentTestUser.Name, createdBatchJob.RegisterUser);
        Assert.NotNull(createdBatchJob.RegisterDate);
        Assert.Equal(createdBatchJob.Status, JobStatus.InProgress);
        Assert.Null(createdBatchJob.StartDate);
        Assert.Null(createdBatchJob.EndDate);
        
        var database = _fixture.RedisConnection.CreateDatabase();
        var redisValue = database.StringGet(NameHandler.GetBatchJobStringName(createdBatchJob.Id));
        Assert.Equal((int)redisValue, createdBatchJob.TotalCount);
       
        using var call = _jobServiceClient.StartJob();
        var startJobRequestList = new List<StartJobRequest>();
        for (int i = 0; i < jobTotalCount; i++)
        {
            startJobRequestList.Add(new StartJobRequest()
            {
                BatchJobId = createdBatchJob.Id,
                JobId = $"test_{i}",
                WorkflowVersionId = 1,
                Parameter = ByteString.CopyFrom($"This is [{i}] Params", Encoding.Default),
                TimeOut = 10000,
                JobType = JobType.Default
            });
            
            await call.RequestStream.WriteAsync(startJobRequestList[i]);
        }
        await call.RequestStream.CompleteAsync();
        await call.ResponseAsync;
        
        var message = database.StreamReadGroup(NameHandler.GetStartJobStreamName(JobType.Default),
            NameHandler.GetStartJobStreamName(JobType.Default), "consumer1", count:100);

        Assert.Equal(10, message.Count());

        for (int i = 0; i < jobTotalCount; i++)
        {
            var hashEntries = database.HashGetAll(NameHandler.GetJobHashName(message[i].Values[0].Value, message[i].Values[1].Value));
            Assert.NotEmpty(hashEntries);
            await database.StreamAcknowledgeAsync(NameHandler.GetStartJobStreamName(JobType.Default)
                , NameHandler.GetStartJobStreamName(JobType.Default)
                , message[i].Id);
            
            foreach (var hashEntry in hashEntries)
            {
                if (hashEntry.Name == "status")
                {
                    Assert.Equal(JobStatus.Ready, hashEntry.Value);
                }
                else if (hashEntry.Name == "register_date")
                {
                    Assert.NotEqual(string.Empty, (string)hashEntry.Value);
                }
                else if (hashEntry.Name == "workflow_version_id")
                {
                    Assert.Equal(startJobRequestList[i].WorkflowVersionId, hashEntry.Value);
                }
                else if (hashEntry.Name == "parameter")
                {
                    Assert.Equal(startJobRequestList[i].Parameter.ToByteArray(), (byte[])hashEntry.Value);
                }
                else if (hashEntry.Name == "type")
                {
                    Assert.Equal(startJobRequestList[i].JobType, hashEntry.Value);
                }
            }

            var workflowVersion = (await _fixture.WorkflowService.ListWorkflowVersionsAsync(new ListWorkflowVersionsRequest()
            {
                QueryParameter = new Domain.Common.Protos.QueryParameter()
                {
                    PageIndex = 0,
                    PageSize = 10,
                    Where = $"Id=1"
                },
                WorkflowId = 1
            })).WorkflowVersions.FirstOrDefault();
            var fileUri = Path.Combine(_fixture.VolumeBaseUri, "volume1" , workflowVersion.WorkflowId.ToString(), workflowVersion.Id.ToString(), workflowVersion.FileName);
            
            var nameEntries = message[i].Values;
            Assert.NotEmpty(nameEntries);
            foreach (var nameEntry in nameEntries)
            {
                if (nameEntry.Name == "batch_job_id")
                {
                    Assert.Equal(startJobRequestList[i].BatchJobId, nameEntry.Value);
                }
                else if (nameEntry.Name == "job_id")
                {
                    Assert.Equal(startJobRequestList[i].JobId, nameEntry.Value);
                }
                else if (nameEntry.Name == "workflow_type")
                {
                    Assert.Equal(createdBatchJob.WorkflowType, nameEntry.Value);
                }
                else if (nameEntry.Name == "time_out")
                {
                    Assert.Equal(startJobRequestList[i].TimeOut, nameEntry.Value);
                }
                else if (nameEntry.Name == "uri")
                {
                    Assert.Equal(fileUri, nameEntry.Value);
                }
            }
        }
    }
}