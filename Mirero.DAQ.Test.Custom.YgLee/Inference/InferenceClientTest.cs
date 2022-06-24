using Mirero.DAQ.Test.Custom.YgLee.Inference.Unit;

namespace Mirero.DAQ.Test.Custom.YgLee.Inference;

public class InferenceClientTest
{
    internal static readonly string TestDataPath = "D:/workspace/TestData";
    internal static readonly string DownloadFolder = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName!, "download_images");
    internal static readonly string GtPath = "D:/workspace/TestData";

    public static void Test()
    {
        //VolumeTest.Test();
        //ServerTest.Test();
        //InferenceWorkerTest.Test();
        //TrainingWorkerTest.Test();
        //ModelTest.Test();
        // ModelVersionTest.Test();
        //EtcTest.TestFileStream();
        //EtcTest.TestEnum();
        //TestDataGenerator.TestDataGenerate();
        InferenceTest.Test();
    }
}