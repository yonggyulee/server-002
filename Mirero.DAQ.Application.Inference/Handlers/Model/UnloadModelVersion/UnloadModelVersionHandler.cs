using System.Text.Json.Nodes;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.Option;
using Mirero.DAQ.Infrastructure.Caching;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Grpc.Client;
using Mirero.DAQ.Infrastructure.Inference;
using Org.Pytorch.Serve.Grpc.Management;
using Polly;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.UnloadModelVersion;

public class UnloadModelVersionHandler : IRequestHandler<UnloadModelVersionCommand, Empty>
{
    private readonly ILogger<InferenceHandlerBase> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly WorkerRetryOption _retryOption;
    private readonly ICacheItemProvider<string, GrpcChannelData> _channelProvider;

    public UnloadModelVersionHandler(ILogger<InferenceHandlerBase> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, IMapper mapper,
        ICacheItemProvider<string, GrpcChannelData> channelProvider, WorkerRetryOption retryOption)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (dbContextFactory == null)
        {
            throw new ArgumentNullException(nameof(dbContextFactory));
        }

        _dbContext = dbContextFactory.CreateDbContext();
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _channelProvider = channelProvider;
        _retryOption = retryOption;
    }

    public async Task<Empty> Handle(UnloadModelVersionCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var policy = Policy.Handle<RpcException>().WaitAndRetryAsync(_retryOption.RetryCount,
            _ => TimeSpan.FromMilliseconds(_retryOption.RetryDelayMilliseconds),
            (ex, _) => _logger.LogError(ex, ex.Message));
        
        //var selectedEntity =
        //    await _dbContext.ModelDeploys
        //        .Include(md => md.Worker.Server)
        //        .Include(md => md.DefaultModelVersion.Model)
        //        .SingleOrDefaultAsync(im => im.Id == request.ModelDeployId, cancellationToken) ??
        //    throw new InvalidOperationException();

        //var key = nameof(InferenceWorker) + ":" + selectedEntity.Worker.Id;

        //var channel = _channelProvider.GetOrDefault(key);

        //if (channel == null)
        //{
        //    var inferencePort = JsonNode.Parse(selectedEntity.Worker.Properties)?["ManagementApi"];
        //    var address = "http://" + selectedEntity.Worker.Server.Address + ":" + inferencePort;
        //    _channelProvider.Add(key, address);
        //    channel = _channelProvider.Get(key);
        //}

        //var client = new ManagementAPIsService.ManagementAPIsServiceClient(channel.Channel);

        //var response = await policy.ExecuteAsync(async () => await client.UnregisterModelAsync(
        //    new UnregisterModelRequest
        //    {
        //        ModelName = selectedEntity.DefaultModelVersion.Model.ModelName,
        //    }));

        //_dbContext.ModelDeploys.Remove(selectedEntity);
        //await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}