//using System.Text;

//namespace Mirero.DAQ.Test.Custom.YgLee.MachineLearning.Unit;

//public class EtcTest
//{
//    public static void TestFileStream()
//    {
//        const string DownloadFolder = "D:/workspace/daq-server/Src/Mirero.DAQ.Test.Custom.YgLee/ml_download_data/";
//        const string TestDataPath = "D:/workspace/TestData/TestModel_mar";

//        var uri = Path.Combine(TestDataPath, "image2_2.jpg");
//        using var fs = new FileStream(uri, FileMode.Open, FileAccess.Read);

//        var chunk_size = 32 * 1024;
//        var chunk = new byte[chunk_size];

//        var downUri = Path.Combine(DownloadFolder, "test_file_stream", "test2.jpg");

//        var testDir = Path.GetDirectoryName(downUri);

//        if (testDir != null)
//        {
//            Directory.CreateDirectory(testDir);
//        }

//        if (!File.Exists(downUri))
//        {
//            File.Create(downUri);
//        }

//        using var downFs = new FileStream(downUri, FileMode.Open, FileAccess.Write);

//        int n;
//        while ((n = fs.Read(chunk, 0, chunk_size)) != 0)
//        {
//            Console.WriteLine(n);
//            downFs.Write(chunk);
//        }
//    }

//    public static void TestEnum()
//    {
//        Console.WriteLine(Status.Ready);
//        Console.WriteLine(Status.Ready.ToString());
//    }

//    private enum Status
//    {
//        Start,
//        End,
//        Ready
//    }
//}