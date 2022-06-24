using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mirero.DAQ.Domain.Dataset.Entities;

public class ClassificationGt
{
    public long Id { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
    public long DatasetId { get; set; }
    public long ImageId { get; set; }
    public long ClassCodeId { get; set; }
    
    public GtDataset GtDataset { get; set; }
    public Image Image { get; set; }
    public ClassCode ClassCode { get; set; }
}