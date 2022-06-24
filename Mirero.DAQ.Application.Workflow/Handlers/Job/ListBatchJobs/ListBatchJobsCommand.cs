using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.ListBatchJobs;

public class ListBatchJobsCommand : IRequest<ListBatchJobsResponse>
{
    public ListBatchJobsRequest Request { get; set; }

    public ListBatchJobsCommand(ListBatchJobsRequest request)
    {
        Request = request;
    }
}