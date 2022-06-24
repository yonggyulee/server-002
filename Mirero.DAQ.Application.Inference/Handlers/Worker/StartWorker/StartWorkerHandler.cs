using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Inference.Handlers.Worker.StartWorker;

public class StartWorkerHandler : IRequestHandler<StartWorkerCommand, Empty>
{
    private readonly ILogger<StartWorkerHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public StartWorkerHandler(ILogger<StartWorkerHandler> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (dbContextFactory == null)
        {
            throw new ArgumentNullException(nameof(dbContextFactory));
        }
        _dbContext = dbContextFactory.CreateDbContext();
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Empty> Handle(StartWorkerCommand command, CancellationToken cancellationToken)
    {
        // TODO : Docker Container Start하는 동작 구현
        throw new NotImplementedException();
    }
}