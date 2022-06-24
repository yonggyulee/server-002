using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

using ImageDatasetDto = Mirero.DAQ.Domain.Dataset.Protos.V1.ImageDataset;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.DeleteSample;

public class DeleteSampleHandler : SampleHandler, IRequestHandler<DeleteSampleCommand, Sample>
{
    private readonly RequesterContext _requesterContext;

    public DeleteSampleHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<DeleteSampleHandler> logger, IFileStorage fileStorage,
        IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
        _requesterContext = requesterContext;
    }

    public async Task<Sample> Handle(DeleteSampleCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");
        
        var selectedSample = await _dbContext.Samples
                                 .Include(s => s.Images)
                                 .Include(s => s.ImageDataset.Volume)
                                 .SingleOrDefaultAsync(
                                     s =>
                                         s.Id == request.SampleId && s.DatasetId == request.DatasetId,
                                     cancellationToken: cancellationToken)
                             ?? throw new NotImplementedException();

        var dataset = selectedSample.ImageDataset;

        await using var @lock = await _lockProvider.AcquireWriteLockAsync(
            GenerateLockId<ImageDatasetDto>(selectedSample.DatasetId), request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        var (deleteUris, deletedFileBuffers) = await DeleteImageFilesAsync(selectedSample);

        try
        {
            _dbContext.Samples.Remove(selectedSample);
            dataset.CreateUser = _requesterContext.UserName;
            dataset.UpdateUser = _requesterContext.UserName;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _fileStorage.SaveFilesAsync(deleteUris, deletedFileBuffers, cancellationToken);
            throw;
        }

        return _ToSample(selectedSample);
    }
}
