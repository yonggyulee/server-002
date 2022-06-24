using Google.Protobuf;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService;

public class TestDataGenerator
{
    private VolumeService.VolumeServiceClient? _volumeServiceClient;
    private ImageDatasetService.ImageDatasetServiceClient? _imageDatasetServiceClient;
    private ClassCodeService.ClassCodeServiceClient? _classCodeServiceClient;
    private GtDatasetService.GtDatasetServiceClient? _gtDatasetServiceClient;

    private readonly string _volumeBaseUri;

    private static readonly string ImagePath =
        Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName!, "TestImages");


    public TestDataGenerator(VolumeService.VolumeServiceClient? volumeServiceClient = null,
        ImageDatasetService.ImageDatasetServiceClient? imageDatasetServiceClient = null,
        ClassCodeService.ClassCodeServiceClient? classCodeServiceClient = null,
        GtDatasetService.GtDatasetServiceClient? gtDatasetServiceClient = null,
        string volumeBaseUri = "")
    {
        _volumeServiceClient = volumeServiceClient;
        _imageDatasetServiceClient = imageDatasetServiceClient;
        _classCodeServiceClient = classCodeServiceClient;
        _gtDatasetServiceClient = gtDatasetServiceClient;
        _volumeBaseUri = volumeBaseUri;
    }

    public void VolumeDataGenerate(int cnt = 1)
    {
        if (_volumeServiceClient == null)
        {
            throw new NullReferenceException($"{nameof(_volumeServiceClient)} is null.");
        }

        var tempDir = $"{_volumeBaseUri}/{Path.GetRandomFileName()}";

        for (int i = 1; i <= cnt; i++)
        {
            var response = _volumeServiceClient.CreateVolume(new CreateVolumeRequest
            {
                Id = "volume" + i,
                Title = "Volume" + i,
                Type = "image",
                Uri = tempDir,
                Capacity = 100000000
            });
            Console.WriteLine(response.Title);
        }
        Console.WriteLine("VolumeDataGenerate() Complete.");
    }

    public void DatasetDataGenerate(int cnt)
    {
        if (_volumeServiceClient == null)
        {
            throw new NullReferenceException($"{nameof(_volumeServiceClient)} is null.");
        }
        if (_imageDatasetServiceClient == null)
        {
            throw new NullReferenceException($"{nameof(_imageDatasetServiceClient)} is null.");
        }

        var response = _volumeServiceClient.ListVolumes(new ListVolumesRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 100
            }
        });

        var volList = response.Volumes.ToList();
        var n = 1;
        foreach (var v in volList)
        {
            for (var j = 0; j < cnt; j++)
            {
                var createdDataset = _imageDatasetServiceClient.CreateImageDataset(new CreateImageDatasetRequest
                {
                    Title = "Title_ImageDataset" + n,
                    DirectoryName = "image_dataset" + n,
                    VolumeId = v.Id,
                    Description = "Test ImageDataset" + n + " Create."
                });
                n++;
                Console.WriteLine(createdDataset.Title);
            }
        }
        Console.WriteLine("DatasetDataGenerate() Complete.");
    }

    public void SampleDataGenerate(int cnt, int imgCount)
    {
        if (_imageDatasetServiceClient == null)
        {
            throw new NullReferenceException($"{nameof(_imageDatasetServiceClient)} is null.");
        }

        var response = _imageDatasetServiceClient.ListImageDatasets(new ListImageDatasetsRequest()
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 100
            }
        });

        var dsList = response.Datasets.ToList();

        Console.WriteLine(dsList.Count);
        var i = 0;
        var files = Directory.EnumerateFiles(ImagePath).ToList();

        foreach (var d in dsList)
        {
            for (var j = 1; j <= cnt; j++)
            {
                var imgList = new List<Image>();
                for (var k = 0; k < imgCount; k++)
                {
                    var fileUri = files[i % files.Count];
                    using var fs = new FileStream(fileUri, FileMode.Open, FileAccess.Read);
                    var buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    imgList.Add(new Image
                    {
                        Filename = "image" + i + ".jpg",
                        Extension = "jpg",
                        ImageCode = "right",
                        Buffer = ByteString.CopyFrom(buffer),
                    });
                    i++;
                }

                var createdSample = _imageDatasetServiceClient.CreateSample(new CreateSampleRequest
                {
                    Sample = new Sample
                    {
                        Id = j,
                        DatasetId = d.Id,
                        Images = { imgList }
                    },
                    LockTimeoutSec = 1,
                });

                Console.WriteLine($"SampleId : ({createdSample.Id}, {createdSample.DatasetId})");
            }
        }
        Console.WriteLine("SampleDataGenerate() Complete.");
    }

    public void ImageDataGenerate(int cnt)
    {
        if (_imageDatasetServiceClient == null)
        {
            throw new NullReferenceException($"{nameof(_imageDatasetServiceClient)} is null.");
        }

        var response = _imageDatasetServiceClient.ListImageDatasets(new ListImageDatasetsRequest()
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 100
            }
        });
        var ds_list = response.Datasets.ToList();
        Console.WriteLine(ds_list.Count);
        foreach (var d in ds_list)
        {
            for (var j = 1; j <= cnt; j++)
            {
                var createSample = _imageDatasetServiceClient.CreateSample(new CreateSampleRequest
                {
                    Sample = new Sample
                    {
                        Id = j,
                        DatasetId = d.Id
                    },
                    LockTimeoutSec = 1,
                });

                Console.WriteLine(createSample.Id);
            }
        }
        Console.WriteLine("ImageDataGenerate() Complete.");
    }

    public void ClassCodeSetGenerate(int cnt)
    {
        if (_volumeServiceClient == null)
        {
            throw new NullReferenceException($"{nameof(_volumeServiceClient)} is null.");
        }
        if (_classCodeServiceClient == null)
        {
            throw new NullReferenceException($"{nameof(_classCodeServiceClient)} is null.");
        }

        var tasks = new List<string> { "Classification", "ObjectDetection", "Segmentation" };

        var response = _volumeServiceClient.ListVolumes(new ListVolumesRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 100
            }
        });
        var vol_list = response.Volumes.ToList();
        var n = 1;
        foreach (var v in vol_list)
        {
            for (var j = 0; j < cnt; j++)
            {
                var createdClassCodeSet = _classCodeServiceClient.CreateClassCodeSet(new CreateClassCodeSetRequest
                {
                    Title = "Title_ClassCodeSet" + n,
                    DirectoryName = "class_code_set" + n,
                    Task = tasks[j % tasks.Count],
                    VolumeId = v.Id,
                    Description = "Test ClassCodeSet" + n + "Create."
                });
                n++;

                Console.WriteLine(createdClassCodeSet.Title);
            }
        }
        Console.WriteLine("ClassCodeSetGenerate() Complete.");
    }

    public void ClassCodeDataGenerate(int cnt, int imgCount)
    {
        if (_classCodeServiceClient == null)
        {
            throw new NullReferenceException($"{nameof(_classCodeServiceClient)} is null.");
        }

        var response = _classCodeServiceClient.ListClassCodeSets(new ListClassCodeSetsRequest()
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 100
            }
        });

        var cs_list = response.ClassCodeSets.ToList();

        Console.WriteLine(cs_list.Count);
        int i = 0;
        var files = Directory.EnumerateFiles(ImagePath).ToList();

        foreach (var cs in cs_list)
        {
            for (var j = 1; j <= cnt; j++)
            {
                var imgList = new List<ClassCodeReferenceImage>();
                for (int k = 0; k < imgCount; k++)
                {
                    var fileUri = files[i % files.Count];
                    using var fs = new FileStream(fileUri, FileMode.Open, FileAccess.Read);
                    var buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    imgList.Add(new ClassCodeReferenceImage
                    {
                        ClassCodeSetId = cs.Id,
                        Filename = "image" + i + ".jpg",
                        Extension = "jpg",
                        Buffer = ByteString.CopyFrom(buffer),
                    });
                    i++;
                }

                var createdClassCode = _classCodeServiceClient.CreateClassCode(new CreateClassCodeRequest
                {
                    Name = "code" + j,
                    Code = j,
                    ClassCodeSetId = cs.Id,
                    ClassCodeReferenceImages = { imgList },
                    LockTimeoutSec = 2,
                });

                Console.WriteLine(createdClassCode.Name);
            }
        }
        Console.WriteLine("ClassCodeDataGenerate() Complete.");
    }

    public void GtDatasetGenerate(int itv = 3)
    {
        if (_volumeServiceClient == null)
        {
            throw new NullReferenceException($"{nameof(_volumeServiceClient)} is null.");
        }
        if (_imageDatasetServiceClient == null)
        {
            throw new NullReferenceException($"{nameof(_imageDatasetServiceClient)} is null.");
        }
        if (_classCodeServiceClient == null)
        {
            throw new NullReferenceException($"{nameof(_classCodeServiceClient)} is null.");
        }
        if (_gtDatasetServiceClient == null)
        {
            throw new NullReferenceException($"{nameof(_gtDatasetServiceClient)} is null.");
        }

        var volumes = _volumeServiceClient.ListVolumes(new ListVolumesRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 100
            }
        }).Volumes.ToList();

        var datasets = _imageDatasetServiceClient.ListImageDatasets(new ListImageDatasetsRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 100
            },
            LockTimeoutSec = 1,
        }).Datasets.ToList();

        var classCodeSets = _classCodeServiceClient.ListClassCodeSets(new ListClassCodeSetsRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 100
            }
        }).ClassCodeSets.ToList();

        var csList = volumes.Select(v => classCodeSets.Where(cs => cs.VolumeId == v.Id).ToList()).ToList();

        var n = 0;
        foreach (var d in datasets)
        {
            var dCount = datasets.Where(ds => ds.VolumeId == d.VolumeId).ToList().Count;
            if ((n % csList[n / dCount].Count) % itv == 0)
            {
                var dirName =
                    Path.Combine(d.DirectoryName + "_classification_gt_dataset_" + (n / itv + 1));

                var createdClassificationGtDataset = _gtDatasetServiceClient.CreateClassificationGtDataset(new CreateClassificationGtDatasetRequest
                {
                    Title = d.Title + "_ClassificationGtDataset_" + (n + 1),
                    DirectoryName = dirName,
                    VolumeId = d.VolumeId,
                    ImageDatasetId = d.Id,
                    ClassCodeSetId = classCodeSets[n % classCodeSets.Count].Id
                });

                Console.WriteLine(createdClassificationGtDataset.Title);
            }

            if ((n % csList[n / dCount].Count) % itv == 1)
            {
                var dir_name =
                    Path.Combine(d.DirectoryName + "_object_detection_gt_dataset_" + (n / itv + 1));

                var createdObjectDetectionGtDataset = _gtDatasetServiceClient.CreateObjectDetectionGtDataset(new CreateObjectDetectionGtDatasetRequest
                {
                    Title = d.Title + "_ObjectDetectionGtDataset_" + (n + 1),
                    DirectoryName = dir_name,
                    VolumeId = d.VolumeId,
                    ImageDatasetId = d.Id,
                    ClassCodeSetId = classCodeSets[n % classCodeSets.Count].Id
                });

                Console.WriteLine(createdObjectDetectionGtDataset.Title);
            }

            if ((n % csList[n / dCount].Count) % itv == 2)
            {
                var dir_name =
                    Path.Combine(d.DirectoryName + "_segmentation_gt_dataset_" + (n / itv + 1));

                var createdSegmentationGtDataset = _gtDatasetServiceClient.CreateSegmentationGtDataset(new CreateSegmentationGtDatasetRequest
                {
                    Title = d.Title + "_SegmentationGtDataset_" + (n + 1),
                    DirectoryName = dir_name,
                    VolumeId = d.VolumeId,
                    ImageDatasetId = d.Id,
                    ClassCodeSetId = classCodeSets[n % classCodeSets.Count].Id
                });

                Console.WriteLine(createdSegmentationGtDataset.Title);
            }
            n++;
        }

        Console.WriteLine("GtDatasetGenerate() Complete.");
    }

    //public void GtDataGenerate(int itv = 3)
    //{
    //    if (_imageDatasetServiceClient == null)
    //    {
    //        throw new NullReferenceException($"{nameof(_imageDatasetServiceClient)} is null.");
    //    }
    //    if (_classCodeServiceClient == null)
    //    {
    //        throw new NullReferenceException($"{nameof(_classCodeServiceClient)} is null.");
    //    }
    //    if (_gtDatasetServiceClient == null)
    //    {
    //        throw new NullReferenceException($"{nameof(_gtDatasetServiceClient)} is null.");
    //    }

    //    var datasets = _gtDatasetServiceClient.ListGtDatasets(new ListGtDatasetsRequest
    //    {
    //        QueryParameter = new QueryParameter
    //        {
    //            PageIndex = 0,
    //            PageSize = 10000
    //        }
    //    }).GtDatasets.ToList();

    //    var classCodeSets = _classCodeServiceClient.ListClassCodeSets(new ListClassCodeSetsRequest
    //    {
    //        QueryParameter = new QueryParameter
    //        {
    //            PageIndex = 0,
    //            PageSize = 10000
    //        }
    //    }).ClassCodeSets.ToList();

    //    var samples = _imageDatasetServiceClient.ListSamples(new ListSamplesRequest
    //    {
    //        QueryParameter = new QueryParameter
    //        {
    //            PageIndex = 0,
    //            PageSize = 10000
    //        },
    //        LockTimeoutSec = 1,
    //    }).Samples.ToList();

    //    var classCodes = _classCodeServiceClient.ListClassCodes(new ListClassCodesRequest
    //    {
    //        QueryParameter = new QueryParameter
    //        {
    //            PageIndex = 0,
    //            PageSize = 10000
    //        },
    //        LockTimeoutSec = 1,
    //    }).ClassCodes.ToList();

    //    var odPath = Path.Combine(TestDataPath, "ObjectDetection");
    //    var sgPath = Path.Combine(TestDataPath, "Segmentation");

    //    int n = 0;
    //    foreach (var d in datasets)
    //    {
    //        var ds_cdSet = classCodeSets.SingleOrDefault(
    //            c => c.Id == d.ClassCodeSetId);

    //        switch (ds_cdSet.Task)
    //        {
    //            case "Classification":
    //                {
    //                    var cSamples =
    //                        samples.Where(s => s.DatasetId == d.ImageDatasetId);
    //                    var clsCodes =
    //                        classCodes.Where(c => c.ClassCodeSetId == ds_cdSet.Id).ToList();
    //                    var gt_num = 0;
    //                    foreach (var s in cSamples)
    //                    {
    //                        foreach (var img in s.Images)
    //                        {
    //                            var createdClassificationGt = _gtDatasetServiceClient.CreateClassificationGt(new CreateClassificationGtRequest
    //                            {
    //                                DatasetId = d.Id,
    //                                ImageId = img.Id,
    //                                ClassCodeId = clsCodes[gt_num % clsCodes.Count].Id
    //                            });
    //                            gt_num++;

    //                            Console.WriteLine(createdClassificationGt.ClassCodeName);
    //                        }
    //                    }

    //                    break;
    //                }
    //            case "ObjectDetection":
    //                {
    //                    var files = Directory.EnumerateFiles(odPath).ToList();
    //                    var cSamples =
    //                        samples.Where(s => s.DatasetId == d.ImageDatasetId);
    //                    var gt_num = 0;
    //                    foreach (var s in cSamples)
    //                    {
    //                        foreach (var img in s.Images)
    //                        {
    //                            using var fs = new FileStream(files[gt_num % files.Count], FileMode.Open, FileAccess.Read);
    //                            var buffer = new byte[fs.Length];
    //                            fs.Read(buffer, 0, buffer.Length);
    //                            var createdObjectDetectionGt = _gtDatasetServiceClient.CreateObjectDetectionGt(new CreateObjectDetectionGtRequest
    //                            {
    //                                Filename = img.Filename.Replace(".jpg", "") + "_object_detection_gt_" + gt_num + ".xml",
    //                                Extension = "xml",
    //                                Buffer = ByteString.CopyFrom(buffer),
    //                                DatasetId = d.Id,
    //                                ImageId = img.Id,
    //                                LockTimeoutSec = 1,
    //                            });
    //                            gt_num++;
    //                            Console.WriteLine(createdObjectDetectionGt.Filename);
    //                        }
    //                    }

    //                    break;
    //                }
    //            case "Segmentation":
    //                {
    //                    var files = Directory.EnumerateFiles(sgPath).ToList();
    //                    var cSamples =
    //                        samples.Where(s => s.DatasetId == d.ImageDatasetId);
    //                    var gt_num = 0;
    //                    foreach (var s in cSamples)
    //                    {
    //                        foreach (var img in s.Images)
    //                        {
    //                            using var fs = new FileStream(files[gt_num % files.Count], FileMode.Open, FileAccess.Read);
    //                            var buffer = new byte[fs.Length];
    //                            fs.Read(buffer, 0, buffer.Length);
    //                            var createdSegmentationGt = _gtDatasetServiceClient.CreateSegmentationGt(new CreateSegmentationGtRequest
    //                            {
    //                                Filename = img.Filename.Replace(".jpg", "") + "_segmentation_gt_" + gt_num + ".png",
    //                                Extension = "jpg",
    //                                Buffer = ByteString.CopyFrom(buffer),
    //                                DatasetId = d.Id,
    //                                ImageId = img.Id,
    //                                LockTimeoutSec = 1,
    //                            });
    //                            gt_num++;
    //                            Console.WriteLine(createdSegmentationGt.Filename);
    //                        }
    //                    }

    //                    break;
    //                }
    //        }

    //        n++;
    //    }

    //    Console.WriteLine("GtDataGenerate() Complete.");
    //}
}