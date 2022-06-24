using Google.Protobuf;
using Mapster;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using VolumeEntity = Mirero.DAQ.Domain.Dataset.Entities.Volume;
using ClassCodeSetEntity = Mirero.DAQ.Domain.Dataset.Entities.ClassCodeSet;
using ImageDatasetEntity = Mirero.DAQ.Domain.Dataset.Entities.ImageDataset;
using SampleEntity = Mirero.DAQ.Domain.Dataset.Entities.Sample;
using ImageEntity = Mirero.DAQ.Domain.Dataset.Entities.Image;
using ClassCodeReferenceImageEntity = Mirero.DAQ.Domain.Dataset.Entities.ClassCodeReferenceImage;
using ClassCodeEntity = Mirero.DAQ.Domain.Dataset.Entities.ClassCode;
using ClassificationGtDatasetEntity = Mirero.DAQ.Domain.Dataset.Entities.ClassificationGtDataset;
using ObjectDetectionGtDatasetEntity = Mirero.DAQ.Domain.Dataset.Entities.ObjectDetectionGtDataset;
using SegmentationGtDatasetEntity = Mirero.DAQ.Domain.Dataset.Entities.SegmentationGtDataset;
using ClassificationGtEntity = Mirero.DAQ.Domain.Dataset.Entities.ClassificationGt;
using ObjectDetectionGtEntity = Mirero.DAQ.Domain.Dataset.Entities.ObjectDetectionGt;
using SegmentationGtEntity = Mirero.DAQ.Domain.Dataset.Entities.SegmentationGt;

namespace Mirero.DAQ.Service.Extensions.Dataset;

public static class DatasetMapperExtension
{
    public static TypeAdapterConfig AddDatasetMapperConfig(this TypeAdapterConfig config)
    {
        _AddImageDatasetMapperConfig(config);

        _AddVolumeMapperConfig(config);

        _AddSampleMapperConfig(config);

        _AddImageMapperConfig(config);

        _AddClassCodeSetMapperConfig(config);

        _AddClassCodeMapperConfig(config);

        _AddClassCodeReferenceImageMapperConfig(config);

        _AddClassificationGtDatasetMapperConfig(config);

        _AddObjectDetectionGtDatasetMapperConfig(config);

        _AddSegmentationGtDatasetMapperConfig(config);

        _AddClassificationGtMapperConfig(config);

        _AddObjectDetectionGtMapperConfig(config);

        _AddSegmentationGtMapperConfig(config);

        _AddGtDatasetMapperConfig(config);

        return config;
    }

    private static void _AddImageDatasetMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<ImageDatasetEntity, ImageDataset>()
            .Map(dest => dest.ThumbnailBuffer,
                src =>
                    src.ThumbnailBuffer == null ? null : ByteString.CopyFrom(src.ThumbnailBuffer));

        config.NewConfig<CreateImageDatasetRequest, ImageDatasetEntity>()
            .Map(dest => dest.CreateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            .Map(dest => dest.UpdateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));

