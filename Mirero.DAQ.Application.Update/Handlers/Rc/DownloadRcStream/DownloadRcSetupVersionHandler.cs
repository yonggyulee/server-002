using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Update.UriGenerator;
using Mirero.DAQ.Infrastructure.Database.Update;
using Mirero.DAQ.Infrastructure.Grpc.Extensions;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Update.Handlers.Rc.DownloadRcStream;

public sealed class DownloadRcSetupVersionHandler : IRequestHandler<DownloadRcSetupVersionCommand, Empty>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly UpdateDbContextInmemory _dbContext;
    private readonly IUriGenerator _uriGenerator;
    private readonly IFileStorage _fileStorage;

    public DownloadRcSetupVersionHandler(ILogger<DownloadRcSetupVersionHandler> logger, IMapper mapper, IConfiguration configuration,
        IDbContextFactory<UpdateDbContextInmemory> dbContextFactory, IUriGenerator uriGenerator, IFileStorage fileStorage)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _uriGenerator = uriGenerator ?? throw new ArgumentNullException(nameof(uriGenerator));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
    }

    public async Task<Empty> Handle(DownloadRcSetupVersionCommand setupVersionCommand, CancellationToken cancellationToken)
    {
        var volumeUri = await _dbContext.Volumes.FirstOrDefaultAsync(v => v.Id == "Rc", cancellationToken) ?? throw new Exception();
        var mppSetupVersion = await _dbContext.MppSetupVersions.FirstOrDefaultAsync(
            msv => msv.Id == setupVersionCommand.Request.RcSetupVersionId, cancellationToken) ?? throw new Exception("");

        var uri = _uriGenerator.GetMppSetupVersionUri(volumeUri.Uri, mppSetupVersion).AbsolutePath;

        await using (var fileStream = File.OpenRead(uri))
        {
            await fileStream.WriteGrpcServerStreamWriterAsync(setupVersionCommand.ResponseStream, setupVersionCommand.Request.ChunkSize, cancellationToken);
        }

        return new Empty();
    }
}