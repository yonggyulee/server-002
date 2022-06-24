using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Workflow.Handlers.Volume.DeleteVolume;

public class DeleteVolumeHandler : IRequestHandler<DeleteVolumeCommand, Empty>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public DeleteVolumeHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IFileStorage fileStorage) 
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
    }

    public async Task<Empty> Handle(DeleteVolumeCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var selectedVolume = await _dbContext.Volumes.FindAsync(
                                  new object?[] { request.VolumeId },
                                  cancellationToken: cancellationToken)
                              ?? throw new NotImplementedException();

        _dbContext.Volumes.Remove(selectedVolume);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _fileStorage.DeleteFolder(selectedVolume.Uri);

        return new Empty();
    }
}
