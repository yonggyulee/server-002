using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;
using GroupDto = Mirero.DAQ.Domain.Account.Protos.V1.Group;
using GroupEntity = Mirero.DAQ.Domain.Account.Entities.Group;

namespace Mirero.DAQ.Application.Account.Handlers.Group.UpdateGroup
{
    public class UpdateGroupHandler : IRequestHandler<UpdateGroupCommand, GroupDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public UpdateGroupHandler(ILogger<UpdateGroupHandler> logger, IMapper mapper,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<GroupDto> Handle(UpdateGroupCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var selectedGroup = await _dbContext.Groups.SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken)
                                ?? throw new Exception($"{request.Id} doesn't exist.");
            var group = _mapper.From(request).AdaptToType<GroupEntity>();

            selectedGroup.Title = group.Title == null ? selectedGroup.Title : group.Title;
            selectedGroup.Description = group.Description == null ? selectedGroup.Description : group.Description;
            selectedGroup.Properties = group.Properties == null ? selectedGroup.Properties : group.Properties;

            _dbContext.Groups.Update(selectedGroup);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var result =
                await _dbContext.Groups.SingleOrDefaultAsync(g => g.Id == group.Id, cancellationToken: cancellationToken)
                ?? throw new Exception($"{group.Id} doesn't exist.");

            return _mapper.From(result).AdaptToType<GroupDto>();
        }
    }
}
