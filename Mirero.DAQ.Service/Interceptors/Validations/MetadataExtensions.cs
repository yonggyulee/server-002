using FluentValidation.Results;
using Grpc.Core;
using Mirero.DAQ.Domain.Common.Extensions;

namespace Mirero.DAQ.Service.Interceptors.Validations;

internal static class MetadataExtensions
{
    public static Metadata ToValidationMetadata(this IList<ValidationFailure> failures)
    {
        var metadata = new Metadata();
        if (failures.Any())
        {
            metadata.Add(new Metadata.Entry("daq-grpc-validation-errors", failures.ToValidationTrailers().ToBase64()));
        }
        return metadata;
    }
}