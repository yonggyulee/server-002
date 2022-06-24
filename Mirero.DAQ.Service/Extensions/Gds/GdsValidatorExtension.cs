using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Mirero.DAQ.Service.Validations.Gds.Gds;
using Mirero.DAQ.Service.Validations.Gds.Server;
using Mirero.DAQ.Service.Validations.Gds.Volume;

namespace Mirero.DAQ.Service.Extensions.Gds;

public static class GdsValidatorExtension
{
    public static IServiceCollection AddGdsValidator(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddTransient<IValidator<CreateVolumeRequest>, CreateVolumeRequestValidator>();
        service.AddTransient<IValidator<ListVolumesRequest>, ListVolumeRequestValidator>();
        service.AddTransient<IValidator<UpdateVolumeRequest>, UpdateVolumeRequestValidator>();
        service.AddTransient<IValidator<DeleteVolumeRequest>, DeleteVolumeRequestValidator>();
        service.AddTransient<IValidator<CreateServerRequest>, CreateServerRequestValidator>();
        service.AddTransient<IValidator<ListServersRequest>, ListServerRequestValidator>();
        service.AddTransient<IValidator<UpdateServerRequest>, UpdateServerRequestValidator>();
        service.AddTransient<IValidator<DeleteServerRequest>, DeleteServerRequestValidator>();
        service.AddTransient<IValidator<CreateGdsRequest>, CreateGdsRequestValidator>();
        service.AddTransient<IValidator<ListGdsesRequest>, ListGdsesRequestValidator>();
        service.AddTransient<IValidator<UploadGdsStreamRequest>, UploadGdsRequestValidator>();
        service.AddTransient<IValidator<DownloadGdsStreamRequest>, DownLoadGdsRequestValidator>();
        service.AddTransient<IValidator<DeleteGdsRequest>, DeleteGdsRequestValidator>();
        return service;
    }
}