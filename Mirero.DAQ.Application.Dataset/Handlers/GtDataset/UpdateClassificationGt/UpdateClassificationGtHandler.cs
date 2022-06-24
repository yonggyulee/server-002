using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ClassificationGtEntity = Mirero.DAQ.Domain.Dataset.Entities.ClassificationGt;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateClassificationGt;

public class UpdateClassificationGtHandler : GtDatasetHandler, IRequestHandler<UpdateClassificationGtCommand, ClassificationGt>
{
    private readonly RequesterContext _requesterContext;

    public UpdateClassificationGtHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<UpdateClassificationGtHandler> logger,
        IFileStorage fileStorage, IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
        _requesterContext = requesterContext;
    }
    public async Task<ClassificationGt> Handle(UpdateClassificationGtCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var classificationGt = _mapper.From(request).AdaptToType<ClassificationGtEntity>();

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");
        
        var gtDataset =
            await _dbContext.ClassificationGtDatasets.SingleOrDefaultAsync(d => d.Id == request.DatasetId,
                cancellationToken) ?? throw new InvalidOperationException($"GtDataset Id = {request.DatasetId}");
        
        _dbContext.ClassificationGts.Update(classificationGt);
        gtDataset.CreateUser = _requesterContext.UserName;
        gtDataset.UpdateUser = _requesterContext.UserName;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(classificationGt).AdaptToType<ClassificationGt>();
    }
}
