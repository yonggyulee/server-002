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

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset.CreateSample;

public class CreateSampleHandler : SampleHandler, IRequestHandler<CreateSampleCommand, Sample>
{
    private readonly RequesterContext _requesterContext;
    public CreateSampleHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<CreateSampleHandler> logger, IFileStorage fileStorage,
        IMapper mapper, RequesterContext requesterContext) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
        _requesterContext = requesterContext;
    }

    public async Task<Sample> Handle(CreateSampleCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var sample = _mapper.From(request.Sample).AdaptToType<SampleEntity>();

        if (_requesterContext.UserName is null)
            throw new InvalidOperationException("사용자명이 RequestContext에 존재하지 않습니다.");
        
        await using var @lock =
            await _lockProvider.AcquireWriteLockAsync(GenerateLockId<ImageDatasetDto>(sample.DatasetId),
                request.LockTimeoutSec,
                cancellationToken: cancellationToken);

        if (sample.Images.Count == 0)
        {
            await _dbContext.Samples.AddAsync(sample, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.From(sample).AdaptToType<Sample>();
        }

        var dataset = (await _dbContext.ImageDatasets
                           .Include(d => d.Volume)
                           .SingleOrDefaultAsync(d => d.Id == sample.DatasetId,
                               cancellationToken: cancellationToken)
                       ?? throw new InvalidOperationException($"ImageDataset Id = {sample.DatasetId}"));

        var datasetUri = Path.Combine(dataset.Volume.Uri, dataset.DirectoryName);

        var imageFilenames = await SaveImageFilesAsync(datasetUri, sample);

        try
        {
            await _dbContext.Samples.AddAsync(sample, cancellationToken);
            dataset.CreateUser = _requesterContext.UserName;
            dataset.UpdateUser = _requesterContext.UserName;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _fileStorage.DeleteFilesAsync(datasetUri, imageFilenames, cancellationToken);
            throw;
        }

        return _ToSample(sample);
    }
}
