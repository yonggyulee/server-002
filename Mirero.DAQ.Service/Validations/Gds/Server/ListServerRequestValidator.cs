using FluentValidation;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Gds.Server;

public class ListServerRequestValidator : AbstractValidator<ListServersRequest>
{
    public ListServerRequestValidator()
    {
        RuleFor(x => x.QueryParameter.PageIndex).GreaterThanOrEqualTo(0);
        RuleFor(x => x.QueryParameter.PageSize).NotEmpty().GreaterThan(0);
    }
}