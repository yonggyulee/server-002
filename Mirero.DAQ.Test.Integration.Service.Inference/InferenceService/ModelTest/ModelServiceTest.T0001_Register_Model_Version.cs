using Grpc.Core;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Inference.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Inference.InferenceService.ModelTest;

public partial class ModelServiceTest
{
    
    [Fact]
    public async Task T0001_Register_Model_Version()
    {
        // TODO : 다른 Endpoint 테스트 코드 구현 후 테스트 코드 정리 예정.
        // 현재 torchserve docker container가 실행 중이어야 테스트 가능.
        // torchserve docker container model-store에 해당 mar 파일이 있어야 테스트 가능.
        // (seed_data/sample_models 경로에 테스트용 모델 및 mar 파일.)
        var superUserOptions = await _GetAuthAsync();

        //var response = await _client.RegisterModelVersionAsync(new RegisterModelRequest
        //{
        //    ModelDeployId = "model_deploy_1",
        //    ModelId = 1,
        //    WorkerId = "worker_1",
        //    RegisterModelOptions = new RegisterModelOptions(),
        //}, superUserOptions);

        //Assert.NotNull(response);
    }

    [Fact]
    public async void T0001_Register_Model_Version_Retry_Exception()
    {
        var superUserOptions = await _GetAuthAsync();

        //RegisterModelResponse? response = null;
        //await Assert.ThrowsAnyAsync<RpcException>(async () =>
        //{
        //    response = await _client.RegisterModelVersionAsync(new RegisterModelRequest
        //    {
        //        ModelDeployId = "model_deploy_1",
        //        ModelId = 1,
        //        WorkerId = "worker_1",
        //        RegisterModelOptions = new RegisterModelOptions(),
        //}, superUserOptions);
        //});

        //Assert.NotNull(response);
    }

}