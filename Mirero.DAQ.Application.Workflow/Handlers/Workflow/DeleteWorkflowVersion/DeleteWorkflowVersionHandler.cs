using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.DeleteWorkflow;
using Mirero.DAQ.Application.Workflow.Handlers.Workflow.ResetDefaultWorkflowVersion;
using Mirero.DAQ.Application.Workflow.UriGenerator;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.DeleteWorkflowVersion;

public class DeleteWorkflowVersionHandler : IRequestHandler<DeleteWorkflowVersionCommand, Empty>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly IUriGenerator _uriGenerator;
    private readonly IMediator _mediator;

    public DeleteWorkflowVersionHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IFileStorage fileStorage
            , IUriGenerator uriGenerator
            , IMediator mediator)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _uriGenerator = uriGenerator ?? throw new ArgumentNullException(nameof(uriGenerator));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<Empty> Handle(DeleteWorkflowVersionCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var deleteTarget = await _dbContext.WorkflowVersions
                               .Include(x => x.Workflow)
                               .ThenInclude(x => x.Volume)
                               .FirstOrDefaultAsync(x => x.Id == request.WorkflowVersionId, cancellationToken) 
                           ?? throw new NotImplementedException();

        var workflowDefaultVersion = await _dbContext.DefaultWorkflowVersions.FirstOrDefaultAsync(x => x.WorkflowId == deleteTarget.WorkflowId, cancellationToken: cancellationToken);
        if (workflowDefaultVersion?.WorkflowVersionId == deleteTarget.Id)
        {
            await _mediator.Send(
                new ResetDefaultWorkflowVersionCommand(
                    new Domain.Workflow.Protos.V1.ResetDefaultWorkflowVersionRequest() { WorkflowId = deleteTarget.WorkflowId }), cancellationToken);
        }

        //빈껍데기인 경우 모두 삭제
        if (_dbContext.WorkflowVersions.Count(x => x.WorkflowId == deleteTarget.WorkflowId) == 1)
        {
            await _mediator.Send(
                new DeleteWorkflowCommand(
                    new Domain.Workflow.Protos.V1.DeleteWorkflowRequest() { WorkflowId = deleteTarget.WorkflowId }), cancellationToken);
        }
        else
        {
            var workflowVersionDir = _uriGenerator
                .GetWorkflowVersionUri(deleteTarget.Workflow.Volume.Uri, deleteTarget.WorkflowId, deleteTarget.Id);
            if (Directory.Exists(workflowVersionDir))
            {
                _fileStorage.DeleteFolder(workflowVersionDir);
            }

            _dbContext.WorkflowVersions.Remove(deleteTarget);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return new Empty();
    }
}
