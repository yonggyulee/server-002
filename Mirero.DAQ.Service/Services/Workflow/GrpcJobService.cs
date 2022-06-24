using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Workflow.Handlers.Job.CancelBatchJob;
using Mirero.DAQ.Application.Workflow.Handlers.Job.CancelJob;
using Mirero.DAQ.Application.Workflow.Handlers.Job.CreateBatchJob;
using Mirero.DAQ.Application.Workflow.Handlers.Job.DeleteBatchJob;
using Mirero.DAQ.Application.Workflow.Handlers.Job.ListBatchJobs;
using Mirero.DAQ.Application.Workflow.Handlers.Job.ListJobs;
using Mirero.DAQ.Application.Workflow.Handlers.Job.MonitoringBatchJobs;
using Mirero.DAQ.Application.Workflow.Handlers.Job.StartJob;
using Mirero.DAQ.Application.Workflow.Handlers.Job.WaitBatchJob;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Service.Services.Workflow
{
    public class GrpcJobService : JobService.JobServiceBase
    {
        private readonly ILogger<GrpcJobService> _logger;
        private readonly IMediator _mediator;

        public GrpcJobService(ILogger<GrpcJobService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        #region BatchJob
        public override async Task<ListBatchJobsResponse> ListBatchJobs(ListBatchJobsRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new ListBatchJobsCommand(request), context.CancellationToken);
        }

        public override async Task<BatchJob> CreateBatchJob(CreateBatchJobRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new CreateBatchJobCommand(request), context.CancellationToken);
        }

        public override async Task<Empty> StartJob(IAsyncStreamReader<StartJobRequest> requestStream, ServerCallContext context)
        {
            return await _mediator.Send(new StartJobCommand(requestStream), context.CancellationToken);
        }

        public override async Task WaitBatchJob(WaitBatchJobRequest request, IServerStreamWriter<WaitBatchJobResponse> response, ServerCallContext context)
        {
            await _mediator.Send(new WaitBatchJobCommand(request, response), context.CancellationToken);
        }

        public override async Task<Empty> CancelBatchJob(CancelBatchJobRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new CancelBatchJobCommand(request), context.CancellationToken);
        }

        public override async Task<Empty> DeleteBatchJob(DeleteBatchJobRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new DeleteBatchJobCommand(request), context.CancellationToken);
        }

        public override async Task<MonitoringBatchJobsResponse> MonitoringBatchJobs(IAsyncStreamReader<MonitoringBatchJobsRequest> request, ServerCallContext context)
        {
            return await _mediator.Send(new MonitoringBatchJobsCommand(request), context.CancellationToken);
        }
        #endregion

        #region Job
        public override async Task<ListJobsResponse> ListJobs(ListJobsRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new ListJobsCommand(request), context.CancellationToken);
        }

        public override async Task<Empty> CancelJob(CancelJobRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new CancelJobCommand(request), context.CancellationToken);
        }
        #endregion
    }
}
