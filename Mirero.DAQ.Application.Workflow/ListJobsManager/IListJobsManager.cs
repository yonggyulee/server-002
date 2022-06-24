using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Workflow.Entities;

namespace Mirero.DAQ.Application.Workflow.ListJobsManager;

public interface IListJobsManager
{
    Task<(int Count, IEnumerable<Job> Items)> GetJobs(BatchJob batchJob,
        QueryParameter queryParameter, CancellationToken cancellationToken);
}