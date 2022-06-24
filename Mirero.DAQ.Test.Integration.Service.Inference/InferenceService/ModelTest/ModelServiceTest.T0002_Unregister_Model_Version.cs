using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Inference.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Inference.InferenceService.ModelTest;

public partial class ModelServiceTest
{
    [Fact]
    public async Task T0002_Unregister_Model_Version()
    {
        // TODO : 다른 Endpoint 테스트 코드 구현 후 테스트 코드 정리 예정.
        // 현재 torchserve docker container가 실행 중이어야 테스트 가능.
        var superUserOptions = await _GetAuthAsync();

        //var response = await _client.UnregisterModelVersionAsync(new UnregisterModelVersionRequest
        //{
        //    ModelDeployId = "model_deploy_1",
        //}, superUserOptions);

        //Assert.NotNull(response);
    }

    [Fact]
    public async Task T0002_Unregister_Model_Version_Retry_Exception()
    {
        var superUserOptions = await _GetAuthAsync();

        Empty? response = null;
        //await Assert.ThrowsAnyAsync<RpcException>(async () =>
        //{
        //    response = await _client.UnregisterModelVersionAsync(new UnregisterModelVersionRequest
        //    {
        //        ModelDeployId = "model_deploy_1",
        //}, superUserOptions);
        //});

        //Assert.NotNull(response);
    }

}