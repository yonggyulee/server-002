using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using WorkerEntity = Mirero.DAQ.Domain.Inference.Entities.Worker;

namespace Mirero.DAQ.Application.Inference.Handlers.Worker.CreateWorker;

public class CreateWorkerHandler : IRequestHandler<CreateWorkerCommand, Empty>
{
    private readonly ILogger<CreateWorkerHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public CreateWorkerHandler(ILogger<CreateWorkerHandler> logger,
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

    public async Task<Empty> Handle(CreateWorkerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var worker = _mapper.From(request).AdaptToType<WorkerEntity>();

        try
        {
            await _dbContext.Workers.AddAsync(worker, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            throw;
        }

        return new Empty();
    }
}