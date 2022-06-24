//using System.Net;
//using System.Text;
//using Google.Protobuf;
//using Google.Protobuf.WellKnownTypes;
//using Grpc.Core;
//using Grpc.Net.Client;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Mirero.DAQ.Domain.Common.Data;
//using Mirero.DAQ.Domain.Common.Protos;
//using Mirero.DAQ.Domain.Recipe.Entities;
//using Mirero.DAQ.Domain.Recipe.Protos.V1;
//using Serilog;
//using ILogger = Microsoft.Extensions.Logging.ILogger;
//using QueryParameter = Mirero.DAQ.Domain.Common.Protos.QueryParameter;
//using Recipe = Mirero.DAQ.Domain.Recipe.Protos.V1.Recipe;
//using RecipeVersion = Mirero.DAQ.Domain.Recipe.Protos.V1.RecipeVersion;
//using Server = Mirero.DAQ.Domain.Recipe.Protos.V1.Server;
//using Volume = Mirero.DAQ.Domain.Recipe.Protos.V1.Volume;

//namespace Mirero.DAQ.Test.Custom.Hjlee;

//public class RecipeTest
//{
//    private static RecipeService.RecipeServiceClient _recipeServiceClient;
//    private static Serilog.ILogger _logger = Log.Logger;
 
//    public static void Test()
//    {
//        var channel = GrpcChannel.ForAddress("http://localhost:5000");
//        _recipeServiceClient = new RecipeService.RecipeServiceClient(channel);

//        /*
//         * 1. Volume 생성(TestCreateVolume)
//         * 2. Recipe 생성(TestCreateRecipe)
//         * 3. RecipeVersion 생성(TestCreateRecipeVersion)
//         * 4. RecipeBatchJob 생성(TestCreateRecipeBatchJob)
//         */

//        //TestListVolumes();
//        //TestCreateVolume();
//        //TestUpdateVolume();
//        //TestDeleteVolume();
//        //TestListServers();
//        //TestCreateServer();
//        //TestUpdateServer();
//        //TestDeleteServer();
//        //TestCreateRecipeWorker();
//        //TestStartRecipeWorker();
//        //TestStopRecipeWorker();
//        //TestRemoveRecipeWorker();
//        //TestListRecipes();
//        //TestCreateRecipe();
//        //TestUpdateRecipe();
//        //TestDeleteRecipe();
//        //TestListRecipeVersions();
//        //TestGetRecipeVersions();
//        //TestCreateRecipeVersion();
//        //TestUpdateRecipeVersion();
//        //TestDeleteRecipeVersion();
//        //TestDownloadRecipeVersion();
//        //TestListRecipeBatchJobs();
//        //TestCreateRecipeBatchJob();
//        TestStartRecipeBatchJobStream().Wait();
//        //TestCancelRecipeBatchJob();
//        //TestWaitRecipeBatchJob();
//        //TestDeleteRecipeBatchJob();
//        //TestListRecipeJobs();
//        //TestListRecipeType();
//        //TestListPipelineType();
//    }

//    #region Volume

//    private static void TestListVolumes()
//    {
//        _logger.Information("리스트volume");
//        var listVolumesResponse = _recipeServiceClient.ListVolumes(new ListVolumesRequest
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 3,
//                Where = "",
//            }
//        });

//        _logger.Information(listVolumesResponse.ToString());
//    }

//    private static void TestCreateVolume()
//    {
//        _logger.Information("생성volume");

//        var createVolumesResponse = _recipeServiceClient.CreateVolume(new Volume
//        {
//            Id = "Volume1", Title = "Title_Volume1", Uri = "D:\\dev_mirero\\workspace\\DAQ60\\Server\\daq-server\\Src\\Mirero.DAQ.Test.Custom.Hjlee\\RecipeFileStorage\\Volume1", //Usage = 1 , Capacity = 100000000
//        });
        
//        _logger.Information(createVolumesResponse.ToString());
//    }

//    private static void TestUpdateVolume()
//    {
//        _logger.Information("업데이트volume(디렉토리 위치 변경");

//        var updateVolumesResponse = _recipeServiceClient.UpdateVolume(new Volume
//        {
//            Id = "Volume",
//            Title = "Title_Volume1",
//            Uri = "D:/dev_mirero/workspace/DAQ60/Server/daq-server/Src/Mirero.DAQ.Test.Custom.Hjlee/RecipeFileStorage2/",
//        });

//        _logger.Information(updateVolumesResponse.ToString());
//    }

//    private static void TestDeleteVolume()
//    {
//        _logger.Information("삭제Volume");

//        //var deleteVolumeResponse = 
//        _recipeServiceClient.DeleteVolume(new DeleteVolumeRequest
//        {
//            VolumeId = "Id_Volume4"
//        });

//        //_logger.Information(deleteVolumeResponse.ToString());
//    }

//    #endregion

//    private static void TestListServers()
//    {
//        _logger.Information("리스트Server");
//        var listServersResponse = _recipeServiceClient.ListServers(new ListServersRequest
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 3,
//                Where = "",
//            }
//        });
//        _logger.Information(listServersResponse.ToString());
//    }

