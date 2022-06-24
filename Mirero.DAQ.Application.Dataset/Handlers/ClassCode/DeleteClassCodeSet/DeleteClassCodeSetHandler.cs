using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.DeleteClassCodeSet;

public class DeleteClassCodeSetHandler : DatasetHandlerBase, IRequestHandler<DeleteClassCodeSetCommand, ClassCodeSet>
{
    public DeleteClassCodeSetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DeleteClassCodeSetHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<ClassCodeSet> Handle(DeleteClassCodeSetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var selectedClassCodeSet = await _dbContext.ClassCodeSets.Include(c => c.Volume)
                                       .SingleOrDefaultAsync(
                                           c => c.Id == request.ClassCodeSetId,
                                           cancellationToken: cancellationToken)
                                   ?? throw new InvalidOperationException($"ClassCodeSet Id={request.ClassCodeSetId}");

        await using var @lock = await _lockProvider.AcquireWriteLockAsync(
            GenerateLockId<ClassCodeSet>(selectedClassCodeSet.Id),
            request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        var gtDatasetUris = (await _dbContext.GtDatasets.Include(d => d.Volume)
            .Select(d => new { d.ClassCodeSetId, Uri = Path.Combine(d.Volume.Uri, d.DirectoryName) })
            .Where(d => d.ClassCodeSetId == selectedClassCodeSet.Id).ToListAsync(cancellationToken)).Select(d => d.Uri);

        _dbContext.ClassCodeSets.Remove(selectedClassCodeSet);
        await _dbContext.SaveChangesAsync(cancellationToken);

        foreach (var uri in gtDatasetUris)
        {
            _fileStorage.DeleteFolder(uri);
        }
        _fileStorage.DeleteFolder(Path.Combine(selectedClassCodeSet.Volume.Uri, selectedClassCodeSet.DirectoryName));

        return _mapper.From(selectedClassCodeSet).AdaptToType<ClassCodeSet>();
    }
}