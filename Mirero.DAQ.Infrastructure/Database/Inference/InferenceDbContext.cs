using System.Net;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Inference.Entities;
using Mirero.DAQ.Infrastructure.Database.Inference.EntityConfigurations;

namespace Mirero.DAQ.Infrastructure.Database.Inference;

public class InferenceDbContext : DbContext 
{
    public DbSet<Volume> Volumes { get; set; } = null!;
    public DbSet<Server> Servers { get; set; } = null!;
    public DbSet<Worker> Workers { get; set; } = null!;
    public DbSet<Model> Models { get; set; } = null!;
    public DbSet<ModelVersion> ModelVersions { get; set; } = null!;
    public DbSet<DefaultModelVersion> DefaultModelVersions { get; set; } = null!;


    public InferenceDbContext(DbContextOptions options) : base(options) { }

    public InferenceDbContext(DbContextOptions<InferenceDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("inference");
        builder.ApplyConfiguration(new VolumeEntityTypeConfiguration());
        builder.ApplyConfiguration(new ServerEntityTypeConfiguration());
        builder.ApplyConfiguration(new WorkerEntityTypeConfiguration());
        builder.ApplyConfiguration(new ModelEntityTypeConfiguration());
        builder.ApplyConfiguration(new ModelVersionEntityTypeConfiguration());
        builder.ApplyConfiguration(new DefaultModelVersionEntityTypeConfiguration());

        // TODO : 테스트용 데이터 추후 삭제 예정.
        _HasTestData(builder,
                volumeCount: 3,
                modelCount: 3,
                modelVersionCount: 2,
                serverCount: 1,
                workerCount: 1
                );
    }

    private static string BaseVolumeUri { get; } = Environment.GetEnvironmentVariable("DAQ60_TEST_VOLUME_BASE_DIR") ?? "c:/mirero/volumes/inference";

    private void _HasTestData(ModelBuilder builder, int volumeCount = 3, int modelCount = 3, int modelVersionCount = 2,
        int serverCount = 1, int workerCount = 1)
    {
        HasVolumeData(builder, volumeCount);
        HasModelData(builder, volumeCount, modelCount);
        HasModelVersionData(builder, volumeCount, modelCount, modelVersionCount);
        HasServerData(builder, serverCount);
        HasWorkerData(builder, serverCount, workerCount);
    }

    private void HasVolumeData(ModelBuilder builder, int count = 1)
    {
        for (var i = 1; i <= count; i++)
        {
            builder.Entity<Volume>().HasData(new Volume
            {
                Id = "volume" + i,
                Title = "Volume" + i,
                Type = "model",
                Uri = BaseVolumeUri + "/volume" + i,
                Capacity = 100000000
            });
        }
    }

    private void HasModelData(ModelBuilder builder, int volumeCount, int count = 1)
    {
        var info = new List<List<object?>>()
        {
            new() {"classification", "resnet"},
            new() {"object_detection", "fcnn"},
            new() {"segmentation", "deeplabv3"},
            new() {"anomaly", "cnn"}
        };

        var n = 1;
        for (var i = 1; i <= volumeCount; i++)
        {
            for (var j = 0; j < count; j++)
            {
                builder.Entity<Model>().HasData(new Model
                {
                    Id = n,
                    ModelName = "model_" + n,
                    TaskName = (string)info[i % info.Count][0]!,
                    NetworkName = (string)info[j%info.Count][1]!,
                    VolumeId = "volume" + i
                });
                n++;
            }
        }
    }

    private void HasModelVersionData(ModelBuilder builder, int volumeCount, int modelCount, int count = 1)
    {
        var modelId = 1;
        var dmvId = 1;
        var n = 1;

        var currentDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        
        for (var v = 0; v < volumeCount; v++)
        {
            for (var i = 0; i < modelCount; i++)
            {
                for (var j = 1; j <= count; j++)
                {
                    var random = new Random();
                    var createDate = currentDate.AddSeconds(-(random.Next(18000) * n));

                    builder.Entity<ModelVersion>().HasData(
                        new ModelVersion
                        {
                            Id = n,
                            Filename = "model_" + j + ".mar",
                            ModelId = modelId,
                            Version = j.ToString(),
                            CreateDate = createDate,
                            UpdateDate = createDate,
                            CreateUser = "test_user1",
                            UpdateUser = "test_user1"
                        });
                    if (j == 1)
                    {
                        builder.Entity<DefaultModelVersion>().HasData(
                            new DefaultModelVersion
                            {
                                Id = dmvId,
                                ModelId = modelId,
                                ModelVersionId = n,
                            });
                        dmvId++;
                    }
                    n++;
                }
                modelId++;
            }

        }
        
    }

    private void HasServerData(ModelBuilder builder, int count = 1)
    {
        var oss = new List<List<object?>>()
        {
            new() {"CentOS", "7", 1, (long)512000000, null, 0, (long)0},
            new() {"Ubuntu", "16.04 LTS", 0, (long)0, "DL360GEN10", 2, (long)512000000},
            new() {"Debian", "11", 1, (long)512000000, "A100", 1, (long)256000000}
        };

        var n = 1;

        for (var j = 0; j < count; j++)
        {
            builder.Entity<Server>().HasData(new Server
            {
                Id = "server_" + n,
                Address = IPAddress.Parse("192.168.70.32"),
                OsType = (string)oss[j % oss.Count][0]!,
                OsVersion = (string)oss[j % oss.Count][1]!,
                CpuCount = (int)oss[j % oss.Count][2]!,
                CpuMemory = (long)oss[j % oss.Count][3]!,
                GpuName = (string)oss[j % oss.Count][4]!,
                GpuCount = (int)oss[j % oss.Count][5]!,
                GpuMemory = (long)oss[j % oss.Count][6]!,
            });
            n++;
        }
    }

    private void HasWorkerData(ModelBuilder builder, int serverCount, int count = 1)
    {
        var info = new List<List<object?>>()
        {
            new() {"tf", 1, (long)512000000, 1, (long)256000000},
            new() {"torch", 0, (long)0, 2, (long)512000000 },
            new() {"mms", 1, (long)512000000, 0, (long)0},
            new() {"trt", 1, (long)512000000, 1, (long)256000000}
        };

        var workerId = 1;

        for (var i = 1; i <= serverCount; i++)
        {
            for (var j = 1; j <= count; j++)
            {
                builder.Entity<Worker>().HasData(new Worker()
                {
                    Id = "worker_" + workerId,
                    ServerId = "server_" + i,
                    ModelVersionId = workerId,
                    Port = 8193,
                    ServingType = (string)info[j%info.Count][0]!,
                    CpuCount = (int)info[j % info.Count][1]!,
                    CpuMemory = (long)info[j % info.Count][2]!,
                    GpuCount = (int)info[j % info.Count][3]!,
                    GpuMemory = (long)info[j % info.Count][4]!,
                    Properties = $"{{\"InferenceApi\":{8193}, \"ManagementApi\":{8194}}}"
                });

                workerId++;
            }
        }
    }
}