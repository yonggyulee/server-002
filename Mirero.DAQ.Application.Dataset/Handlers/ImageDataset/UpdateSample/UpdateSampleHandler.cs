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
using SampleEntity = Mirero.DAQ.Domain.Dataset.Entities.Sample;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.UpdateSample;

public class UpdateSampleHandler : SampleHandler, IRequestHandler<UpdateSampleCommand, Sample>
{
    private readonly RequesterContext _requesterContext;

    public UpdateSampleHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<UpdateSampleHandler> logger, IFileStorage fileStorage,
        IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
        _requesterContext = requesterContext;
    }

    public async Task<Sample> Handle(UpdateSampleCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");
        
        var sample = _mapper.From(request.Sample).AdaptToType<SampleEntity>();

        var selectedSample = await _dbContext.Samples
                                 .Include(s => s.ImageDataset.Volume)
                                 .Include(s => s.Images)
                                 .SingleOrDefaultAsync(s =>
                                         s.Id == sample.Id && s.DatasetId == sample.DatasetId,
                                     cancellationToken: cancellationToken)
                             ?? throw new NotImplementedException();

        var dataset = selectedSample.ImageDataset;

        await using var @lock =
            await _lockProvider.AcquireWriteLockAsync(GenerateLockId<ImageDatasetDto>(sample.DatasetId),
                request.LockTimeoutSec,
                cancellationToken: cancellationToken);

        var (deleteUris, deletedFileBuffers) = await DeleteImageFilesAsync(selectedSample);
        var newUris = await SaveImageFilesAsync(
            Path.Combine(selectedSample.ImageDataset.Volume.Uri, selectedSample.ImageDataset.DirectoryName), sample);

        try
        {
            _mapper.From(sample).AdaptTo(selectedSample);
            dataset.CreateUser = _requesterContext.UserName;
            dataset.UpdateUser = _requesterContext.UserName;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _fileStorage.DeleteFilesAsync(newUris, cancellationToken);
            await _fileStorage.SaveFilesAsync(deleteUris, deletedFileBuffers, cancellationToken);
            throw;
        }

        return _ToSample(sample);
    }
}
