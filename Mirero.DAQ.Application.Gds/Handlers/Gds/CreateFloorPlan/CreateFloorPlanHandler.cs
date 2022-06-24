using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Gds;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.CreateFloorPlan;

public class CreateFloorPlanHandler : IRequestHandler<CreateFloorPlanCommand, FloorPlan>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly GdsDbContext _dbContext;
    private readonly RequesterContext _requesterContext;
    
    public CreateFloorPlanHandler(ILogger<CreateFloorPlanHandler> logger, 
        IMapper mapper, IDbContextFactory<GdsDbContextPostgreSQL> dbContextFactory
        ,RequesterContext requestContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _requesterContext = requestContext;
    }

    public async Task<FloorPlan> Handle(CreateFloorPlanCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var floorPlanGds = request
            .Gdses
            .Select(gds => new Domain.Gds.Entities.FloorPlanGds
            {
                GdsId = gds.GdsId, 
                Layers = JsonSerializer.Serialize(gds.Layers.ToList()), 
                OffsetX = gds.OffsetX, 
                OffsetY = gds.OffsetY
            })
            .ToList();

        if (_requesterContext.UserName is null)
        {
            throw new InvalidOperationException("RegisterUser의 설정을 위한 사용자명이 RequestContext에 존재하지 않습니다.");
        }
        
        var floorPlan = new Domain.Gds.Entities.FloorPlan
        {
            Title = request.Title,
            RegisterDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
            UpdateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
            RegisterUser = _requesterContext.UserName,
            Properties = request.Properties,
            Description = request.Description,
            FloorPlanGdses = floorPlanGds
        }; 

        await _dbContext.FloorPlan.AddAsync(floorPlan, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        var floorPlans = _mapper.From(floorPlan).AdaptToType<FloorPlan>();

        return floorPlans;
    }
}
