using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
using Mirero.DAQ.Test.Integration.Service.TestEnv;

namespace Mirero.DAQ.Test.Custom.YgLee.Dataset;
public static class DatasetClientTest
{
    internal static readonly string TestDataPath = "D:/workspace/TestData";
    internal static readonly string DownloadFolder = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName!, "download_images");
    internal static readonly string GtPath = "D:/workspace/TestData";
    public static void Test()
    {
        //TestDataGenerator.TestDataGenerate();
        //DatasetTest.Test();
        //new TestFileDataGenerator("C:/mirero/volumes").GenerateTestFileData();
        
        Console.WriteLine(System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name);
        Console.WriteLine(System.Reflection.Assembly.GetEntryAssembly()?.Location);
        Console.WriteLine(Environment.CurrentDirectory);
        Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

        //SampleTest.Test();

        //TrainingDataTest.Test();

        // EtcTest.TestCompareDateTime();

        // EtcTest.TestParseQureyString();

        // EtcTest.TestPathCombine();

        // EtcTest.TestEnumerateFiles();

        // EtcTest.TestBook();

        // EtcTest.TestDirectory();

        // EtcTest.TestMoveDir();

        // ImageTest.Test();

        //ClassCodeSetTest.Test();

        //ClassCodeTest.Test();

        // CGtDatasetTest.Test();
        //
        //OdGtDatasetTest.Test();

        //SgGtDatasetTest.Test();

        // GtDatasetTest.Test();

        //InheritTest.Test();

        // CGtTest.Test();

        //OdGtTest.Test();

        //SgGtTest.Test();

        //QueryTest.Test();

        //EtcTest.test_1111();
    }
}
