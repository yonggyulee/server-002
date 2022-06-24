using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.ListClassCodeSets;

public class ListClassCodeSetsCommand : IRequest<ListClassCodeSetsResponse>
{
    public ListClassCodeSetsRequest Request { get; set; }

    public ListClassCodeSetsCommand(ListClassCodeSetsRequest request)
    {
        Request = request;
    }
}