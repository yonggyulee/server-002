using System.Text.Json.Nodes;
using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.Option;
using Mirero.DAQ.Infrastructure.Caching;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Grpc.Client;
using Mirero.DAQ.Infrastructure.Inference;
using Org.Pytorch.Serve.Grpc.Inference;
using Polly;
using PredictionResponse = Mirero.DAQ.Domain.Inference.Protos.V1.PredictionResponse;

namespace Mirero.DAQ.Application.Inference.Handlers.Inference.Prediction;

public class PredictionHandler : IRequestHandler<PredictionCommand, PredictionResponse>
{
    private readonly ILogger<PredictionHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly WorkerRetryOption _retryOption;
    private readonly ICacheItemProvider<string, GrpcChannelData> _channelProvider;

    public PredictionHandler(ILogger<PredictionHandler> logger, 
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, WorkerRetryOption retryOption, 
        ICacheItemProvider<string, GrpcChannelData> channelProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (dbContextFactory == null)
        {
            throw new ArgumentNullException(nameof(dbContextFactory));
        }
        _dbContext = dbContextFactory.CreateDbContext();
        _retryOption = retryOption;

        _channelProvider = channelProvider;
    }

    public async Task<PredictionResponse> Handle(PredictionCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        // TODO : Exception 구현 예정.
        var policy = Policy.Handle<RpcException>().WaitAndRetryAsync(_retryOption.RetryCount,
            _ => TimeSpan.FromMilliseconds(_retryOption.RetryDelayMilliseconds),
            (ex, _) => _logger.LogError(ex, ex.Message));

        //var selectedEntity = await _dbContext.ModelDeploys
        //                         .Include(md => md.Worker.Server)
        //                         .Include(md => md.DefaultModelVersion.Model)
        //                         .SingleOrDefaultAsync(
        //                             im => im.Id == request.ModelDeployId,
        //                             cancellationToken)
        //                     ?? throw new InvalidOperationException();

        //var key = nameof(InferenceWorker) + ":" + selectedEntity.WorkerId;

        //var channel = _channelProvider.GetOrDefault(key);

        //if (channel == null)
        //{
        //    var inferencePort = JsonNode.Parse(selectedEntity.Worker.Properties)?["InferenceApi"];
        //    var address = "http://" + selectedEntity.Worker.Server.Address + ":" + inferencePort;
        //    _channelProvider.Add(key, address);
        //    channel = _channelProvider.Get(key);
        //}

        //var client = new InferenceAPIsService.InferenceAPIsServiceClient(channel.Channel);
        
        //var response = await policy.ExecuteAsync(async () => await client.PredictionsAsync(new PredictionsRequest
        //{
        //    ModelName = selectedEntity.DefaultModelVersion.Model.ModelName,
        //    Input = {request.Input}
        //}));

        return new PredictionResponse
        {
            //Prediction = response.Prediction
        };
    }
}
