using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mirero.DAQ.Domain.Dataset.Entities;

public class ClassCodeSet
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Task { get; set; }
    public string DirectoryName { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public string CreateUser { get; set; }
    public string UpdateUser { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
    public string VolumeId { get; set; }
    
    public Volume Volume { get; set; }
    public ICollection<ClassCode> ClassCodes { get; set; } = new List<ClassCode>();
}