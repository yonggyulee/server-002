using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Application.Workflow.StreamCreator;
using Mirero.DAQ.Domain.Common.Constants;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Grpc.Extensions;
using Mirero.DAQ.Infrastructure.Grpc.StreamCreator;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.UploadWorkflowVersion;

public class UploadWorkflowVersionHandler: IRequestHandler<UploadWorkflowVersionCommand, Empty>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly WorkflowVersionUploadStreamCreator _streamCreator;
    
    public UploadWorkflowVersionHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory,
        WorkflowVersionUploadStreamCreator streamCreator)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _streamCreator = streamCreator ?? throw new ArgumentNullException(nameof(streamCreator));
    }
    
    public async Task<Empty> Handle(UploadWorkflowVersionCommand command, CancellationToken cancellationToken)
    {
        var requestStream = command.RequestStream;
        var isSuccess = true;
        try
        {
            await requestStream.ReadGrpcServerStreamReaderAsync(_streamCreator, cancellationToken: cancellationToken);
        }
        catch
        {
            isSuccess = false;
        }
        finally
        {
            await UpdateWorkflowVersionDataStatus(_streamCreator.GetId(), isSuccess, cancellationToken);
        }
        return new Empty();
    }

    private async Task UpdateWorkflowVersionDataStatus(long? id, bool isSuccess, CancellationToken cancellationToken)
    {
        var workflowVersion = await _dbContext.WorkflowVersions.AsTracking().SingleOrDefaultAsync(
                                  x => x.Id == id,
                                  cancellationToken: cancellationToken)
                              ?? throw new NotImplementedException();
        
        workflowVersion.DataStatus = isSuccess ? DataStatus.Success : DataStatus.Fail;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}