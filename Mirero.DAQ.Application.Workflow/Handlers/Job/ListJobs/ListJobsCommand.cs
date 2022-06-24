using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.ListJobs;

public class ListJobsCommand : IRequest<ListJobsResponse>
{
    public ListJobsRequest Request { get; set; }

    public ListJobsCommand(ListJobsRequest request)
    {
        Request = request;
    }
}