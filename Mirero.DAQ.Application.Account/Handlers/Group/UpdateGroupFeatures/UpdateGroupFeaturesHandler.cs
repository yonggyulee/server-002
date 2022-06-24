using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Account.Constants;
using Mirero.DAQ.Domain.Account.Entities;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;
using FeatureDto = Mirero.DAQ.Domain.Account.Protos.V1.Feature;

namespace Mirero.DAQ.Application.Account.Handlers.Group.UpdateGroupFeatures
{
    public class UpdateGroupFeaturesHandler : IRequestHandler<UpdateGroupFeaturesCommand, UpdateGroupFeaturesResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public UpdateGroupFeaturesHandler(ILogger<UpdateGroupFeaturesHandler> logger, IMapper mapper,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }
        public async Task<UpdateGroupFeaturesResponse> Handle(UpdateGroupFeaturesCommand commnad, CancellationToken cancellationToken)
        {
            var request = commnad.Request;
            var listFeatures = typeof(FeatureId).GetFields().Select(p => p.Name).ToList();
            var enabledFeatures = request.Features
                .Where(f => f.Enabled.Equals(true))
                .Select(f => f.Id).ToList();

            var deleteGroup = _dbContext.GroupFeatures
                                  .Where(gf => gf.GroupId == request.GroupId).ToList()
                              ?? throw new Exception($"{request.GroupId} doesn't exist.");

            _dbContext.GroupFeatures.RemoveRange(deleteGroup);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var groupFeatures = enabledFeatures
                .Select(f => { return _mapper.From((request.GroupId, f)).AdaptToType<GroupFeature>(); });

            await _dbContext.GroupFeatures.AddRangeAsync(groupFeatures, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var featureDtos = listFeatures
                .Select(
                    f => enabledFeatures.Contains(f)
                        ? _mapper.From((f, f, true)).AdaptToType<FeatureDto>()
                        : _mapper.From((f, f, false)).AdaptToType<FeatureDto>()
                );

            return _mapper.From(featureDtos).AdaptToType<UpdateGroupFeaturesResponse>();
        }
    }
}
