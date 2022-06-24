using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.TrainingData.ListObjectDetectionDataStream;

public class ListObjectDetectionDataStreamHandler : DatasetHandlerBase, IRequestHandler<ListObjectDetectionDataStreamCommand, ListObjectDetectionDataStreamResponse>
{
    public ListObjectDetectionDataStreamHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListObjectDetectionDataStreamHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<ListObjectDetectionDataStreamResponse> Handle(ListObjectDetectionDataStreamCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}