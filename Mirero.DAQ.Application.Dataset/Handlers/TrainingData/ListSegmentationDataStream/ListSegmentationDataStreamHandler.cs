using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.TrainingData.ListSegmentationDataStream;

public class ListSegmentationDataStreamHandler : DatasetHandlerBase, IRequestHandler<ListSegmentationDataStreamCommand, ListSegmentationDataStreamResponse>
{
    public ListSegmentationDataStreamHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListSegmentationDataStreamHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<ListSegmentationDataStreamResponse> Handle(ListSegmentationDataStreamCommand command,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}