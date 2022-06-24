using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;
using SystemDto = Mirero.DAQ.Domain.Account.Protos.V1.System;
using SystemEntity = Mirero.DAQ.Domain.Account.Entities.System;

namespace Mirero.DAQ.Application.Account.Handlers.Group.UpdateSystem
{
    public class UpdateSystemHandler : IRequestHandler<UpdateSystemCommand, SystemDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public UpdateSystemHandler(ILogger<UpdateSystemHandler> logger, IMapper mapper,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<SystemDto> Handle(UpdateSystemCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var system = _mapper.From(request).AdaptToType<SystemEntity>();

            _dbContext.Systems.Update(system);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var result =
                await _dbContext.Systems.SingleOrDefaultAsync(s => s.Id == system.Id, cancellationToken: cancellationToken)
                ?? throw new Exception($"{system.Id} doesn't exist.");

            return _mapper.From(result).AdaptToType<SystemDto>();
        }
    }
}
