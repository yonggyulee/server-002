using System.Linq.Dynamic.Core;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ClassificationGtDataset = Mirero.DAQ.Domain.Dataset.Entities.ClassificationGtDataset;
using ImageDatasetDto = Mirero.DAQ.Domain.Dataset.Protos.V1.ImageDataset;

namespace Mirero.DAQ.Application.Dataset.Handlers.TrainingData.ListClassificationDataStream;

public class ListClassificationDataStreamHandler : TrainingDataHandler, IRequestHandler<ListClassificationDataStreamCommand>
{
    public ListClassificationDataStreamHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListClassificationDataStreamHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<Unit> Handle(ListClassificationDataStreamCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var responseStream = command.ResponseStream;

        var end = (int)Math.Ceiling((float)request.TotalCount / request.QueryParameter.PageSize);

        await using var @lock =
            await _AcquireMultipleLockByTrainingData<ClassificationGtDataset>(request.QueryParameter.Where,
                request.LockTimeoutSec, cancellationToken);

        var currentCount = 0;
        for (var i = 0; i < end; i++)
        {
            var items = await _dbContext.Images.Include(img => img.Sample.ImageDataset.Volume)
                .AsNoTracking()
                .AsPagedQueryableResult(request.QueryParameter)
                .Join(_dbContext.ClassificationGts, img => img.Id, gt => gt.ImageId,
                    (img, gt) => new
                    {
                        Image = img,
                        ClassificationGt = gt
                    })
                .ToListAsync(cancellationToken);

            //var (_, items) = await _dbContext.ClassificationGts.Include(gt => gt.Image.Sample.ImageDataset.Volume)
            //    .AsNoTracking().AsPagedResultAsync(request.QueryParameter, cancellationToken);

            var images = await Task.WhenAll(items.Select(async it => await _ToImageAsync(it.Image, cancellationToken)));

            var gts = items.Select(it => _mapper.From(it.ClassificationGt).AdaptToType<ClassificationGt>());

            currentCount += items.Count;

            await responseStream.WriteAsync(new ListClassificationDataStreamResponse
    {
                PageResult = new PageResult
                {
                    PageIndex = request.QueryParameter.PageIndex,
                    PageSize = request.QueryParameter.PageSize,
                    Count = currentCount
                },
                Images = { images },
                ClassificationGts = { gts }
            });

            if (request.TotalCount - currentCount < 1) break;

            request.QueryParameter.PageIndex += 1;
            request.QueryParameter.PageSize = (int)Math.Min(request.QueryParameter.PageSize, request.TotalCount - currentCount);
        }
        
        return Unit.Value;
    }
}