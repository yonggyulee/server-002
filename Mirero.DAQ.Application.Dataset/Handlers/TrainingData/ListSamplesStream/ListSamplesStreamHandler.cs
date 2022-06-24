using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.TrainingData.ListSamplesStream;

public class ListSamplesStreamHandler : DatasetHandlerBase, IRequestHandler<ListSamplesStreamCommand, ListSamplesStreamResponse>
{
    public ListSamplesStreamHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListSamplesStreamHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<ListSamplesStreamResponse> Handle(ListSamplesStreamCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}