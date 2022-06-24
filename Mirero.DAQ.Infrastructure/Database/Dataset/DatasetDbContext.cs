using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Dataset.Entities;
using Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;
using Newtonsoft.Json.Linq;

namespace Mirero.DAQ.Infrastructure.Database.Dataset;

public class DatasetDbContext : DbContext
{
    public DbSet<Volume> Volumes { get; set; } = null!;
    public DbSet<ImageDataset> ImageDatasets { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;
    public DbSet<ClassCodeSet> ClassCodeSets { get; set; } = null!;
    public DbSet<ClassCode> ClassCodes { get; set; } = null!;
    public DbSet<ClassCodeReferenceImage> ClassCodeReferenceImages { get; set; } = null!;
    public DbSet<ClassificationGt> ClassificationGts { get; set; } = null!;
    public DbSet<ObjectDetectionGt> ObjectDetectionGts { get; set; } = null!;
    public DbSet<SegmentationGt> SegmentationGts { get; set; } = null!;
    public DbSet<Sample> Samples { get; set; } = null!;
    public DbSet<GtDataset> GtDatasets { get; set; } = null!;
    public DbSet<ClassificationGtDataset> ClassificationGtDatasets { get; set; } = null!;
    public DbSet<ObjectDetectionGtDataset> ObjectDetectionGtDatasets { get; set; } = null!;
    public DbSet<SegmentationGtDataset> SegmentationGtDatasets { get; set; } = null!;

    public DatasetDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DatasetDbContext(DbContextOptions<DatasetDbContext> options) : base(options) 
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("dataset");
        builder.ApplyConfiguration(new ImageDatasetEntityTypeConfiguration());
        builder.ApplyConfiguration(new ImageEntityTypeConfiguration());
        builder.ApplyConfiguration(new ClassificationGtEntityTypeConfiguration());
        builder.ApplyConfiguration(new ObjectDetectionGtEntityTypeConfiguration());
        builder.ApplyConfiguration(new SegmentationGtEntityTypeConfiguration());
        builder.ApplyConfiguration(new ClassCodeSetEntityTypeConfiguration());
        builder.ApplyConfiguration(new ClassCodeEntityTypeConfiguration());
        builder.ApplyConfiguration(new VolumeEntityTypeConfiguration());
        builder.ApplyConfiguration(new SampleEntityTypeConfiguration());
        builder.ApplyConfiguration(new GtDatasetEntityTypeConfiguration());
        builder.ApplyConfiguration(new ClassificationGtDatasetEntityTypeConfiguration());
        builder.ApplyConfiguration(new ObjectDetectionGtDatasetEntityTypeConfiguration());
        builder.ApplyConfiguration(new SegmentationGtDatasetEntityTypeConfiguration());
        builder.ApplyConfiguration(new ClassCodeReferenceImageEntityTypeConfiguration());
        // builder.ApplyConfiguration(new ImageDatasetGtDatasetClassCodeSetEntityTypeConfiguration());
        // builder.ApplyConfiguration(new VolumeDatasetEntityTypeConfiguration());

        _HasTestData(builder, 
            volumeCount: 3,
            imageDatasetCount: 30,
            sampleCount: 50,
            classCodeSetCount: 30,
            classCodeCount: 5,
            imageCount: 3
            );
    }

    private static string BaseVolumeUri { get; } = Environment.GetEnvironmentVariable("DAQ60_TEST_VOLUME_BASE_DIR") ?? "c:/mirero/volumes";

    private void _HasTestData(ModelBuilder builder, int volumeCount = 1, int imageDatasetCount = 1, int sampleCount = 1,
        int classCodeSetCount = 1, int classCodeCount = 3, int imageCount = 3)
    {
        _HasVolumeData(builder, volumeCount);
        _HasImageDatasetData(builder, volumeCount, imageDatasetCount);
        _HasSampleData(builder, imageDatasetCount, sampleCount, imageCount);
        _HasClassCodeSetData(builder, volumeCount, classCodeSetCount);
        _HasClassCodeData(builder, classCodeSetCount, classCodeCount, imageCount);
    }

