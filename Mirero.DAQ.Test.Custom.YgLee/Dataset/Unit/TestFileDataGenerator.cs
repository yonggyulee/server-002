using System;
using System.IO;
using System.Linq;

namespace Mirero.DAQ.Test.Integration.Service.TestEnv;

public class TestFileDataGenerator
{
    private static readonly string CurrentPath =
        Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName!;

    public TestFileDataGenerator(string volumeBaseUri)
    {
        VolumeBaseUri = volumeBaseUri;
    }

    public string VolumeBaseUri { get; set; }

    public void GenerateTestFileData()
    {
        GenerateDatasetTestFileData(
            volumeCount: 3,
            imageDatasetCount: 30,
            sampleCount: 50,
            classCodeSetCount: 30,
            classCodeCount: 5,
            imageCount: 3
            );
    }

    public void RemoveTestFileData()
    {
        RemoveDatasetTestFileData(1);
    }

    private void GenerateDatasetTestFileData(int volumeCount = 1, int imageDatasetCount = 1, int sampleCount = 1,
        int classCodeSetCount = 1, int classCodeCount = 3, int imageCount = 3)
    {
        var testImagePath = Path.Combine(CurrentPath, "Mirero.DAQ.Test.Integration.Service.Dataset/seed_data/test_dataset_images");

        var imageDatasetId = 1;
        var classCodeSetId = 1;
        var sampleImageId = 1;
        var classCodeImageId = 1;

        var files = Directory.EnumerateFiles(testImagePath).ToList();

        for (var i = 1; i <= volumeCount; i++)
        {
            var volDirName = Path.Combine(VolumeBaseUri, "volume") + i;
            Directory.CreateDirectory(volDirName);
            for (var j = 0; j < imageDatasetCount; j++)
            {
                var imgDsDirName = Path.Combine(volDirName, "dataset" + imageDatasetId);

                Directory.CreateDirectory(imgDsDirName);
                for (var k = 0; k < sampleCount; k++)
                {
                    for (var l = 0; l < imageCount; l++)
                    {
                        var fileUri = files[i % files.Count];
                        File.Copy(fileUri, Path.Combine(imgDsDirName, "image" + sampleImageId + ".jpg"));
                        sampleImageId++;
                    }
                }
                imageDatasetId++;
            }

            for (var j = 0; j < classCodeSetCount; j++)
            {
                var ccsDirName = Path.Combine(volDirName, "class_code_set" + classCodeSetId);

                Directory.CreateDirectory(ccsDirName);
                for (var k = 0; k < classCodeCount; k++)
                {
                    for (var l = 0; l < imageCount; l++)
                    {
                        var fileUri = files[i % files.Count];
                        File.Copy(fileUri, Path.Combine(ccsDirName, "image" + classCodeImageId + ".jpg"));
                        classCodeImageId++;
                    }
                }
                classCodeSetId++;
            }
        }
    }

    private void RemoveDatasetTestFileData(int volumeCount)
    {
        for (var i = 1; i <= volumeCount; i++)
        {
            var volumeDirName = Path.Combine(VolumeBaseUri, "volume" + i);

            if (Directory.Exists(volumeDirName))
            {
                Directory.Delete(volumeDirName, true);
            }
        }
    }
}