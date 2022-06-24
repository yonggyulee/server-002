using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

using SampleEntity = Mirero.DAQ.Domain.Dataset.Entities.Sample;

namespace Mirero.DAQ.Application.Dataset.Handlers.ImageDataset;

public class SampleHandler : DatasetHandlerBase
{
    public SampleHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory, IPostgresLockProviderFactory lockProviderFactory, ILogger<DatasetHandlerBase> logger, IFileStorage fileStorage, IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    protected async Task<Sample> _ToSampleWithImageAsync(SampleEntity sample, CancellationToken cancellationToken)
    {
        sample.Images = (await Task.WhenAll(sample.Images.Select(async i =>
        {
            i.Buffer = await _fileStorage.GetFileBufferAsync(
                Path.Combine(
                    sample.ImageDataset.Volume.Uri,
                    sample.ImageDataset.DirectoryName),
                i.Filename, cancellationToken);
            return i;
        }))).ToList();
        return _ToSample(sample);
    }

    protected Sample _ToSample(SampleEntity model)
    {
        var sample = _mapper.From(model).AdaptToType<Sample>();
        sample.Images.AddRange(
            model.Images.Select(
                i => _mapper.From(i).AdaptToType<Image>()));
        return sample;
    }
}