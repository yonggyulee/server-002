using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.GetClassCode;

public class GetClassCodeHandler : ClassCodeHandler, IRequestHandler<GetClassCodeCommand, Domain.Dataset.Protos.V1.ClassCode>
{
    public GetClassCodeHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<GetClassCodeHandler> logger,
        IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<Domain.Dataset.Protos.V1.ClassCode> Handle(GetClassCodeCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        var classCode = await _dbContext.ClassCodes
                            .Include(c => c.ClassCodeSet.Volume)
                            .Include(c => c.ClassCodeReferenceImages)
                            .AsNoTracking()
                            .SingleOrDefaultAsync(c => c.Id == request.ClassCodeId,
                                cancellationToken: cancellationToken)
                        ?? throw new InvalidOperationException($"ClassCode Id={request.ClassCodeId}");

        await using var @lock = await _lockProvider.AcquireReadLockAsync(
            GenerateLockId<ClassCodeSet>(classCode.ClassCodeSetId), request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        return await _ToClassCodeWithImageAsync(classCode, cancellationToken);
    }
}