//    private static void TestCreateServer()
//    {
//        _logger.Information("생성Server");
//        var createServerResponse = _recipeServiceClient.CreateServer(new Server
//        {
//            Id = "Id_server4",
//            Address = IPAddress.IPv6None.ToString(),
//            OsType = "OsType",
//            OsVersion = "OsVersion",
//            CpuCount = 1,
//            CpuMemory = 1,
//            GpuName = "GpuName",
//            GpuCount = 1,
//            GpuMemory = 1
//        });
//        _logger.Information(createServerResponse.ToString());
//    }

//    private static void TestUpdateServer()
//    {
//        _logger.Information("업데이트Server");
//        var updateServerResponse = _recipeServiceClient.UpdateServer(new Server
//        {
//            Id = "Id_server4",
//            Address = IPAddress.IPv6None.ToString(),
//            OsType = "OsType2",
//            OsVersion = "OsVersion2",
//            CpuCount = 1,
//            CpuMemory = 1,
//            GpuName = "GpuName",
//            GpuCount = 1,
//            GpuMemory = 2
//        });
//        _logger.Information(updateServerResponse.ToString());
//    }

//    private static void TestDeleteServer()
//    {
//        _logger.Information("삭제Server");
//        _recipeServiceClient.DeleteServer(new DeleteServerRequest
//        {
//            ServerId = "Id_server4"
//        });
//    }

//    private static void TestCreateRecipeWorker()
//    {
//        _logger.Information("");
//        _logger.Information("");
//    }

//    private static void TestStartRecipeWorker()
//    {
//        _logger.Information("");
//        _logger.Information("");
//    }

//    private static void TestStopRecipeWorker()
//    {
//        _logger.Information("");
//        _logger.Information("");
//    }

//    private static void TestRemoveRecipeWorker()
//    {
//        _logger.Information("");
//        _logger.Information("");
//    }

//    private static void TestListRecipes()
//    {
//        _logger.Information("리스트Recipe");
//        var listRecipeResponse = _recipeServiceClient.ListRecipes(new ListRecipesRequest
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 3,
//                Where = "",
//            }
//        });
//        _logger.Information(listRecipeResponse.ToString());
//    }

//    private static void TestCreateRecipe()
//    {
//        _logger.Information("생성Recipe");
//        var createRecipeResponse = _recipeServiceClient.CreateRecipe(new Recipe
//        {
//            //VolumeId = "Volume1", Title = "Title_Recipe1"
//            VolumeId = "Volume1", Title = "Title_Recipe2"
//            //VolumeId = "Volume2", Title = "Title_Recipe3"
//            //VolumeId = "Volum4", Title = "Title_Recipe5"
//        });

//        _logger.Information(createRecipeResponse.ToString());
//    }

//    private static void TestUpdateRecipe()
//    {
//        _logger.Information("업데이트Recipe");
//        var updateRecipeResponse = _recipeServiceClient.UpdateRecipe(new Recipe
//        {
//            Id = 6,
//            VolumeId = "Id_Volume2",
//            Title = "Title_Recipe1"
//        });

//        _logger.Information(updateRecipeResponse.ToString());
//    }

//    private static void TestDeleteRecipe()
//    {
//        _logger.Information("삭제Recipe");
//        var deleteRecipeResponse = _recipeServiceClient.DeleteRecipe(new DeleteRecipeRequest
//        {
//            RecipeId = 6
//        });
//    }

//    private static void TestListRecipeVersions()
//    {
//        _logger.Information("리스트RecipeVersion");

//        var listRecipeVersionsResponse = _recipeServiceClient.ListRecipeVersions(new ListRecipeVersionsRequest
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 3,
//                Where = "",
//            }
//        });

//        _logger.Information(listRecipeVersionsResponse.ToString());
//    }

//    private static void TestGetRecipeVersions()
//    {
//        _logger.Information("한개RecipeVersion");

//        var getRecipeVersionsResponse = _recipeServiceClient.GetRecipeVersion(new GetRecipeVersionRequest
//        {
//            RecipeVersionId = 15
//        });
//        _logger.Information(getRecipeVersionsResponse.ToString());
//    }
    
//    private static void TestCreateRecipeVersion()
//    {
//        _logger.Information("생성recipeVersion");

//        var CreateRecipeBatchJobResponse = _recipeServiceClient.CreateRecipeVersion(new RecipeVersion
//        {
//            RecipeId = 2,
//            Version = "v1",
//            Filename = "file_2_v1.txt",
//            //Uri = "Version1",
//            Data = ByteString.CopyFromUtf8("data"), //FromBase64("data"),
//            CreateUserName = "김순영",
//            UpdateUserName = "김순영",
//            CreateDate = Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)),
//            UpdateDate = Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)),
//            //Properties = ,
//            //Description = 
//        });

//        _logger.Information(CreateRecipeBatchJobResponse.ToString());
//    }

//    private static void TestUpdateRecipeVersion()
//    {
//        _logger.Information("업데이트RecipeVersion");

