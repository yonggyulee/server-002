using Google.Protobuf;
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

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.ListImageDatasets;

public class ListImageDatasetsHandler : DatasetHandlerBase, IRequestHandler<ListImageDatasetsCommand, ListImageDatasetsResponse>
{
    public ListImageDatasetsHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListImageDatasetsHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<ListImageDatasetsResponse> Handle(ListImageDatasetsCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var (count, items) = await _dbContext.ImageDatasets.AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter,
                d => _mapper.From(d).AdaptToType<ImageDatasetDto>(),
                cancellationToken);

        if (request.WithThumbnail)
            items = await Task.WhenAll(items.Select(async d =>
            {
                await using var ctx = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
                await using var lockProvider = _lockProviderFactory.CreateLockProvider(ctx);
                await using (await lockProvider.AcquireReadLockAsync(GenerateLockId<ImageDatasetDto>(d.Id),
                                 request.LockTimeoutSec,
                                 cancellationToken: cancellationToken))
                {
                    var uri = (await ctx.Volumes.FindAsync(
                                   new object?[] { d.VolumeId },
                                   cancellationToken: cancellationToken)
                               ?? throw new NotImplementedException()).Uri;
                    var image = await ctx.Images.FirstOrDefaultAsync(
                        i => i.DatasetId == d.Id, cancellationToken: cancellationToken);
                    if (image == null)
                    {
                        return d;
                    }

                    d.ThumbnailBuffer = ByteString.CopyFrom(await
                        _fileStorage.GetFileBufferAsync(
                            Path.Combine(uri, d.DirectoryName),
                            image.Filename, cancellationToken));
                    return d;
                }
            }));

        return _mapper.From((request, Items: items, Count: count)).AdaptToType<ListImageDatasetsResponse>();
    }
}