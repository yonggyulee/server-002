using System.Text;
using Google.Protobuf;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Redis;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.JobService;

public partial class JobServiceTest
{
    [Fact]
    public async Task T05_Get_Jobs_From_Redis()
    {
        #region signIn
        await _fixture.SignInAsync("administrator", "mirero2816!");
        #endregion

        var jobTotalCount = 10;
        var createRequest = new CreateBatchJobRequest()
        {
            Id = "Test_Lot_5",
            Title = "Test Batch Job",
            TotalCount = jobTotalCount,
            WorkflowType = WorkflowType.RecipeWorkflow
        };
        
        var createdBatchJob = await _jobServiceClient.CreateBatchJobAsync(createRequest, _fixture.OptionsWithAuthHeader());

        using var call = _jobServiceClient.StartJob();
        var startJobRequestList = new List<StartJobRequest>();
        for (int i = 0; i < jobTotalCount; i++)
        {
            startJobRequestList.Add(new StartJobRequest()
            {
                BatchJobId = createRequest.Id,
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

        //다른 작업이랑 꼬이지 않도록 message 정리
        var message = _fixture.RedisConnection.CreateDatabase().StreamReadGroup(NameHandler.GetStartJobStreamName(JobType.Default),
            NameHandler.GetStartJobStreamName(JobType.Default), "consumer1", count:100);
        for (int i = 0; i < jobTotalCount; i++)
        {
            await _fixture.RedisConnection.CreateDatabase().StreamAcknowledgeAsync(NameHandler.GetStartJobStreamName(JobType.Default)
                , NameHandler.GetStartJobStreamName(JobType.Default)
                , message[i].Id);
        }

        var listRequest = new ListJobsRequest()
        {
            QueryParameter = new QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10
            },
            BatchJobId = createdBatchJob.Id
        };

        var jobs = (await _jobServiceClient.ListJobsAsync(listRequest)).Jobs;
        Assert.Equal(createdBatchJob.TotalCount, jobs.Count());
        for (int i = 0; i < jobTotalCount; i++)
        {
            var targetJob = jobs.FirstOrDefault(x => x.Id == startJobRequestList[i].JobId);
            Assert.NotNull(targetJob);
            Assert.Equal(startJobRequestList[i].BatchJobId, targetJob.BatchJobId);
            Assert.Equal(startJobRequestList[i].JobId, targetJob.Id);
            Assert.Equal(startJobRequestList[i].WorkflowVersionId, targetJob.WorkflowVersionId);
            Assert.Equal(startJobRequestList[i].Parameter, targetJob.Parameter);
            Assert.Equal(startJobRequestList[i].JobType, targetJob.Type);
        }
    }
}