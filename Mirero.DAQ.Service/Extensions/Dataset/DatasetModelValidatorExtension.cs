using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Service.Validations.Dataset;

namespace Mirero.DAQ.Service.Extensions.Dataset;

public static class DatasetModelValidatorExtension
{
    public static IServiceCollection AddDatasetValidator(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IValidator<ImageDataset>, ImageDatasetValidator>();
        services.AddTransient<IValidator<Volume>, VolumeValidator>();
        services.AddTransient<IValidator<Sample>, SampleValidator>();
        services.AddTransient<IValidator<Image>, ImageValidator>();
        services.AddTransient<IValidator<ClassCodeSet>, ClassCodeSetValidator>();
        services.AddTransient<IValidator<ClassCode>, ClassCodeValidator>();
        services.AddTransient<IValidator<ClassCodeReferenceImage>, ClassCodeReferenceImageValidator>();
        services.AddTransient<IValidator<ClassificationGtDataset>, ClassificationGtDatasetValidator>();
        services.AddTransient<IValidator<ObjectDetectionGtDataset>, ObjectDetectionGtDatasetValidator>();
        services.AddTransient<IValidator<SegmentationGtDataset>, SegmentationGtDatasetValidator>();
        services.AddTransient<IValidator<ClassificationGt>, ClassificationGtValidator>();
        services.AddTransient<IValidator<ObjectDetectionGt>, ObjectDetectionGtValidator>();
        services.AddTransient<IValidator<SegmentationGt>, SegmentationGtValidator>();
        
        return services;
    }
}