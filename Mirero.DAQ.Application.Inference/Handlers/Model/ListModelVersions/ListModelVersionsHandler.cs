using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Inference.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.ListModelVersions;

public class ListModelVersionsHandler : IRequestHandler<ListModelVersionsCommand, ListModelVersionsResponse>
{
    private readonly ILogger<ListModelVersionsHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IMapper _mapper;

    public ListModelVersionsHandler(ILogger<ListModelVersionsHandler> logger,
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

    public async Task<ListModelVersionsResponse> Handle(ListModelVersionsCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var (count, items) = await _dbContext.ModelVersions
            .AsNoTracking()
            .Include(m => m.Model.Volume)
            .AsPagedResultAsync(request.QueryParameter, m => _mapper.From(m).AdaptToType<ModelVersion>(),
                cancellationToken);

        return _mapper.From((request, items, count)).AdaptToType<ListModelVersionsResponse>();
    }
}