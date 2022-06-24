using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.Volume.ListVolumes;

public class ListVolumesHandler : DatasetHandlerBase, IRequestHandler<ListVolumesCommand, ListVolumesResponse>
{
    public ListVolumesHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListVolumesHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<ListVolumesResponse> Handle(ListVolumesCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var (count, items) = await _dbContext.Volumes
            .AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter, v => _mapper.From(v).AdaptToType<Domain.Dataset.Protos.V1.Volume>(),
                cancellationToken);

        return _mapper.From((request, items, count)).AdaptToType<ListVolumesResponse>();
    }
}