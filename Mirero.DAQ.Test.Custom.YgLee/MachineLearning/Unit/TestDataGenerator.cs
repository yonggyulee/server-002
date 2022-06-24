//using Google.Protobuf;
//using Grpc.Net.Client;
//using Mirero.DAQ.Domain.Common.Protos;
//using Mirero.DAQ.Domain.MachineLearning.Protos.V1;
//using Volume = Mirero.DAQ.Domain.MachineLearning.Protos.V1.Volume;

//namespace Mirero.DAQ.Test.Custom.YgLee.MachineLearning.Unit;

//public class TestDataGenerator
//{
//    private static MachineLearningService.MachineLearningServiceClient? _client;
//    private const string TestDataPath = "D:/workspace/TestData/TestModel_mar";

//    public static void TestDataGenerate()
//    {
//        var channel = GrpcChannel.ForAddress("http://localhost:5002");
//        _client = new MachineLearningService.MachineLearningServiceClient(channel);

//        VolumeDataGenerate(3);
//        ServerDataGenerate(3);
//        InferenceWorkerDataGenerate(4);
//        TrainingWorkerDataGenerate(4);
//        ModelDataGenerate(4);
//        ModelVersionDataGenerate(4);
//    }

//    private static void VolumeDataGenerate(int cnt = 1)
//    {
//        var types = new List<string>() {"classification", "object_detection", "segmentation"};
//        var path = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName;

//        path = Path.Combine(path, "test_data\\machine_learning_file_storage\\ml_volume_");

//        Console.WriteLine(path);

//        for (int i = 1; i <= cnt; i++)
//        {
//            _client.CreateVolume(new Volume
//            {
//                Id = "ml_volume_" + i,
//                Title = "ML_Volume" + i,
//                Type = types[i%3],
//                Uri = path + i,
//                Capacity = 100000000
//            });
//        }
//    }

//    public static void ServerDataGenerate(int cnt)
//    {
//        var oss = new List<List<object?>>()
//        {
//            new List<object?>() {"CentOS", "7", 1, 512000000, null, 0, 0},
//            new List<object?>() {"Ubuntu", "16.04 LTS", 0, 0, "DL360GEN10", 2, 512000000 },
//            new List<object?>() {"Debian", "11", 1, 512000000, "A100", 1, 256000000}
//        };
//        for (int i = 1; i <= cnt; i++)
//        {
//            _client.CreateServer(new Server
//            {
//                Id = "ml_server_" + i,
//                Address = "192.168.100.20" + i,
//                OsType = (string) oss[i%3][0],
//                OsVersion = (string) oss[i%3][1],
//                CpuCount = (int)oss[i % 3][2],
//                CpuMemory = (int)oss[i % 3][3],
//                GpuName = (string)oss[i%3][4],
//                GpuCount = (int)oss[i % 3][5],
//                GpuMemory = (int)oss[i % 3][6]
//            });
//        }
//    }

//    public static void InferenceWorkerDataGenerate(int cnt)
//    {
//        var info = new List<List<object?>>()
//        {
//            new List<object?>() {"tf", 1, 512000000, 1, 256000000},
//            new List<object?>() {"torch", 0, 0, 2, 512000000 },
//            new List<object?>() {"mms", 1, 512000000, 0, 0},
//            new List<object?>() {"trt", 1, 512000000, 1, 256000000}
//        };

//        var response = _client.ListServers(new ListServersRequest
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 100
//            }
//        });
//        var server_list = response.Servers.ToList();
//        var n = 1;
//        foreach (var s in server_list)
//        {
//            for (var j = 0; j < cnt; j++)
//            {
//                _client.CreateInferenceWorker(new CreateInferenceWorkerRequest
//                {
//                    InferenceWorker = new InferenceWorker
//                    {
//                        Id = "ml_inferenceWorker_" + n,
//                        ServingType = (string) info[j % 4][0],
//                        ServerId = s.Id,
//                        CpuCount = (int) info[j % 4][1],
//                        CpuMemory = (int) info[j % 4][2],
//                        GpuCount = (int) info[j % 4][3],
//                        GpuMemory = (int) info[j % 4][4],
//                        // Properties = "{ \"key\" : \"test_properties3\" }",
//                        // Description = "Descriptions",
//                    }
//                });
//                n++;
//            }
//        }
//    }

//    public static void TrainingWorkerDataGenerate(int cnt)
//    {
//        var info = new List<List<object?>>()
//        {
//            new List<object?>() {"classification", 0, 0, 1, 256000000},
//            new List<object?>() {"object_detection", 0, 0, 2, 512000000 },
//            new List<object?>() {"segmentation", 1, 512000000, 0, 0},
//            new List<object?>() {"anomaly", 1, 512000000, 1, 256000000}
//        };