//        var updateRecipeVersionResponse = _recipeServiceClient.UpdateRecipeVersion(
//            new UpdateRecipeVersionRequest
//            {
//                RecipeVersion = new RecipeVersion
//                {
//                    Id = 15,
//                    RecipeId = 12,
//                    Filename = "file_v1.py",
//                    Data = ByteString.CopyFromUtf8("data"), //FromBase64("data"),
//                    UpdateUserName = "김순영2",
//                    UpdateDate = Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)),
//                },
//                UpdateData = true
//            });

//        _logger.Information(updateRecipeVersionResponse.ToString());
//    }

//    private static void TestDeleteRecipeVersion()
//    {
//        _logger.Information("삭제RecipeVersion");
//        _recipeServiceClient.DeleteRecipeVersion(new DeleteRecipeVersionRequest
//        {
//            RecipeVersionId = 14
//        });
        
//        _logger.Information("");
//    }

//    private static void TestDownloadRecipeVersion()
//    {
//        _logger.Information("다운로드RecipeVersion");
//        var downloadRecipeVersionResponse = _recipeServiceClient.DownloadRecipeVersion(new DownloadRecipeVersionRequest
//        {
//            RecipeVersionId = 15
//        });

//        _logger.Information(downloadRecipeVersionResponse.ToString());
//    }

//    private static void TestListRecipeBatchJobs()
//    {
//        _logger.Information("리스트RecipeBatchJob");

//        var listRecipeBatchJobsResponse = _recipeServiceClient.ListRecipeBatchJobs(new ListRecipeBatchJobsRequest
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 3,
//                Where = "",
//            }
//        });
        
//        _logger.Information(listRecipeBatchJobsResponse.ToString());
//    }

//    private static void TestCreateRecipeBatchJob()
//    {
//        _logger.Information("생성RecipeBatchJob");

//        _recipeServiceClient.CreateRecipeBatchJob(new CreateRecipeBatchJobRequest
//        {
//            RecipeVersionId = 10,
//            Type = "Inference",
//            Title = "Batch5"
//        });
//    }
    
//    private static async Task TestStartRecipeBatchJobStream()
//    {
//        _logger.Information("시작RecipeBatchJob");
//        /*while(true)
//        {
//            var count = new Random().Next(1, 10);
//            _logger.Information("Sending count {count}", count);
//            await _recipeServiceClient.StartRecipeBatchJobStream().RequestStream.WriteAsync(new StartRecipeBatchJobStreamRequest() { Count = count });
//        }*/
        
        
//        List<StartRecipeBatchJobStreamRequest> streams = new List<StartRecipeBatchJobStreamRequest> {
//            new StartRecipeBatchJobStreamRequest() {RecipeId = $"test_{DateTime.Now.Ticks}", RecipeBatchJobId = 5, Parameters = "test1"},
//            new StartRecipeBatchJobStreamRequest() {RecipeId = $"test_{DateTime.Now.Ticks}", RecipeBatchJobId = 8, Parameters = "test2"},
//        };

//        using var call = _recipeServiceClient.StartRecipeBatchJobStream();
//        foreach (var stream in streams)
//        {
//            await call.RequestStream.WriteAsync(stream);
//            Console.WriteLine($"StartRecipeBatchJobStream Id: {stream.RecipeId}");
//        }
//        await call.RequestStream.CompleteAsync();
//        var response = await call.ResponseAsync;


//        //_recipeServiceClient.StartRecipeBatchJobStream().RequestStream.CompleteAsync();
//    }

//    private static void TestCancelRecipeBatchJob()
//    {
//        _logger.Information("");
//        _logger.Information("");
//    }

//    private static void TestWaitRecipeBatchJob()
//    {
//        _logger.Information("");
//        _logger.Information("");
//    }

//    private static void TestDeleteRecipeBatchJob()
//    {
//        _logger.Information("삭제RecipeBatchJob");

//        var deleteRecipeBatchJobsResponse = _recipeServiceClient.DeleteRecipeBatchJob(new DeleteRecipeBatchJobRequest()
//        {
//            RecipeBatchJobId = 7
//            //QueryParameter = new QueryParameter
//            //{
//            //    PageIndex = 0,
//            //    PageSize = 3,
//            //    Where = "",
//            //}
//        });
//    }

//    private static void TestListRecipeJobs()
//    {
//        var response = _recipeServiceClient.ListRecipeJobs(new ListRecipeJobsRequest()
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 3,
//                Where = "",
//            }
//        });

//        _logger.Information(response.ToString());
//    }
    
//    private static void TestListRecipeType()
//    {
//        _logger.Information("리스트RecipeType");
        
//        var listRecipeTypeResponse = _recipeServiceClient.ListRecipeType(new ListRecipeTypeRequest 
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 3,
//                Where = "",
//            }
//        });
        
//        _logger.Information("");
//    }
    
//    private static void TestListPipelineType()
//    {
//        _logger.Information("리스트PipelineType");
        
//        var listRecipeBatchJobsResponse = _recipeServiceClient.ListPipelineType(new ListPipelineTypeRequest
//        {
//            QueryParameter = new QueryParameter
//            {
//                PageIndex = 0,
//                PageSize = 3,
//                Where = "",
//            }
//        });
        
//        _logger.Information("");
//    }
//}