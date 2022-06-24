using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Application.Workflow.UriGenerator;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Grpc.StreamCreator;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Workflow.StreamCreator;

public class WorkflowDownloadStreamCreator : IReadStreamCreator
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IUriGenerator _uriGenerator;
    private readonly IFileStorage _fileStorage;
    
    public WorkflowDownloadStreamCreator(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
        , IUriGenerator uriGenerator
        , IFileStorage fileStorage)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _uriGenerator = uriGenerator ?? throw new ArgumentNullException(nameof(uriGenerator));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
    }
    
    public async Task<Stream> ReadStreamAsync(long id, CancellationToken cancellationToken = default)
    {
        var uri = await GetWorkflowUri(id, cancellationToken);
        return _fileStorage.OpenReadFileStream(uri);
    }
    
    private async Task<string> GetWorkflowUri(long id, CancellationToken cancellationToken)
    {
        var workflowVersion = await _dbContext.WorkflowVersions
            .Include(wv => wv.Workflow)
            .ThenInclude(w => w.Volume)
            .SingleOrDefaultAsync(m => m.Id == id, cancellationToken: cancellationToken) ?? throw new NullReferenceException();
        
        return Path.Combine(
            _uriGenerator.GetWorkflowVersionUri(workflowVersion.Workflow.Volume.Uri
                , workflowVersion.WorkflowId
                , workflowVersion.Id)   
            , workflowVersion.FileName);
    }
    
}