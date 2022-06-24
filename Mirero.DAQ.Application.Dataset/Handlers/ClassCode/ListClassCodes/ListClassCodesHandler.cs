using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.ListClassCodes;

public class ListClassCodesHandler : ClassCodeHandler, IRequestHandler<ListClassCodesCommand, ListClassCodesResponse>
{
    public ListClassCodesHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListClassCodesHandler> logger,
        IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<ListClassCodesResponse> Handle(ListClassCodesCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var selectQueryable = request.WithThumbnail
            ? _dbContext.ClassCodes.Include(c => c.ClassCodeReferenceImages.OrderBy(c => c.Id).Take(1))
            : _dbContext.ClassCodes.Include(c => c.ClassCodeReferenceImages);

        var (count, items) = await selectQueryable
            .Include(c => c.ClassCodeSet.Volume)
            .AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter, cancellationToken);

        if (!items.Any())
        {
            return _mapper.From((request, items.Select(_ToClassCode), count))
                .AdaptToType<ListClassCodesResponse>();
        }

        var classCodeSetIds =
            items.Select(c => c.ClassCodeSetId).Distinct().Select(c => GenerateLockId<ClassCodeSet>(c));

        await using var @lock =
            await _lockProvider.AcquireReadLockAsync(classCodeSetIds, request.LockTimeoutSec,
                cancellationToken: cancellationToken);
        
        var classCodes = request.WithBuffer
            ? await Task.WhenAll(items.Select(c => _ToClassCodeWithImageAsync(c, cancellationToken)))
            : items.Select(_ToClassCode);

        return _mapper.From((request, classCodes, count))
            .AdaptToType<ListClassCodesResponse>();
    }
}