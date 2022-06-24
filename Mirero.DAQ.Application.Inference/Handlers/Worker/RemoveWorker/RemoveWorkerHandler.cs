using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Inference.Handlers.Worker.RemoveWorker;

public class RemoveWorkerHandler : IRequestHandler<RemoveWorkerCommand, Empty>
{
    private readonly ILogger<RemoveWorkerHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public RemoveWorkerHandler(ILogger<RemoveWorkerHandler> logger,
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

    public async Task<Empty> Handle(RemoveWorkerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var selectedWorker = await _dbContext.Workers.FindAsync(
                                 new object?[] { request.WorkerId },
                                 cancellationToken: cancellationToken)
                             ?? throw new NotImplementedException();

        _dbContext.Workers.Remove(selectedWorker);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}