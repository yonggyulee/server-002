using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Workflow;

namespace Mirero.DAQ.Application.Workflow.Handlers.Volume.ListVolumes;

public class ListVolumesHandler : IRequestHandler<ListVolumesCommand, ListVolumesResponse>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public ListVolumesHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IMapper mapper) 
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ListVolumesResponse> Handle(ListVolumesCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var (count, items) = await _dbContext.Volumes
            .AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter, v => _mapper.From(v).AdaptToType<Domain.Workflow.Protos.V1.Volume>(),
                cancellationToken);

        return _mapper.From((request, items, count)).AdaptToType<ListVolumesResponse>();
    }
}
