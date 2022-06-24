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

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.DeleteClassificationGt;

public class DeleteClassificationGtHandler : GtDatasetHandler, IRequestHandler<DeleteClassificationGtCommand, ClassificationGt>
{
    private RequesterContext _requesterContext;

    public DeleteClassificationGtHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DeleteClassificationGtHandler> logger,
        IFileStorage fileStorage, IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
        _requesterContext = requesterContext;
    }
    public async Task<ClassificationGt> Handle(DeleteClassificationGtCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var deletedGt = new ClassificationGtEntity
        {
            Id = request.ClassificationGtId
        };

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");
        
        var classificationGtDataset =
            (await _dbContext.ClassificationGts.Include(gt => gt.GtDataset).Select(gt => new {gt.Id, gt.GtDataset})
                 .SingleOrDefaultAsync(gt => gt.Id == request.ClassificationGtId, cancellationToken) ??
             throw new InvalidOperationException($"ClassificationGt Id = {request.ClassificationGtId}")).GtDataset;

        _dbContext.ClassificationGts.Remove(deletedGt);
        classificationGtDataset.CreateUser = _requesterContext.UserName;
        classificationGtDataset.UpdateUser = _requesterContext.UserName;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(deletedGt).AdaptToType<ClassificationGt>();
    }
}
