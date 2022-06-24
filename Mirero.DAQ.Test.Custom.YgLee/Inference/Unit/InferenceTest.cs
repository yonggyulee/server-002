using Google.Protobuf;
using Grpc.Net.Client;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Test.Custom.YgLee.Inference.Unit;

public class InferenceTest
{
    private static InferenceService.InferenceServiceClient _client;
    private const string DownloadFolder = "D:/workspace/daq-server/Src/Mirero.DAQ.Test.Custom.YgLee/ml_download_data/";
    private static readonly string TestDataPath = Path.Combine(InferenceClientTest.TestDataPath, "TestImages");
    
    public static void Test()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:5020");
        _client = new InferenceService.InferenceServiceClient(channel);
        
        TestPrediction();
    }
    
    private static void TestPrediction()
    {
        var bytes = File.ReadAllBytes(
            Path.Combine(TestDataPath ?? throw new InvalidOperationException(),
                "310007.jpg"));

        var dict = new Dictionary<string, ByteString> {{"data", ByteString.CopyFrom(bytes)}};

        var response = _client.Prediction(new PredictionRequest
        {
            ModelDeployId = "model_deploy_1",
            Input = {dict},
        });
        
        var resultPath = Path.Combine(DownloadFolder, "test_result", "test_prediction_result_01.txt");

        Directory.CreateDirectory(Path.GetDirectoryName(resultPath)!);

        File.WriteAllBytes(resultPath, response!.Prediction.ToByteArray());
    }
}