        config.NewConfig<UpdateImageDatasetRequest, ImageDatasetEntity>()
            .Map(dest => dest.UpdateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));

        config
            .NewConfig<(ListImageDatasetsRequest Request
                , IEnumerable<ImageDataset> ImageDatasets
                , int Count),
                ListImageDatasetsResponse>()
            .ConstructUsing(src => new ListImageDatasetsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                Datasets = { src.ImageDatasets }
            });
    }

    private static void _AddVolumeMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<CreateVolumeRequest, VolumeEntity>()
            .Map(dest => dest.Usage, src => 0);

        config
            .NewConfig<(ListVolumesRequest Request
                , IEnumerable<Volume> Volumes
                , int Count),
                ListVolumesResponse>()
            .ConstructUsing(src => new ListVolumesResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                Volumes = { src.Volumes }
            });
    }

    private static void _AddSampleMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<(ListSamplesRequest Request
                , IEnumerable<Sample> Samples
                , int Count),
                ListSamplesResponse>()
            .ConstructUsing(src => new ListSamplesResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                Samples = { src.Samples }
            });

        config.NewConfig<SampleEntity, Sample>()
            .Ignore(dest => dest.Images);

        config.NewConfig<SampleEntity, Sample>()
            .Ignore(dest => dest.Images);

        // Update 시 관계 데이터 수정 방지.
        config.NewConfig<SampleEntity, SampleEntity>()
            .Ignore(dest => dest.ImageDataset);
    }

    private static void _AddImageMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<Image, ImageEntity>()
            .Map(dest => dest.Buffer, src => src.Buffer.ToByteArray())
            .Map(dest => dest.ThumbnailBuffer,
                src => src.ThumbnailBuffer == null ? null : src.ThumbnailBuffer.ToByteArray());

        config.NewConfig<ImageEntity, Image>()
            .Map(dest => dest.Buffer, src => src.Buffer == null ? null : ByteString.CopyFrom(src.Buffer))
            .Map(dest => dest.ThumbnailBuffer,
                src => src.ThumbnailBuffer == null ? null : ByteString.CopyFrom(src.ThumbnailBuffer));
    }

    private static void _AddClassCodeSetMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<CreateClassCodeSetRequest, ClassCodeSetEntity>()
            .Map(dest => dest.CreateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            .Map(dest => dest.UpdateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));

        config.NewConfig<UpdateClassCodeSetRequest, ClassCodeSetEntity>()
            .Map(dest => dest.UpdateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));

        config
            .NewConfig<(ListClassCodeSetsRequest Request
                , IEnumerable<ClassCodeSet> ClassCodeSets
                , int Count),
                ListClassCodeSetsResponse>()
            .ConstructUsing(src => new ListClassCodeSetsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                ClassCodeSets = { src.ClassCodeSets }
            });

        // Update 시 CreateDate 및 관계 데이터 수정 방지.
        //config.NewConfig<ClassCodeSetEntity, ClassCodeSetEntity>()
        //    .Ignore(dest => dest.CreateDate)
        //    .Ignore(dest => dest.Volume);
    }

    private static void _AddClassCodeMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<CreateClassCodeRequest, ClassCodeEntity>()
            .Map(dest => dest.CreateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            .Map(dest => dest.UpdateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));

        config.NewConfig<UpdateClassCodeRequest, ClassCodeEntity>()
            .Map(dest => dest.UpdateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));

        config
            .NewConfig<(ListClassCodesRequest Request
                , IEnumerable<ClassCode> ClassCodes
                , int Count),
                ListClassCodesResponse>()
            .ConstructUsing(src => new ListClassCodesResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                ClassCodes = { src.ClassCodes }
            });

        // Update 시 CreateDate 및 관계 데이터 수정 방지.
        config.NewConfig<ClassCodeEntity, ClassCodeEntity>()
            .Ignore(dest => dest.CreateDate)
            .Ignore(dest => dest.ClassCodeSet);
    }

    private static void _AddClassCodeReferenceImageMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<(ListClassCodeReferenceImagesRequest Request
                , IEnumerable<ClassCodeReferenceImage> ClassCodeReferenceImages
                , int Count),
                ListClassCodeReferenceImagesResponse>()
            .ConstructUsing(src => new ListClassCodeReferenceImagesResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                ClassCodeReferenceImages = { src.ClassCodeReferenceImages }
            });

        config.NewConfig<ClassCodeReferenceImage, ClassCodeReferenceImageEntity>()
            .Map(dest => dest.Buffer,
                src => src.Buffer.ToByteArray());
        config.NewConfig<ClassCodeReferenceImageEntity, ClassCodeReferenceImage>()
            .Map(dest => dest.Buffer,
                src =>
                    src.Buffer == null ? null : ByteString.CopyFrom(src.Buffer));
    }

    private static void _AddClassificationGtDatasetMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<CreateClassificationGtDatasetRequest, ClassificationGtDatasetEntity>()
            .Map(dest => dest.CreateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            .Map(dest => dest.UpdateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));

        config.NewConfig<UpdateClassificationGtDatasetRequest, ClassificationGtDatasetEntity>()
            .Map(dest => dest.UpdateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));

        config
            .NewConfig<(ListClassificationGtDatasetsRequest Request
                , IEnumerable<ClassificationGtDataset> ClassificationGtDatasets
                , int Count),
                ListClassificationGtDatasetsResponse>()
            .ConstructUsing(src => new ListClassificationGtDatasetsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                ClassificationGtDatasets = { src.ClassificationGtDatasets }
            });
    }

    private static void _AddObjectDetectionGtDatasetMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<CreateObjectDetectionGtDatasetRequest, ObjectDetectionGtDatasetEntity>()
            .Map(dest => dest.CreateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            .Map(dest => dest.UpdateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));

        config.NewConfig<UpdateObjectDetectionGtDatasetRequest, ObjectDetectionGtDatasetEntity>()
            .Map(dest => dest.UpdateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));

        config
            .NewConfig<(ListObjectDetectionGtDatasetsRequest Request
                , IEnumerable<ObjectDetectionGtDataset> ObjectDetectionGtDatasets
                , int Count),
                ListObjectDetectionGtDatasetsResponse>()
            .ConstructUsing(src => new ListObjectDetectionGtDatasetsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                ObjectDetectionGtDatasets = { src.ObjectDetectionGtDatasets }
            });
    }

    private static void _AddSegmentationGtDatasetMapperConfig(TypeAdapterConfig config)
    {
        config.NewConfig<CreateSegmentationGtDatasetRequest, SegmentationGtDatasetEntity>()
            .Map(dest => dest.CreateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            .Map(dest => dest.UpdateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));

        config.NewConfig<UpdateSegmentationGtDatasetRequest, SegmentationGtDatasetEntity>()
            .Map(dest => dest.UpdateDate,
                src => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc));

        config
            .NewConfig<(ListSegmentationGtDatasetsRequest Request
                , IEnumerable<SegmentationGtDataset> SegmentationGtDatasets
                , int Count),
                ListSegmentationGtDatasetsResponse>()
            .ConstructUsing(src => new ListSegmentationGtDatasetsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                SegmentationGtDatasets = { src.SegmentationGtDatasets }
            });

        // Update 시 CreateDate 및 관계 데이터 수정 방지.
        config.NewConfig<SegmentationGtDatasetEntity, SegmentationGtDatasetEntity>()
            .Ignore(dest => dest.CreateDate)
            .Ignore(dest => dest.Volume)
            .Ignore(dest => dest.ImageDataset)
            .Ignore(dest => dest.ClassCodeSet);
    }

    private static void _AddClassificationGtMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<IEnumerable<ClassificationGt>, ListClassificationGtsResponse>()
            .ConstructUsing(src => new ListClassificationGtsResponse
            {
                ClassificationGts = { src }
            });

        config
            .NewConfig<ClassificationGtEntity, ClassificationGt>()
            .Map(dest => dest.ClassCodeName, src => src.ClassCode.Name);
    }

    private static void _AddObjectDetectionGtMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<(ListObjectDetectionGtsRequest Request
                , IEnumerable<ObjectDetectionGt> ObjectDetectionGts
                , int Count),
                ListObjectDetectionGtsResponse>()
            .ConstructUsing(src => new ListObjectDetectionGtsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                ObjectDetectionGts = { src.ObjectDetectionGts }
            });

        config
            .NewConfig<ObjectDetectionGtEntity, ObjectDetectionGt>()
            .Map(dest => dest.Buffer,
                src => src.Buffer == null ? null : ByteString.CopyFrom(src.Buffer));
    }

    private static void _AddSegmentationGtMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<(ListSegmentationGtsRequest Request
                , IEnumerable<SegmentationGt> SegmentationGts
                , int Count),
                ListSegmentationGtsResponse>()
            .ConstructUsing(src => new ListSegmentationGtsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                SegmentationGts = { src.SegmentationGts }
            });

        config
            .NewConfig<SegmentationGtEntity, SegmentationGt>()
            .Map(dest => dest.Buffer,
                src => src.Buffer == null ? null : ByteString.CopyFrom(src.Buffer));
    }

    private static void _AddGtDatasetMapperConfig(TypeAdapterConfig config)
    {
        config
            .NewConfig<(ListGtDatasetsRequest Request,
                IEnumerable<GtDataset> GtDatasets,
                int Count), ListGtDatasetsResponse>()
            .ConstructUsing(src => new ListGtDatasetsResponse
            {
                PageResult = new PageResult
                {
                    PageIndex = src.Request.QueryParameter.PageIndex,
                    PageSize = src.Request.QueryParameter.PageSize,
                    Count = src.Count
                },
                GtDatasets = { src.GtDatasets }
            });
    }
}