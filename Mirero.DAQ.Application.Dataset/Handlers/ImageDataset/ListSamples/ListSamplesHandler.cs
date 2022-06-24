using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

using ImageDatasetDto = Mirero.DAQ.Domain.Dataset.Protos.V1.ImageDataset;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.ListSamples;

public class ListSamplesHandler : SampleHandler, IRequestHandler<ListSamplesCommand, ListSamplesResponse>
{
    public ListSamplesHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListSamplesHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<ListSamplesResponse> Handle(ListSamplesCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var (count, items) = await _dbContext.Samples
            .Include(s => s.ImageDataset.Volume)
            .Include(s => s.Images)
            .AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter, cancellationToken);

        var datasetIds = items.Select(s => s.DatasetId).Distinct().Select(s => GenerateLockId<ImageDatasetDto>(s));

        await using var @lock = await _lockProvider.AcquireReadLockAsync(datasetIds, request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        var samples = request.WithBuffer
            ? await Task.WhenAll(items.Select(s => _ToSampleWithImageAsync(s, cancellationToken)))
            : items.Select(_ToSample);

        return _mapper.From((request, samples, count)).AdaptToType<ListSamplesResponse>();
    }
}