    private void _HasVolumeData(ModelBuilder builder, int count = 1)
    {
        for (var i = 1; i <= count; i++)
        {
            builder.Entity<Volume>().HasData(new Volume
            {
                Id = "volume" + i,
                Title = "Volume" + i,
                Type = "image",
                Uri = Path.Combine(BaseVolumeUri, "volume") + i,
                Capacity = 100000000
            });
        }
    }

    private void _HasImageDatasetData(ModelBuilder builder, int volumeCount, int count = 1)
    {
        var currentDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        var random = new Random();

        var n = 1;
        for (var i = 1; i <= volumeCount; i++)
        {
            for (var j = 0; j < count; j++)
            {
                var createDate = currentDate.AddSeconds(-(random.Next(18000) * n));

                builder.Entity<ImageDataset>().HasData(new ImageDataset
                {
                    Id = n,
                    Title = "Dataset" + n,
                    DirectoryName = "dataset" + n,
                    // Properties = "{ \"key\" : \"test_properties3\" }",
                    // Description = "Descriptions",
                    CreateUser = "test_user1",
                    UpdateUser = "test_user1",
                    CreateDate = createDate,
                    UpdateDate = createDate,
                    VolumeId = "volume" + i
                });
                n++;
            }
        }
    }

    private void _HasSampleData(ModelBuilder builder, int imageDatasetCount, int count = 1, int imageCount = 3)
    {
        var imageId = 1;

        for (var i = 1; i <= imageDatasetCount; i++)
        {
            for (var j = 1; j <= count; j++)
            {
                builder.Entity<Sample>().HasData(
                    new Sample
                    {
                        Id = j,
                        DatasetId = i
                    });

                for (var k = 0; k < imageCount; k++)
                {
                    builder.Entity<Image>().HasData(new Image
                    {
                        Id = imageId,
                        SampleId = j,
                        DatasetId = i,
                        Filename = "image" + imageId + ".jpg",
                        Extension = "jpg",
                        ImageCode = "right",
                    });
                    imageId++;
                }
            }
        }
    }

    private void _HasClassCodeSetData(ModelBuilder builder, int volumeCount, int count = 1)
    {
        var tasks = new List<string> { "Classification", "ObjectDetection", "Segmentation" };

        var n = 1;

        var currentDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        var random = new Random();

        for (var i = 1; i <= volumeCount; i++)
        {
            for (var j = 0; j < count; j++)
            {
                var createDate = currentDate.AddSeconds(-(random.Next(18000) * n));

                builder.Entity<ClassCodeSet>().HasData(new ClassCodeSet
                {
                    Id = n,
                    Title = "ClassCodeSet" + n,
                    DirectoryName = "class_code_set" + n,
                    Task = tasks[j % tasks.Count],
                    // Properties = "{ \"key\" : \"test_properties3\" }",
                    // Description = "Descriptions",
                    CreateUser = "test_user1",
                    UpdateUser = "test_user1",
                    CreateDate = createDate,
                    UpdateDate = createDate,
                    VolumeId = "volume" + i
                });
                n++;
            }
        }
    }

    private void _HasClassCodeData(ModelBuilder builder, int classCodeSetCount, int count = 1, int imageCount = 3)
    {
        var classCodeId = 1;
        var referenceImageId = 1;

        var currentDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        var random = new Random();

        for (var i = 1; i <= classCodeSetCount; i++)
        {
            for (var j = 1; j <= count; j++)
            {
                var createDate = currentDate.AddSeconds(-(random.Next(60) * classCodeId));

                builder.Entity<ClassCode>().HasData(new ClassCode
                {
                    Id = classCodeId,
                    Name = "code" + j,
                    Code = j,
                    CreateUser = "test_user1",
                    UpdateUser = "test_user1",
                    CreateDate = createDate,
                    UpdateDate = createDate,
                    ClassCodeSetId = i
                });

                for (var k = 0; k < imageCount; k++)
                {

                    builder.Entity<ClassCodeReferenceImage>().HasData(new ClassCodeReferenceImage
                    {
                        Id = referenceImageId,
                        ClassCodeId = classCodeId,
                        //ClassCodeSetId = i,
                        Filename = "image" + referenceImageId + ".jpg",
                        Extension = "jpg",
                    });
                    referenceImageId++;
                }

                classCodeId++;
            }
        }
    }
}