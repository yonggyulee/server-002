using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Dataset.Handlers.GtDataset;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ClassificationGtEntity = Mirero.DAQ.Domain.Dataset.Entities.ClassificationGt;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.CreateClassificationGt;

public class CreateClassificationGtHandler : GtDatasetHandler, IRequestHandler<CreateClassificationGtCommand, ClassificationGt>
{
    private readonly RequesterContext _requesterContext;

    public CreateClassificationGtHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<CreateClassificationGtHandler> logger,
        IFileStorage fileStorage, IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
        _requesterContext = requesterContext;
    }
    public async Task<ClassificationGt> Handle(CreateClassificationGtCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");

        var classificationGt = _mapper.From(request).AdaptToType<ClassificationGtEntity>();

        var gtDataset =
            await _dbContext.GtDatasets.SingleOrDefaultAsync(d => d.Id == request.DatasetId, cancellationToken) ??
            throw new InvalidOperationException($"GtDataset Id = {request.DatasetId}");

        await _dbContext.ClassificationGts.AddAsync(classificationGt, cancellationToken);
        gtDataset.CreateUser = _requesterContext.UserName;
        gtDataset.UpdateUser = _requesterContext.UserName;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(classificationGt).AdaptToType<ClassificationGt>();
    }
}
