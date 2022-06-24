using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.GetClassificationGt;

public class GetClassificationGtHandler : GtDatasetHandler, IRequestHandler<GetClassificationGtCommand, ClassificationGt>
{
    public GetClassificationGtHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<GetClassificationGtHandler> logger,
        IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
    }
    public async Task<ClassificationGt> Handle(GetClassificationGtCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var classificationGt = await _dbContext.ClassificationGts.AsNoTracking()
                                   .Include(c => c.ClassCode)
                                   .SingleOrDefaultAsync(
                                       c => c.Id == request.ClassificationGtId,
                                       cancellationToken)
                               ?? throw new NotImplementedException();
        return _mapper.From(classificationGt).AdaptToType<ClassificationGt>();
    }
}