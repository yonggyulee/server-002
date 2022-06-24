using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Account.Constants;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Infrastructure.Database.Account;
using FeatureDto = Mirero.DAQ.Domain.Account.Protos.V1.Feature;

namespace Mirero.DAQ.Application.Account.Handlers.Group.GetGroupFeatures
{
    public class GetGroupFeaturesHandler : IRequestHandler<GetGroupFeaturesCommand, GetGroupFeaturesResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public GetGroupFeaturesHandler(ILogger<GetGroupFeaturesHandler> logger, IMapper mapper,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<GetGroupFeaturesResponse> Handle(GetGroupFeaturesCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var checkGroup = await _dbContext.Groups.SingleOrDefaultAsync(s => s.Id == request.GroupId, cancellationToken) ??
                             throw new Exception($"{request.GroupId} doesn't exist.");

            var queryResult = await _dbContext.GroupFeatures
                .OrderBy(gf => gf.FeatureId)
                .Where(gf => gf.GroupId == request.GroupId)
                .AsNoTracking()
                .AsPagedResultAsync(request.QueryParameter, cancellationToken: cancellationToken);

            var features = typeof(FeatureId).GetFields().Select(p => p.Name).ToList();
            var enabledFeatures = queryResult.Items.Select(f => f.FeatureId);

            var groupFeaturDtos = features
                .Select(
                    f => enabledFeatures.Contains(f)
                        ? _mapper.From((f, f, true)).AdaptToType<FeatureDto>()
                        : _mapper.From((f, f, false)).AdaptToType<FeatureDto>()
                );

            return _mapper.From((request, groupFeaturDtos, queryResult.Count)).AdaptToType<GetGroupFeaturesResponse>();
        }
    }
}
