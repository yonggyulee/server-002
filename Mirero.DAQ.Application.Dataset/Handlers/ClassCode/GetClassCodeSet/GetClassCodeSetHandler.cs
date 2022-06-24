using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.GetClassCodeSet;

public class GetClassCodeSetHandler : DatasetHandlerBase, IRequestHandler<GetClassCodeSetCommand, ClassCodeSet>
{
    public GetClassCodeSetHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<GetClassCodeSetHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }

    public async Task<ClassCodeSet> Handle(GetClassCodeSetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var classCodeSet = await _dbContext.ClassCodeSets.AsNoTracking()
                               .SingleOrDefaultAsync(c =>
                                   c.Id == request.ClassCodeSetId, cancellationToken: cancellationToken)
                           ?? throw new InvalidOperationException($"ClassCodeSet Id={request.ClassCodeSetId}");

        return _mapper.From(classCodeSet).AdaptToType<ClassCodeSet>();
    }
}