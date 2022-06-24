using Google.Protobuf;
using Google.Protobuf.Collections;
using Grpc.Core;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Inference.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Inference.InferenceService.InferenceTest;

public partial class InferenceServiceTest
{
    [Fact]
    public async void T01_Prediction()
    {
        // TODO : 다른 Endpoint 테스트 코드 구현 후 테스트 코드 정리 예정.
        // 현재 TorchServe docker container가 실행 중이어야 테스트 가능.
        var superUserOptions = await _GetAuthAsync();

        //var registerResponse = await _modelServiceClient.RegisterModelVersionAsync(new RegisterModelRequest
        //{
        //    ModelDeployId = "model_deploy_1",
        //    ModelId = 1,
        //    WorkerId = "worker_1",
        //    RegisterModelOptions = new RegisterModelOptions
        //    {
        //        InitialWorkers = 1,
        //    }
        //}, superUserOptions);

        //Assert.NotNull(registerResponse);

        var bytes = await File.ReadAllBytesAsync(
            Path.Combine(CurrentPath ?? throw new InvalidOperationException(),
            "seed_data\\test_images", "310007.jpg"));

        var dict = new Dictionary<string, ByteString> {{"data", ByteString.CopyFrom(bytes)}};

        PredictionResponse? response = null;

        await Assert.ThrowsAsync<RpcException>(async () =>
        {
            response = await _client.PredictionAsync(new PredictionRequest
        {
            ModelDeployId = "model_deploy_1",
            Input = { dict },
        }, superUserOptions);
        });

        Assert.NotNull(response);

        var resultPath = Path.Combine(CurrentPath, "test_result", "test_prediction_result_01.txt");

        Directory.CreateDirectory(Path.GetDirectoryName(resultPath)!);

        await File.WriteAllBytesAsync(resultPath, response!.Prediction.ToByteArray());
    }

    [Fact]
    public async void T01_Prediction_Retry_Exception()
    {
        var superUserOptions = await _GetAuthAsync();

        var bytes = await File.ReadAllBytesAsync(
            Path.Combine(CurrentPath ?? throw new InvalidOperationException(),
                "seed_data\\test_images", "310007.jpg"));

        var dict = new Dictionary<string, ByteString> { { "data", ByteString.CopyFrom(bytes) } };

        await Assert.ThrowsAnyAsync<RpcException>(async () =>
        {
            await _client.PredictionAsync(new PredictionRequest
            {
                ModelDeployId = "model_deploy_1",
                Input = { dict },
            }, superUserOptions);
        });
    }
}