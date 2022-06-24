using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using System.Reflection;
using Mirero.DAQ.Domain.Common.Extensions;

namespace Mirero.DAQ.Domain.Dataset.Entities;

public class ImageDataset
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string DirectoryName { get; set; }
    public byte[]? ThumbnailBuffer { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public string CreateUser { get; set; }
    public string UpdateUser { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
    public string VolumeId { get; set; }
    
    public Volume Volume { get; set; }
}