using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.MonitoringBatchJobs;

public class MonitoringBatchJobsCommand : IRequest<MonitoringBatchJobsResponse>
{
    public IAsyncStreamReader<MonitoringBatchJobsRequest> Request { get; set; }

    public MonitoringBatchJobsCommand(IAsyncStreamReader<MonitoringBatchJobsRequest> request)
    {
        Request = request;
    }
}