using Mirero.DAQ.Domain.Common.Data;
using QueryParameter = Mirero.DAQ.Domain.Common.Protos.QueryParameter;

namespace Mirero.DAQ.Domain.Common.Dtos;

public interface IPageListRequest
{
    public QueryParameter QueryParameter { get; }
}