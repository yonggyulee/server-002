using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Account.Constants;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;
using FeatureDto = Mirero.DAQ.Domain.Account.Protos.V1.Feature;

namespace Mirero.DAQ.Application.Account.Handlers.Group.ListFeatures
{
    public class ListFeaturesHandler : IRequestHandler<ListFeaturesCommand, ListFeaturesResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ListFeaturesHandler(ILogger<ListFeaturesHandler> logger, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ListFeaturesResponse> Handle(ListFeaturesCommand command, CancellationToken cancellationToken)
        {
            var request = command;
            var features = typeof(FeatureId).GetFields().Select(p => p.Name).ToList();
            var featureDtos = features.Select((f) => { return _mapper.From((f, f)).AdaptToType<FeatureDto>(); });

            return _mapper.From((request, featureDtos, features.Count)).AdaptToType<ListFeaturesResponse>();
        }
    }
}
