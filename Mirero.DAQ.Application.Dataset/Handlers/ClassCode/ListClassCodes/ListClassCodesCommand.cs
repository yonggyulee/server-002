using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.ListClassCodes;

public class ListClassCodesCommand : IRequest<ListClassCodesResponse>
{
    public ListClassCodesRequest Request { get; set; }

    public ListClassCodesCommand(ListClassCodesRequest request)
    {
        Request = request;
    }
}