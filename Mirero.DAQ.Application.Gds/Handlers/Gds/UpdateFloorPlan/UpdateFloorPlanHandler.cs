using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Gds;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.UpdateFloorPlan;

public class UpdateFloorPlanHandler : IRequestHandler<UpdateFloorPlanCommand, FloorPlan>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly GdsDbContext _dbContext;

    public UpdateFloorPlanHandler(ILogger<UpdateFloorPlanHandler> logger, IMapper mapper, IDbContextFactory<GdsDbContextPostgreSQL> dbContextFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<FloorPlan> Handle(UpdateFloorPlanCommand command, CancellationToken cancellationToken)
    {
        // var request = command.Request;
        // var floorPlanGds = await _dbContext.FloorPlansGds
        //     .SingleOrDefaultAsync(fg => fg.FloorPlan.Title == request.Title, cancellationToken) ?? throw new InvalidOperationException();
        //
        // var gdses = _dbContext.FloorPlansGds.Where(fp => fp.FloorPlanId == floorPlanGds.FloorPlanId);
        // _dbContext.RemoveRange(gdses);
        // await _dbContext.SaveChangesAsync(cancellationToken);
        //
        // foreach (var gds in request.Gdses)
        // {
        //     var item = new FloorPlanGds
        //     {
        //         GdsId = gds.GdsId,
        //         FloorPlanId = floorPlanGds.FloorPlanId,
        //         OffsetX = gds.OffsetX,
        //         OffsetY = gds.OffsetY
        //     }; // Mapper로 수정
        //     
        //     await _dbContext.FloorPlansGds.AddAsync(item, cancellationToken);
        // }
        //
        // await _dbContext.SaveChangesAsync(cancellationToken);
        // var floorPlan = _mapper.From(floorPlanGds).AdaptToType<FloorPlan>();
        // return floorPlan;

        throw new NotImplementedException();
    }
}