using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mirero.DAQ.Domain.Dataset.Entities;

public class Image
{
    public long Id { get; set; }
    public string Filename { get; set; }
    public string Extension { get; set; }
    public string ImageCode { get; set; }
    public byte[]? ThumbnailBuffer { get; set; }
    public byte[]? Buffer { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
    public long DatasetId { get; set; }
    public int SampleId { get; set; }
    
    public Sample Sample { get; set; }
}