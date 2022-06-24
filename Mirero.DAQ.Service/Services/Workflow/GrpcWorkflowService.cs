using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.CreateWorkflow;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.CreateWorkflowVersion;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.DeleteWorkflow;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.DeleteWorkflowVersion;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.DownloadWorkflowVersion;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.ListWorkflows;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.ListWorkflowVersions;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.ResetDefaultWorkflowVersion;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.SetDefaultWorkflowVersion;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.UpdateWorkflow;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.UpdateWorkflowVersion;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.UploadWorkflowVersion;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Service.Services.Workflow
{
    public class GrpcWorkflowService : WorkflowService.WorkflowServiceBase
    {
        private readonly ILogger<GrpcWorkflowService> _logger;
        private readonly IMediator _mediator;

        public GrpcWorkflowService(ILogger<GrpcWorkflowService> logger
            , IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        #region Workflow
        public override async Task<ListWorkflowsResponse> ListWorkflows(ListWorkflowsRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new ListWorkflowsCommand(request), context.CancellationToken);
        }

        public override async Task<Domain.Workflow.Protos.V1.Workflow> CreateWorkflow(CreateWorkflowRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new CreateWorkflowCommand(request), context.CancellationToken);
        }

        public override async Task<Domain.Workflow.Protos.V1.Workflow> UpdateWorkflow(UpdateWorkflowRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new UpdateWorkflowCommand(request), context.CancellationToken);
        }

        public override async Task<Empty> DeleteWorkflow(DeleteWorkflowRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new DeleteWorkflowCommand(request), context.CancellationToken);
        }

        public override async Task<Empty> SetDefaultWorkflowVersion(SetDefaultWorkflowVersionRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new SetDefaultWorkflowVersionCommand(request), context.CancellationToken);
        }

        public override async Task<Empty> ResetDefaultWorkflowVersion(ResetDefaultWorkflowVersionRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new ResetDefaultWorkflowVersionCommand(request), context.CancellationToken);
        }
        #endregion

        #region WorkflowVersion
        public override async Task<ListWorkflowVersionsResponse> ListWorkflowVersions(ListWorkflowVersionsRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new ListWorkflowVersionsCommand(request), context.CancellationToken);
        }

        public override async Task<WorkflowVersion> CreateWorkflowVersion(CreateWorkflowVersionRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new CreateWorkflowVersionCommand(request), context.CancellationToken);
        }

        public override async Task<WorkflowVersion> UpdateWorkflowVersion(UpdateWorkflowVersionRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new UpdateWorkflowVersionCommand(request), context.CancellationToken);
        }

        public override async Task<Empty> DeleteWorkflowVersion(DeleteWorkflowVersionRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new DeleteWorkflowVersionCommand(request), context.CancellationToken);
        }

        public override async Task DownloadWorkflowVersion(DownloadWorkflowVersionRequest request, IServerStreamWriter<StreamBuffer> response, ServerCallContext context)
        {
            await _mediator.Send(new DownloadWorkflowVersionCommand(request, response), context.CancellationToken);
        }

        public override async Task<Empty> UploadWorkflowVersion(IAsyncStreamReader<IdentifiedStreamBuffer> request, ServerCallContext context)
        {
            return await _mediator.Send(new UploadWorkflowVersionCommand(request), context.CancellationToken);
        }
        #endregion
    }
}
