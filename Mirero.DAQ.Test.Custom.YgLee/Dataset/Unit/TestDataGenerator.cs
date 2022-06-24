// using Google.Protobuf;
// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class TestDataGenerator
// {
//     private static ImageDatasetService.ImageDatasetServiceClient _datasetClient;
//     private static ClassCodeService.ClassCodeServiceClient _classCodeClient;
//     private static VolumeService.VolumeServiceClient _volumeClient;
//     private static GtDatasetService.GtDatasetServiceClient _gtClient;
//     
//     private static readonly string DataPath = Path.Combine(
//         Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName!,
//     "test_data\\dataset_file_storage");
//
//     private static readonly string TestDataPath = DatasetClientTest.TestDataPath;
//     private static readonly string ImagePath = Path.Combine(TestDataPath, "TestImages");
//
//     public static void TestDataGenerate()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _datasetClient = new ImageDatasetService.ImageDatasetServiceClient(channel);
//         _classCodeClient = new ClassCodeService.ClassCodeServiceClient(channel);
//         _volumeClient = new VolumeService.VolumeServiceClient(channel);
//         _gtClient = new GtDatasetService.GtDatasetServiceClient(channel);
//         
//         Console.WriteLine("DataGenerate Start.");
//
//         VolumeDataGenerate(3);
//
//         DatasetDataGenerate(15);
//         SampleDataGenerate(50, 3);
//
//         ClassCodeSetGenerate(15);
//         ClassCodeDataGenerate(7, 3);
//
//         GtDatasetGenerate(3);
//         GtDataGenerate(3);
//
//         Console.WriteLine("DataGenerate End.");
//     }
//
//     private static void VolumeDataGenerate(int cnt = 1, int startNum = 0)
//     {
//         for (int i = startNum; i <= cnt; i++)
//         {
//             var response = _volumeClient.CreateVolume(new CreateVolumeRequest
//             {
//                 Id = "volume" + i,
//                 Title = "Volume" + i,
//                 Type = "image",
//                 Uri = Path.Combine(DataPath, "volume") + i,
//                 Capacity = 100000000
//             });
//             Console.WriteLine(response.Title);
//         }
//         Console.WriteLine("VolumeDataGenerate() Complete.");
//     }
//
//     public static void DatasetDataGenerate(int cnt, int startNum = 0)
//     {
//         var response = _volumeClient.ListVolumes(new ListVolumesRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 100
//             }
//         });
//         var vol_list = response.Volumes.ToList();
//         var n = startNum;
//         foreach (var v in vol_list)
//         {
//             for (var j = 0; j < cnt; j++)
//             {
//                 var createdDataset = _datasetClient.CreateImageDataset(new CreateImageDatasetRequest
//                 {
//                     Title = "Dataset" + n,
//                     DirectoryName = "dataset" + n,
//                     // Properties = "{ \"key\" : \"test_properties3\" }",
//                     // Description = "Descriptions",
//                     CreateUser = "이용규",
//                     VolumeId = v.Id
//                 });
//                 n++;
//                 Console.WriteLine(createdDataset.Title);
//             }
//         }
//         Console.WriteLine("DatasetDataGenerate() Complete.");
//     }
//
//     private static void SampleDataGenerate(int cnt, int imgCount, int startNum = 0)
//     {
//         var response = _datasetClient.ListImageDatasets(new ListImageDatasetsRequest()
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 100
//             }
//         });
//         
//         var ds_list = response.Datasets.ToList();
//         
//         Console.WriteLine(ds_list.Count);
//         int i = startNum;
//         var files = Directory.EnumerateFiles(ImagePath).ToList();
//         
//         foreach (var d in ds_list)
//         {
//             for (var j = 1; j <= cnt; j++)
//             {
//                 var imgList = new List<Image>();
//                 for (int k = 0; k < imgCount; k++)
//                 {
//                     var fileUri = files[i % files.Count];
//                     using var fs = new FileStream(fileUri, FileMode.Open, FileAccess.Read);
//                     var buffer = new byte[fs.Length];
//                     fs.Read(buffer, 0, buffer.Length);
//                     imgList.Add(new Image
//                     {
//                         // SampleId = j,
//                         // DatasetId = d.Id,
//                         Filename = "image" + i + ".jpg",
//                         Extension = "jpg",
//                         ImageCode = "right",
//                         // Uri = d.Uri + "\\image" + i + ".jpg",
//                         Buffer = ByteString.CopyFrom(buffer),
//                     });
//                     i++;
//                 }
//                 
//                 var createdSample = _datasetClient.CreateSample(new CreateSampleRequest
//                 {
//                     Sample = new Sample
//                     {
//                         Id = j,
//                         DatasetId = d.Id,
//                         Images = { imgList }
//                     },
//                     LockTimeoutSec = 1,
//                 });
//
//                 Console.WriteLine($"SampleId : ({createdSample.Id}, {createdSample.DatasetId})");
//             }
//         }
//         Console.WriteLine("SampleDataGenerate() Complete.");
//     }
//     
//     private static void ImageDataGenerate(int cnt)
//     {
//         var response = _datasetClient.ListImageDatasets(new ListImageDatasetsRequest()
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 100
//             }
//         });
//         var ds_list = response.Datasets.ToList();
//         Console.WriteLine(ds_list.Count);
//         foreach (var d in ds_list)
//         {
//             for (var j = 1; j <= cnt; j++)
//             {
//                 var createSample = _datasetClient.CreateSample(new CreateSampleRequest
//                 {
//                     Sample = new Sample
//                     {
//                         Id = j,
//                         DatasetId = d.Id
//                     },
//                     LockTimeoutSec = 1,
//                 });
//
//                 Console.WriteLine(createSample.Id);
//             }
//         }
//         Console.WriteLine("ImageDataGenerate() Complete.");
//     }
//
//     private static void ClassCodeSetGenerate(int cnt, int startNum = 0)
//     {
//         var tasks = new List<string>{"Classification", "ObjectDetection", "Segmentation"};
//         
//         var response = _volumeClient.ListVolumes(new ListVolumesRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 100
//             }
//         });
//         var vol_list = response.Volumes.ToList();
//         var n = startNum;
//         foreach (var v in vol_list)
//         {
//             for (var j = 0; j < cnt; j++)
//             {
//                 var createdClassCodeSet = _classCodeClient.CreateClassCodeSet(new CreateClassCodeSetRequest
//                 {
//                     Title = "ClassCodeSet" + n,
//                     DirectoryName = "class_code_set" + n,
//                     Task = tasks[j%tasks.Count],
//                     // Properties = "{ \"key\" : \"test_properties3\" }",
//                     // Description = "Descriptions",
//                     CreateUser = "이용규",
//                     VolumeId = v.Id
//                 });
//                 n++;
//
//                 Console.WriteLine(createdClassCodeSet.Title);
//             }
//         }
//         Console.WriteLine("ClassCodeSetGenerate() Complete.");
//     }
//
//     private static void ClassCodeDataGenerate(int cnt, int imgCount, int startNum = 0)
//     {
//         var response = _classCodeClient.ListClassCodeSets(new ListClassCodeSetsRequest()
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 100
//             }
//         });
//         
//         var cs_list = response.ClassCodeSets.ToList();
//         
//         Console.WriteLine(cs_list.Count);
//         int i = startNum;
//         var files = Directory.EnumerateFiles(ImagePath).ToList();
//         
//         foreach (var cs in cs_list)
//         {
//             for (var j = 1; j <= cnt; j++)
//             {
//                 var imgList = new List<ClassCodeReferenceImage>();
//                 for (int k = 0; k < imgCount; k++)
//                 {
//                     var fileUri = files[i % files.Count];
//                     using var fs = new FileStream(fileUri, FileMode.Open, FileAccess.Read);
//                     var buffer = new byte[fs.Length];
//                     fs.Read(buffer, 0, buffer.Length);
//                     imgList.Add(new ClassCodeReferenceImage
//                     {
//                         // SampleId = j,
//                         ClassCodeSetId = cs.Id,
//                         Filename = "image" + i + ".jpg",
//                         Extension = "jpg",
//                         // Uri = d.Uri + "\\image" + i + ".jpg",
//                         Buffer = ByteString.CopyFrom(buffer),
//                     });
//                     i++;
//                 }
//                 
//                 var createdClassCode = _classCodeClient.CreateClassCode(new CreateClassCodeRequest
//                 {
//                     Name = "code" + j,
//                     Code = j,
//                     CreateUser = "이용규",
//                     ClassCodeSetId = cs.Id,
//                     ClassCodeReferenceImages = { imgList },
//                     LockTimeoutSec = 2,
//                 });
//
//                 Console.WriteLine(createdClassCode.Name);
//             }
//         }
//         Console.WriteLine("ClassCodeDataGenerate() Complete.");
//     }
//
//     private static void GtDatasetGenerate(int itv = 3)
//     {
//         var volumes = _volumeClient.ListVolumes(new ListVolumesRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 100
//             }
//         }).Volumes.ToList();
//
//         var datasets = _datasetClient.ListImageDatasets(new ListImageDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 100
//             },
//             LockTimeoutSec = 1,
//         }).Datasets.ToList();
//
//         var classCodeSets = _classCodeClient.ListClassCodeSets(new ListClassCodeSetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 100
//             }
//         }).ClassCodeSets.ToList();
//
//         var csList = volumes.Select(v => classCodeSets.Where(cs => cs.VolumeId == v.Id).ToList()).ToList();
//
//         var n = 0;
//         foreach (var d in datasets)
//         {
//             var dCount = datasets.Where(ds => ds.VolumeId == d.VolumeId).ToList().Count;
//             if ((n % csList[n/dCount].Count) % itv == 0)
//             {
//                 var dir_name =
//                     Path.Combine(d.DirectoryName + "_classification_gt_dataset_" + (n/itv + 1));
//                 
//                 var createdClassificationGtDataset = _gtClient.CreateClassificationGtDataset(new CreateClassificationGtDatasetRequest
//                 {
//                     Title = d.Title + "_ClassificationGtDataset_" + (n + 1),
//                     DirectoryName = dir_name,
//                     CreateUser = "이용규",
//                     VolumeId = d.VolumeId,
//                     ImageDatasetId = d.Id,
//                     ClassCodeSetId = classCodeSets[n % classCodeSets.Count].Id
//                 });
//
//                 Console.WriteLine(createdClassificationGtDataset.Title);
//             }
//             
//             if ((n % csList[n / dCount].Count) % itv == 1)
//             {
//                 var dir_name =
//                     Path.Combine(d.DirectoryName + "_object_detection_gt_dataset_" + (n/itv + 1));
//                 
//                 var createdObjectDetectionGtDataset = _gtClient.CreateObjectDetectionGtDataset(new CreateObjectDetectionGtDatasetRequest
//                 {
//                     Title = d.Title + "_ObjectDetectionGtDataset_" + (n + 1),
//                     DirectoryName = dir_name,
//                     CreateUser = "이용규",
//                     VolumeId = d.VolumeId,
//                     ImageDatasetId = d.Id,
//                     ClassCodeSetId = classCodeSets[n % classCodeSets.Count].Id
//                 });
//
//                 Console.WriteLine(createdObjectDetectionGtDataset.Title);
//             }
//
//             if ((n % csList[n / dCount].Count) % itv == 2)
//             {
//                 var dir_name =
//                     Path.Combine(d.DirectoryName + "_segmentation_gt_dataset_" + (n/itv + 1));
//                 
//                 var createdSegmentationGtDataset = _gtClient.CreateSegmentationGtDataset(new CreateSegmentationGtDatasetRequest
//                 {
//                     Title = d.Title + "_SegmentationGtDataset_" + (n + 1),
//                     DirectoryName = dir_name,
//                     CreateUser = "이용규",
//                     VolumeId = d.VolumeId,
//                     ImageDatasetId = d.Id,
//                     ClassCodeSetId = classCodeSets[n % classCodeSets.Count].Id
//                 });
//
//                 Console.WriteLine(createdSegmentationGtDataset.Title);
//             }
//             n++;
//         }
//
//         Console.WriteLine("GtDatasetGenerate() Complete.");
//     }
//
//     private static void GtDataGenerate(int itv = 3)
//     {
//         var datasets = _gtClient.ListGtDatasets(new ListGtDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10000
//             }
//         }).GtDatasets.ToList();
//
//         var classCodeSets = _classCodeClient.ListClassCodeSets(new ListClassCodeSetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10000
//             }
//         }).ClassCodeSets.ToList();
//
//         var samples = _datasetClient.ListSamples(new ListSamplesRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10000
//             },
//             LockTimeoutSec = 1,
//         }).Samples.ToList();
//
//         var classCodes = _classCodeClient.ListClassCodes(new ListClassCodesRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10000
//             },
//             LockTimeoutSec = 1,
//         }).ClassCodes.ToList();
//
//         var odPath = Path.Combine(TestDataPath, "ObjectDetection");
//         var sgPath = Path.Combine(TestDataPath, "Segmentation");
//
//         int n = 0;
//         foreach (var d in datasets)
//         {
//             var ds_cdSet = classCodeSets.SingleOrDefault(
//                 c => c.Id == d.ClassCodeSetId);
//             
//             switch (ds_cdSet.Task)
//             {
//                 case "Classification":
//                 {
//                     var cSamples = 
//                         samples.Where(s => s.DatasetId == d.ImageDatasetId);
//                     var clsCodes = 
//                         classCodes.Where(c => c.ClassCodeSetId == ds_cdSet.Id).ToList();
//                     var gt_num = 0;
//                     foreach (var s in cSamples)
//                     {
//                         foreach (var img in s.Images)
//                         {
//                             var createdClassificationGt = _gtClient.CreateClassificationGt(new CreateClassificationGtRequest
//                             {
//                                 DatasetId = d.Id,
//                                 ImageId = img.Id,
//                                 ClassCodeId = clsCodes[gt_num%clsCodes.Count].Id
//                             });
//                             gt_num++;
//
//                             Console.WriteLine(createdClassificationGt.ClassCodeName);
//                         }
//                     }
//
//                     break;
//                 }
//                 case "ObjectDetection":
//                 {
//                     var files = Directory.EnumerateFiles(odPath).ToList();
//                     var cSamples = 
//                         samples.Where(s => s.DatasetId == d.ImageDatasetId);
//                     var gt_num = 0;
//                     foreach (var s in cSamples)
//                     {
//                         foreach (var img in s.Images)
//                         {
//                             using var fs = new FileStream(files[gt_num%files.Count], FileMode.Open, FileAccess.Read);
//                             var buffer = new byte[fs.Length];
//                             fs.Read(buffer, 0, buffer.Length);
//                             var createdObjectDetectionGt = _gtClient.CreateObjectDetectionGt(new CreateObjectDetectionGtRequest
//                             {
//                                 Filename = img.Filename.Replace(".jpg", "") + "_object_detection_gt_" + gt_num + ".xml",
//                                 Extension = "xml",
//                                 Buffer = ByteString.CopyFrom(buffer),
//                                 DatasetId = d.Id,
//                                 ImageId = img.Id,
//                                 LockTimeoutSec = 1,
//                             });
//                             gt_num++;
//                             Console.WriteLine(createdObjectDetectionGt.Filename);
//                         }
//                     }
//
//                     break;
//                 }
//                 case "Segmentation":
//                 {
//                     var files = Directory.EnumerateFiles(sgPath).ToList();
//                     var cSamples = 
//                         samples.Where(s => s.DatasetId == d.ImageDatasetId);
//                     var gt_num = 0;
//                     foreach (var s in cSamples)
//                     {
//                         foreach (var img in s.Images)
//                         {
//                             using var fs = new FileStream(files[gt_num%files.Count], FileMode.Open, FileAccess.Read);
//                             var buffer = new byte[fs.Length];
//                             fs.Read(buffer, 0, buffer.Length);
//                             var createdSegmentationGt = _gtClient.CreateSegmentationGt(new CreateSegmentationGtRequest
//                             {
//                                 Filename = img.Filename.Replace(".jpg", "") + "_segmentation_gt_" + gt_num + ".png",
//                                 Extension = "jpg",
//                                 Buffer = ByteString.CopyFrom(buffer),
//                                 DatasetId = d.Id,
//                                 ImageId = img.Id,
//                                 LockTimeoutSec = 1,
//                             });
//                             gt_num++;
//                             Console.WriteLine(createdSegmentationGt.Filename);
//                         }
//                     }
//
//                     break;
//                 }
//             }
//
//             n++;
//         }
//
//         Console.WriteLine("GtDataGenerate() Complete.");
//     }
// }