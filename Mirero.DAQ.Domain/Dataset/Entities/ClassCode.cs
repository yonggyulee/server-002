using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mirero.DAQ.Domain.Dataset.Entities;

public class ClassCode
{
    public long Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public string CreateUser { get; set; }
    public string UpdateUser { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
    public long ClassCodeSetId { get; set; }
    
    public ClassCodeSet ClassCodeSet { get; set; }
    public ICollection<ClassCodeReferenceImage> ClassCodeReferenceImages { get; set; } =
        new List<ClassCodeReferenceImage>();
}