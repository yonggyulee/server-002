using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;
using SystemEntity = Mirero.DAQ.Domain.Account.Entities.System;


namespace Mirero.DAQ.Application.Account.Handlers.Group.CreateSystem;

public class CreateSystemHandler : IRequestHandler<CreateSystemCommand, Domain.Account.Protos.V1.System>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly AccountDbContextPostgreSQL _dbContext;


    public CreateSystemHandler(ILogger<CreateSystemHandler> logger, IMapper mapper,
        IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<Domain.Account.Protos.V1.System> Handle(CreateSystemCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var system = _mapper.From(request).AdaptToType<SystemEntity>();

        var selectedSystem = _dbContext.Systems.SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken).Result;
        if (selectedSystem != null)
            throw new Exception($"{request.Id} already exist.");

        await _dbContext.Systems.AddAsync(system, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var result =
            await _dbContext.Systems.SingleOrDefaultAsync(s => s.Id == system.Id, cancellationToken: cancellationToken)
            ?? throw new Exception($"{system.Id} doesn't exist.");

        return _mapper.From(result).AdaptToType<Domain.Account.Protos.V1.System>();
    }
}