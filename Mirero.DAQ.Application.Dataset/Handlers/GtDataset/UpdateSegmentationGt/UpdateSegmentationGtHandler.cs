using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using SegmentationGtEntity = Mirero.DAQ.Domain.Dataset.Entities.SegmentationGt;

namespace Mirero.DAQ.Application.Dataset.Handlers.GtDataset.UpdateSegmentationGt;

public class UpdateSegmentationGtHandler : GtDatasetHandler, IRequestHandler<UpdateSegmentationGtCommand, SegmentationGt>
{
    private readonly RequesterContext _requesterContext;

    public UpdateSegmentationGtHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<UpdateSegmentationGtHandler> logger,
        IFileStorage fileStorage, IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage,
        mapper)
    {
        _requesterContext = requesterContext;
    }
    public async Task<SegmentationGt> Handle(UpdateSegmentationGtCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");
            
        var selectedGt = await _dbContext.SegmentationGts
                             .Include(s => s.GtDataset.Volume)
                             .SingleOrDefaultAsync(s => s.Id == request.Id,
                                 cancellationToken)
                         ?? throw new NotImplementedException();

        var gtDataset = selectedGt.GtDataset;

        await using var @lock =
            await _AcquireLockByGtDatasetAsync(selectedGt.GtDataset, request.LockTimeoutSec, true, cancellationToken);

        var originGt = _mapper.From(selectedGt).AdaptToType<SegmentationGtEntity>();

        var (originDatasetUri, newDatasetUri) = ("", "");

        if (request.Buffer != null)
        {
            originDatasetUri = Path.Combine(originGt.GtDataset.Volume.Uri, originGt.GtDataset.DirectoryName);

            newDatasetUri = (await _dbContext.GtDatasets
                                 .Include(d => d.Volume)
                                 .Select(d => new { d.Id, Uri = Path.Combine(d.Volume.Uri, d.DirectoryName) })
                                 .SingleOrDefaultAsync(d => d.Id == request.DatasetId, cancellationToken)
                             ?? throw new NotImplementedException()).Uri;

            originGt.Buffer = await _fileStorage.DeleteFileAsync(originDatasetUri, originGt.Filename, cancellationToken);
            await _fileStorage.SaveFileAsync(newDatasetUri, request.Filename, request.Buffer.ToByteArray(), cancellationToken);
        }

        try
        {
            _mapper.From(request).AdaptTo(selectedGt);
            gtDataset.CreateUser = _requesterContext.UserName;
            gtDataset.UpdateUser = _requesterContext.UserName;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            if (originGt.Buffer == null) throw;
            await _fileStorage.DeleteFileAsync(newDatasetUri, selectedGt.Filename, cancellationToken);
            await _fileStorage.SaveFileAsync(
                originDatasetUri, originGt.Filename, originGt.Buffer, cancellationToken);
            throw;
        }

        return _mapper.From(selectedGt).AdaptToType<SegmentationGt>();
    }
}
