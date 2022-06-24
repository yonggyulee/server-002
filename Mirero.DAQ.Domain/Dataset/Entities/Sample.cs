namespace Mirero.DAQ.Domain.Dataset.Entities;

public class Sample
{
    public int Id { get; set; }     // Not Auto Increment
    public string Properties { get; set; }
    public string Description { get; set; }
    public long DatasetId { get; set; }

    public ImageDataset ImageDataset { get; set; }
    public ICollection<Image> Images { get; set; } = new List<Image>();
}