using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.ListClassificationGts;

public class ListClassificationGtsHandler : GtDatasetHandler, IRequestHandler<ListClassificationGtsCommand, ListClassificationGtsResponse>
{
    public ListClassificationGtsHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListClassificationGtsHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }
    public async Task<ListClassificationGtsResponse> Handle(ListClassificationGtsCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var imageIds = request.ImageIds.ToList();

        // var count = await _dbContext.ClassificationGts.CountAsync(cancellationToken);

        var items = (await _dbContext.ClassificationGts
                .AsNoTracking()
                .Include(c => c.ClassCode)
                .Where(c => imageIds.Contains(c.ImageId))
                .ToListAsync(cancellationToken))
            .Select(c => _mapper.From(c).AdaptToType<ClassificationGt>());

        return _mapper.From(items).AdaptToType<ListClassificationGtsResponse>();
    }
}