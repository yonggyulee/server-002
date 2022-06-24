using MapsterMapper;
using MediatR;
using Mirero.DAQ.Application.Workflow.ListJobsManager;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.ListJobs;

public class ListJobsHandler : IRequestHandler<ListJobsCommand, ListJobsResponse>
{
    private readonly ListJobManagerFactory _listJobManagerFactory;
    private readonly IMapper _mapper;
    public ListJobsHandler(ListJobManagerFactory listJobManagerFactory, IMapper mapper)
    {
        _listJobManagerFactory = listJobManagerFactory;
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ListJobsResponse> Handle(ListJobsCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        await _listJobManagerFactory.InitializeAsync(request.BatchJobId, cancellationToken);
        
        var (count, items) = await _listJobManagerFactory.GetJobs(request.QueryParameter, cancellationToken);
            
        return _mapper.From((request, items.Select(x => _mapper.From(x).AdaptToType<Domain.Workflow.Protos.V1.Job>()), count, _listJobManagerFactory.IsCompleted)).AdaptToType<ListJobsResponse>();
    }
}