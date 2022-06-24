using System.Linq;
using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.UriGenerator;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ModelDto = Mirero.DAQ.Domain.Inference.Protos.V1.Model;
using ModelVersionEntity = Mirero.DAQ.Domain.Inference.Entities.ModelVersion;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.UploadModelVersionStream;

public class UploadModelVersionStreamHandler : IRequestHandler<UploadModelVersionStreamCommand, Empty>
{
    private readonly ILogger<UploadModelVersionStreamHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly ILockProvider _lockProvider;
    private readonly IMapper _mapper;
    private readonly IUriGenerator _uriGenerator;
    

    public UploadModelVersionStreamHandler(ILogger<UploadModelVersionStreamHandler> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, IFileStorage fileStorage,
        IPostgresLockProviderFactory lockProviderFactory, IMapper mapper,
        IUriGenerator uriGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (dbContextFactory == null)
        {
            throw new ArgumentNullException(nameof(dbContextFactory));
        }
        _dbContext = dbContextFactory.CreateDbContext();
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        if (lockProviderFactory == null)
        {
            throw new ArgumentNullException(nameof(lockProviderFactory));
        }
        _lockProvider = lockProviderFactory.CreateLockProvider(_dbContext);
        _uriGenerator = uriGenerator ?? throw new ArgumentNullException(nameof(uriGenerator));
    }

    public async Task<Empty> Handle(UploadModelVersionStreamCommand command, CancellationToken cancellationToken)
    {
        var requestStream = command.RequestStream;
        
        if (!await requestStream.MoveNext(cancellationToken))
        {
            throw new NotImplementedException();
        }

        var message = requestStream.Current;

        var modelVersion = _mapper.From(message ?? throw new NotImplementedException())
            .AdaptToType<ModelVersionEntity>();

        await using var @lock = await _lockProvider.AcquireWriteLockAsync(_lockProvider.GenerateLockId<ModelDto>(modelVersion.ModelId),
            message.LockTimeoutSec, 
            cancellationToken: cancellationToken);

        var uri = (await _dbContext.Models.Include(m => m.Volume)
                       .Select(m => new
                       {
                           m.Id,
                           Uri = _uriGenerator.GetModelVersionUri(
                               m.Volume.Uri, 
                               m.ModelName, 
                               modelVersion.Version, 
                               modelVersion.Filename, false)
                       })
                       .SingleOrDefaultAsync(m => m.Id == modelVersion.ModelId, cancellationToken) ??
                   throw new NullReferenceException()).Uri;

        await using (var fs = _fileStorage.CreateFileStream(uri))
        {
            do
            {
                var buffer = message.Buffer.ToByteArray();
                await fs.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
            } while (await requestStream.MoveNext(cancellationToken));
        }

        try
        {
            await _dbContext.ModelVersions.AddAsync(modelVersion, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch 
        {
            _fileStorage.DeleteFolder(Path.GetDirectoryName(uri)!);
            throw;
        }

        return new Empty();
    }
}