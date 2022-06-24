using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Account.Entities;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;
using SystemDto = Mirero.DAQ.Domain.Account.Protos.V1.System;

namespace Mirero.DAQ.Application.Account.Handlers.Group.UpdateGroupSystems
{
    public class UpdateGroupSystemsHandler : IRequestHandler<UpdateGroupSystemsCommand, UpdateGroupSystemsResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public UpdateGroupSystemsHandler(ILogger<UpdateGroupSystemsHandler> logger, IMapper mapper,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<UpdateGroupSystemsResponse> Handle(UpdateGroupSystemsCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var checkGroup = _dbContext.GroupSystems
                                 .Where(gf => gf.GroupId == request.GroupId).ToList()
                             ?? throw new Exception($"{request.GroupId} doesn't exist.");
            _dbContext.GroupSystems.RemoveRange(checkGroup);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var listSystems = request.Systems.ToList();

            var systemDtos = listSystems.Select(s =>
            {
                return _mapper.From(s).AdaptToType<SystemDto>();
            });

            var groupSystem = systemDtos
                .Select(s => { return _mapper.From((request.GroupId, s.Id)).AdaptToType<GroupSystem>(); });

            await _dbContext.GroupSystems.AddRangeAsync(groupSystem, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var result = listSystems.Select(s => { return _mapper.From(s).AdaptToType<SystemDto>(); });

            return _mapper.From(result).AdaptToType<UpdateGroupSystemsResponse>();
        }
    }
}
