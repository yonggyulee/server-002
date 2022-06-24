using System.Text.Json.Nodes;
using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.Handlers.Model.RegisterModelVersion;
using Mirero.DAQ.Application.Inference.Option;
using Mirero.DAQ.Application.Inference.UriGenerator;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Inference.Protos.V1;
using Mirero.DAQ.Infrastructure.Caching;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Grpc.Client;
using Mirero.DAQ.Infrastructure.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Org.Pytorch.Serve.Grpc.Management;
using Polly;
using WorkerEntity = Mirero.DAQ.Domain.Inference.Entities.Worker;
using ModelVersionEntity = Mirero.DAQ.Domain.Inference.Entities.ModelVersion;
using ManagementRegisterModelRequest = Org.Pytorch.Serve.Grpc.Management.RegisterModelRequest;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.LoadModelVersion;

public class LoadModelVersionHandler : IRequestHandler<LoadModelVersionCommand, LoadModelResponse>
{
    private readonly RequesterContext _requesterContext;
    private readonly ILogger<LoadModelVersionHandler> _logger;
    private readonly ILockProvider _lockProvider;
    private readonly InferenceDbContext _dbContext;
    private readonly IUriGenerator _uriGenerator;
    private readonly WorkerRetryOption _retryOption;
    private readonly ICacheItemProvider<string, GrpcChannelData> _channelProvider;

    public LoadModelVersionHandler(ILogger<LoadModelVersionHandler> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, RequesterContext requesterContext, IUriGenerator uriGenerator,
        WorkerRetryOption retryOption, ICacheItemProvider<string, GrpcChannelData> channelProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (dbContextFactory == null)
        {
            throw new ArgumentNullException(nameof(dbContextFactory));
        }

        _dbContext = dbContextFactory.CreateDbContext();
        if (lockProviderFactory == null)
        {
            throw new ArgumentNullException(nameof(lockProviderFactory));
        }

        _lockProvider = lockProviderFactory.CreateLockProvider(_dbContext);
        _requesterContext = requesterContext ?? throw new ArgumentNullException(nameof(requesterContext));
        _uriGenerator = uriGenerator ?? throw new ArgumentNullException(nameof(uriGenerator));
        _channelProvider = channelProvider;
        _retryOption = retryOption;
    }

    public async Task<LoadModelResponse> Handle(LoadModelVersionCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var policy = Policy.Handle<RpcException>().WaitAndRetryAsync(_retryOption.RetryCount,
            _ => TimeSpan.FromMilliseconds(_retryOption.RetryDelayMilliseconds),
            (ex, _) => _logger.LogError(ex, ex.Message));

        var modelVersion = request.ModelVersionId != null
            ? await _dbContext.ModelVersions.Include(mv => mv.Model.Volume)
                  .SingleOrDefaultAsync(mv => mv.Id == request.ModelVersionId, cancellationToken) ??
              throw new NullReferenceException()
            : (await _dbContext.DefaultModelVersions.Include(dmv => dmv.ModelVersion.Model.Volume)
                   .SingleOrDefaultAsync(dmv => dmv.ModelId == request.ModelId, cancellationToken: cancellationToken) ??
               throw new NullReferenceException()).ModelVersion;

        var modelUri = _uriGenerator.GetModelVersionUri(modelVersion.Model.Volume.Uri,
            modelVersion.Model.ModelName,
            modelVersion.Version,
            modelVersion.Filename,
            serving: true);

        var server = await _dbContext.Servers.SingleOrDefaultAsync(s => s.Id == request.ServerId, cancellationToken) ??
                     throw new NullReferenceException();

        var worker = new WorkerEntity
        {
            Id = "",
            ServerId = request.ServerId,
            CpuCount = request.LoadModelOptions.CpuCount,
            CpuMemory = request.LoadModelOptions.UsingMemory,   // x CpuMemoryRatio
            GpuCount = request.LoadModelOptions.GpuCount ?? 0,
            // GpuMemory = 0,  TODO : 남은 메모리 계산
            Properties = request.Properties,
            Description = request.Description,
        };

        // TODO : Inference Worker Start.

        //var worker =
        //    await _dbContext.Workers.Include(w => w.Server)
        //        .SingleOrDefaultAsync(w => w.Id == request.WorkerId, cancellationToken) ??
        //    throw new NullReferenceException($"Worker({request.WorkerId}) is null.");

        //var key = nameof(InferenceWorker) + ":" + worker.Id;

        //var channel = _channelProvider.GetOrDefault(key);

        //if (channel == null)
        //{
        //    var inferencePort = JsonNode.Parse(worker.Properties)?["ManagementApi"];
        //    var address = "http://" + worker.Server.Address + ":" + inferencePort;
        //    _channelProvider.Add(key, address);
        //    channel = _channelProvider.Get(key);
        //}

        //var client = new ManagementAPIsService.ManagementAPIsServiceClient(channel.Channel);

        //var response = await policy.ExecuteAsync(async () => await client.RegisterModelAsync(
        //    new ManagementRegisterModelRequest
        //    {
        //        ModelName = modelVersion.Model.ModelName,
        //        Url = modelUri,
        //        BatchSize = request.LoadModelOptions.BatchSize ?? 1,
        //        //Handler = request.RegisterModelOptions.Handler,
        //        InitialWorkers = request.LoadModelOptions.InitialWorkers ?? 0,
        //        MaxBatchDelay = request.LoadModelOptions.MaxBatchDelay ?? 100,
        //        ResponseTimeout = request.LoadModelOptions.ResponseTimeout ?? 120,
        //        //Runtime = request.RegisterModelOptions.Runtime,
        //        Synchronous = request.LoadModelOptions.Synchronous,
        //        S3SseKms = request.LoadModelOptions.S3SseKms,
        //    }));

        try
        {
            await _dbContext.Workers.AddAsync(worker, cancellationToken);
            modelVersion.Status = "Loaded";
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            //await client.UnregisterModelAsync(new UnregisterModelRequest
            //{
            //    ModelName = modelVersion.Model.ModelName,
            //    ModelVersion = InferenceWorker.DefaultModelVersion,
            //});
            throw;
        }

        return new LoadModelResponse
        {
            //LoadMsg = response.Msg
            LoadMsg = ""
        };
    }
}
