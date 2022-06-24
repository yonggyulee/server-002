using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Gds;
using GdsDto = Mirero.DAQ.Domain.Gds.Protos.V1.Gds;
using GdsEntity = Mirero.DAQ.Domain.Gds.Entities.Gds;
using GdsStatus = Mirero.DAQ.Domain.Gds.Constants.GdsStatus;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.CreateGds;

public class CreateGdsHandler : IRequestHandler<CreateGdsCommand, CreateGdsResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly GdsDbContext _dbContext;
    private readonly RequesterContext _requesterContext;

    public CreateGdsHandler(ILogger<CreateGdsHandler> logger, IMapper mapper,
        IDbContextFactory<GdsDbContextPostgreSQL> dbContextFactory
        , RequesterContext requestContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _requesterContext = requestContext;
    }

    public async Task<CreateGdsResponse> Handle(CreateGdsCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var gds = _mapper.From(request).AdaptToType<GdsEntity>();

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");

        gds.RegisterUser = _requesterContext.UserName;
        gds.RegisterDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        gds.Status = GdsStatus.NoFile;
        await _dbContext.Gds.AddAsync(gds, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new CreateGdsResponse
        {
            GdsId = gds.Id
        };

        return response;
    }
}