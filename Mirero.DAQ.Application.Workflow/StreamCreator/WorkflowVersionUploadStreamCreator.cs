using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Application.Workflow.UriGenerator;
using Mirero.DAQ.Domain.Workflow.Entities;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Grpc.StreamCreator;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Workflow.StreamCreator;

public class WorkflowVersionUploadStreamCreator : IIdentifiedWriteStreamCreator
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IUriGenerator _uriGenerator;
    private readonly IFileStorage _fileStorage;
    
    private long? _id;
    private string? _uri;
    
    public WorkflowVersionUploadStreamCreator(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
        , IUriGenerator uriGenerator
        , IFileStorage fileStorage)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _uriGenerator = uriGenerator ?? throw new ArgumentNullException(nameof(uriGenerator));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
    }

    public long? GetId()
    {
        return _id;
    }
    
    public async Task<Stream> CreateStreamAsync(long id, CancellationToken cancellationToken)
    {
        _id = id;
        _uri = await GetWorkflowUri(id, cancellationToken);
        return _fileStorage.CreateFileStream(_uri);
    }
    
    public void DeleteStreamAsync(CancellationToken cancellationToken)
    {
        if (File.Exists(_uri))
            File.Delete(_uri);
    }

    private async Task<string> GetWorkflowUri(long id, CancellationToken cancellationToken)
    {
        var workflowVersion = await _dbContext.WorkflowVersions
            .Include(wv => wv.Workflow)
            .ThenInclude(w => w.Volume)
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id, cancellationToken: cancellationToken) ?? throw new NullReferenceException();
        
        return Path.Combine(
            _uriGenerator.GetWorkflowVersionUri(workflowVersion.Workflow.Volume.Uri
                , workflowVersion.WorkflowId
                , workflowVersion.Id)   
            , workflowVersion.FileName);
    }
}