using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;
using Mirero.DAQ.Infrastructure.Database.Update;
using GroupEntity = Mirero.DAQ.Domain.Account.Entities.Group;
using GroupDto = Mirero.DAQ.Domain.Account.Protos.V1.Group;

namespace Mirero.DAQ.Application.Account.Handlers.Group.CreateGroup;

public sealed class CreateGroupHandler : IRequestHandler<CreateGroupCommand, GroupDto>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly AccountDbContextPostgreSQL _dbContext;

    public CreateGroupHandler(ILogger<CreateGroupHandler> logger, IMapper mapper,
        IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<GroupDto> Handle(CreateGroupCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var group = _mapper.From(request).AdaptToType<GroupEntity>();

        var selectedGroup = _dbContext.Groups.SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken).Result;

        if (selectedGroup != null)
            throw new Exception($"{request.Id} already exist.");

        await _dbContext.Groups.AddAsync(group, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var result =
            await _dbContext.Groups.SingleOrDefaultAsync(g => g.Id == group.Id, cancellationToken: cancellationToken)
            ?? throw new Exception($"{group.Id} doesn't exist.");

        return _mapper.From(result).AdaptToType<GroupDto>();
    }
}