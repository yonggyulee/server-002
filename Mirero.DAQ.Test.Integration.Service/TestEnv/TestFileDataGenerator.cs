using System;
using System.IO;
using System.Linq;

namespace Mirero.DAQ.Test.Integration.Service.TestEnv;

public class TestFileDataGenerator
{
    private static readonly string? CurrentProjectName =
        Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Name;
    private static readonly string? CurrentPath =
        Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;

    public TestFileDataGenerator(string volumeBaseUri)
    {
        VolumeBaseUri = volumeBaseUri;
    }

    public string VolumeBaseUri { get; set; }

    public string? GetCurrentPath()
    {
        return CurrentPath;
    }

    public void GenerateTestFileData()
    {
        RemoveTestFileData();
        switch (CurrentProjectName)
        {
            case "Mirero.DAQ.Test.Integration.Service.Dataset":
                GenerateDatasetTestFileData(
                    volumeCount: 3,
                    imageDatasetCount: 30,
                    sampleCount: 50,
                    classCodeSetCount: 30,
                    classCodeCount: 5,
                    imageCount: 3
                );
                break;
            case "Mirero.DAQ.Test.Integration.Service.Workflow":
                GenerateWorkflowTestFileData(volumeCount: 2, workflowVersionCount: 2);
                break;
            case "Mirero.DAQ.Test.Integration.Service.Inference":
                GenerateInferenceTestFileData(
                    volumeCount: 3,
                    modelCount: 3,
                    modelVersionCount: 2
                    );
                break;
            default:
                return;
        }
    }

    public void RemoveTestFileData()
    {
        RemoveDatasetTestFileData(3);
        RemoveWorkflowTestFileData(2);
        RemoveInferenceTestFileData();
    }

    private void GenerateInferenceTestFileData(int volumeCount = 1, int modelCount = 1, int modelVersionCount = 1)
    {
        if (CurrentPath == null) return;

        var testImagePath = Path.Combine(CurrentPath, "seed_data/test_model_archive_files");

        var modelId = 1;
        var modelVersionId = 1;

        var files = Directory.EnumerateFiles(testImagePath).ToList();

        for (var i = 1; i <= volumeCount; i++)
        {
            var volDirName = Path.Combine(VolumeBaseUri, "inference", "volume") + i;
            Directory.CreateDirectory(volDirName);
            for (var j = 0; j < modelCount; j++)
            {
                var modelDirName = Path.Combine(volDirName, "model_" + modelId);

                Directory.CreateDirectory(modelDirName);
                for (var k = 0; k < modelVersionCount; k++)
                {
                    var modelVersionDir = Path.Combine(modelDirName, (k + 1).ToString());
                    Directory.CreateDirectory(modelVersionDir);

                    var fileUri = files[modelVersionId % files.Count];
                    File.Copy(fileUri, Path.Combine(modelVersionDir, "model_" + (k + 1) + ".mar"));
                    modelVersionId++;
                }
                modelId++;
            }
        }
    }

    private void GenerateWorkflowTestFileData(int volumeCount = 1, int workflowVersionCount = 1)
    {
        if (CurrentPath == null) return;
        
        var testImagePath = Path.Combine(CurrentPath, "seed_data");
        var workflowVersionId = 1;
        
        var files = Directory.EnumerateFiles(testImagePath).ToList();
        for (var i = 1; i <= volumeCount; i++)
        {
            var volDirName = Path.Combine(VolumeBaseUri, "volume") + i;
            Directory.CreateDirectory(volDirName);
            for (var j = 0; j < workflowVersionCount; j++)
            {
                var fileSaveDir = Path.Combine(volDirName, "1", workflowVersionId.ToString());
                Directory.CreateDirectory(fileSaveDir);
                
                var fileUri = files[DateTime.Now.Millisecond % files.Count];
                File.Copy(fileUri, Path.Combine(fileSaveDir, $"test{workflowVersionId}.py"));
                
                workflowVersionId++;
            }
        }
    }
    
    private void GenerateDatasetTestFileData(int volumeCount = 1, int imageDatasetCount = 1, int sampleCount = 1,
        int classCodeSetCount = 1, int classCodeCount = 3, int imageCount = 3)
    {
        if (CurrentPath == null) return;

        var testImagePath = Path.Combine(CurrentPath, "seed_data/test_dataset_images");

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
    
    private void RemoveWorkflowTestFileData(int volumeCount)
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

    private void RemoveInferenceTestFileData()
    {
        var volumeDirName = Path.Combine(VolumeBaseUri, "inference");

        if (Directory.Exists(volumeDirName))
        {
            Directory.Delete(volumeDirName, true);
        }
    }
}