namespace Mirero.DAQ.Domain.Dataset.Entities;

public class ClassCodeReferenceImage
{
    public long Id { get; set; }
    public string Filename { get; set; }
    public string Extension { get; set; }
    public byte[]? Buffer { get; set; }
    public string Description { get; set; }
    //public long ClassCodeSetId { get; set; }
    public long ClassCodeId { get; set; }
    
    //public ClassCodeSet ClassCodeSet { get; set; }
    public ClassCode ClassCode { get; set; }
}