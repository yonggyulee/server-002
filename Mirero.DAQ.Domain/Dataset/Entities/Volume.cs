using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mirero.DAQ.Domain.Common.Extensions;

namespace Mirero.DAQ.Domain.Dataset.Entities;

public class Volume
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Uri { get; set; }
    public long Usage { get; set; } // KB
    public long Capacity { get; set; } // KB
    public string Properties { get; set; }
    public string Description { get; set; }
}