//        var response = _client.ListServers(new ListServersRequest
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 100
//            }
//        });
//        var server_list = response.Servers.ToList();
//        var n = 1;
//        foreach (var s in server_list)
//        {
//            for (var j = 0; j < cnt; j++)
//            {
//                _client.CreateTrainingWorker(new CreateTrainingWorkerRequest
//                {
//                    TrainingWorker = new TrainingWorker
//                    {
//                        Id = "ml_training_worker_" + n,
//                        TrainingType = (string)info[j % 4][0],
//                        ServerId = s.Id,
//                        CpuCount = (int)info[j % 4][1],
//                        CpuMemory = (int)info[j % 4][2],
//                        GpuCount = (int)info[j % 4][3],
//                        GpuMemory = (int)info[j % 4][4],
//                        // Properties = "{ \"key\" : \"test_properties3\" }",
//                        // Description = "Descriptions",
//                    }
//                });
//                n++;
//            }
//        }
//    }

//    public static void ModelDataGenerate(int cnt)
//    {
//        var info = new List<List<object?>>()
//        {
//            new List<object?>() {"classification", "resnet"},
//            new List<object?>() {"object_detection", "fcnn"},
//            new List<object?>() {"segmentation", "deeplabv3"},
//            new List<object?>() {"anomaly", "cnn"}
//        };

//        var response = _client.ListVolumes(new ListVolumesRequest
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 100
//            }
//        });
//        var volumes = response.Volumes.ToList();
//        var n = 1;
//        foreach (var v in volumes)
//        {
//            for (var j = 0; j < cnt; j++)
//            {
//                var createdModel = _client.CreateModel(new Model
//                {
//                    VolumeId = v.Id,
//                    TaskName = (string)info[j % 4][0],
//                    NetworkName = (string)info[j % 4][1],
//                    ModelName = "model" + n,
//                    // Properties = "{ \"key\" : \"test_properties3\" }",
//                    // Description = "Descriptions",
//                });
//                n++;

//                Console.WriteLine(createdModel.Id);
//            }
//        }
//        Console.WriteLine("ModelDataGenerate() Complete.");
//    }

//    public static void ModelVersionDataGenerate(int cnt)
//    {
//        var info = new List<string>()
//        {
//            "fcnn_model_1.mar",
//            "resnet_model_1.mar",
//            "deeplabv3_model_1.mar",
//            "densenet_model_1.mar"
//        };

//        var volumesResponse = _client.ListVolumes(new ListVolumesRequest
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 100
//            }
//        });

//        var modelResponse = _client.ListModels(new ListModelsRequest
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 100
//            }
//        });

//        var volumes = volumesResponse.Volumes.ToList();
//        var models = modelResponse.Models.ToList();

//        var n = 1;
//        foreach (var v in volumes)
//        {
//            var v_models = models.Where(m => m.VolumeId == v.Id);
//            foreach (var m in v_models)
//            {
//                for (var j = 0; j < cnt; j++)
//                {
//                    var uri = Path.Combine(TestDataPath, info[j % 4]);
//                    using var fs = new FileStream(uri, FileMode.Open, FileAccess.Read);

//                    var createdModelVersion = _client.CreateModelVersion(new ModelVersion
//                    {
//                        ModelId = m.Id,
//                        Version = Convert.ToString(j + 1),
//                        Filename = info[j % 4],
//                        //Buffer = ByteString.CopyFrom(buffer),
//                        // Properties = "{ \"key\" : \"test_properties3\" }",
//                        // Description = "Descriptions",
//                    });

//                    //Task.Run(async () => await uploadModelFile(m.Id, fs));
//                    uploadModelFile(createdModelVersion.Id, fs).GetAwaiter().GetResult();

//                    n++;
//                    Console.WriteLine(createdModelVersion.Id);
//                }
//            }
//        }

//        Console.WriteLine("ModelVersionDataGenerate() Complete.");

//        async Task uploadModelFile(long modelId, FileStream fs)
//        {
//            const int chunkSize = 32 * 1024;
//            var offset = 0;

//            var fileLength = fs.Length;
//            var fileChunk = new byte[chunkSize];

//            using var call = _client.UploadModelVersionStream();
//            var length = chunkSize;
//            int n;
//            while ((n = fs.Read(fileChunk, 0, length)) != 0)
//            {
//                await call.RequestStream.WriteAsync(new UploadModelVersionRequest
//                {
//                    ModelVersionId = modelId,
//                    Buffer = ByteString.CopyFrom(fileChunk)
//                });

//                offset += length;

//                length = (int)Math.Min(chunkSize, fileLength - offset);
//            }
            
//            await call.RequestStream.CompleteAsync();

//            var response = await call.ResponseAsync;

//            Console.WriteLine(modelId);
//        }
//    }
//}