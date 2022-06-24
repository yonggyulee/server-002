using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ClassCodeSetEntity = Mirero.DAQ.Domain.Dataset.Entities.ClassCodeSet;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.ListClassCodeSets;

public class ListClassCodeSetsHandler : ClassCodeHandler, IRequestHandler<ListClassCodeSetsCommand, ListClassCodeSetsResponse>
{
    public ListClassCodeSetsHandler(IDbContextFactory<DatasetDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory, ILogger<ListClassCodeSetsHandler> logger, IFileStorage fileStorage,
        IMapper mapper) : base(dbContextFactory, lockProviderFactory, logger, fileStorage, mapper)
    {
    }

    public async Task<ListClassCodeSetsResponse> Handle(ListClassCodeSetsCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        var (count, items) = await _dbContext.ClassCodeSets
            .Include(c => c.ClassCodes)
            .ThenInclude(c => c.ClassCodeReferenceImages)
            .Include(c => c.Volume)
            .AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter, cancellationToken);

        var classCodeSets = items.ToList();

        if (!classCodeSets.Any())
        {
            return _mapper.From((request, new List<ClassCodeSet>(), count)).AdaptToType<ListClassCodeSetsResponse>();
        }

        IEnumerable<ClassCodeSet> classCodeSetList;

        if (request.WithBuffer)
        {
            var classCodeSetIds =
                classCodeSets.Select(c => c.Id).Distinct().Select(c => GenerateLockId<ClassCodeSet>(c));

            await using var @lock =
                await _lockProvider.AcquireReadLockAsync(classCodeSetIds, request.LockTimeoutSec,
                    cancellationToken: cancellationToken);

            classCodeSetList = await Task.WhenAll(classCodeSets.Select(cs => _ToClassCodeSetWithImageAsync(cs, cancellationToken)));
        }
        else
        {
            classCodeSetList = classCodeSets.Select(_ToClassCodeSet);
        }

        return _mapper.From((request, classCodeSetList, count)).AdaptToType<ListClassCodeSetsResponse>();
    }

    private async Task<ClassCodeSet> _ToClassCodeSetWithImageAsync(ClassCodeSetEntity classCodeSet, CancellationToken cancellationToken)
    {
        var classCodeSetDto = _mapper.From(classCodeSet).AdaptToType<ClassCodeSet>();

        var classCodes = await Task.WhenAll(classCodeSet.ClassCodes.Select(c => _ToClassCodeWithImageAsync(c, cancellationToken)));
        classCodeSetDto.ClassCodes.AddRange(classCodes);

        return classCodeSetDto;
    }

    private ClassCodeSet _ToClassCodeSet(ClassCodeSetEntity classCodeSet)
    {
        var classCodeSetDto = _mapper.From(classCodeSet).AdaptToType<ClassCodeSet>();
        var classCodes = classCodeSet.ClassCodes.Select(c => _mapper.From(c).AdaptToType<Domain.Dataset.Protos.V1.ClassCode>());
        classCodeSetDto.ClassCodes.AddRange(classCodes);
        return classCodeSetDto;
    }
    
}