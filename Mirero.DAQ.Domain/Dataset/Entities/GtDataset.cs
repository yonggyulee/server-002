namespace Mirero.DAQ.Domain.Dataset.Entities;

public class GtDataset
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string DirectoryName { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public string CreateUser { get; set; }
    public string UpdateUser { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
    public string VolumeId { get; set; }
    public long ImageDatasetId { get; set; }
    public long ClassCodeSetId { get; set; }
    
    public Volume Volume { get; set; }
    public ImageDataset ImageDataset { get; set; }
    public ClassCodeSet ClassCodeSet { get; set; }
}