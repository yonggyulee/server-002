namespace Mirero.DAQ.Infrastructure.Container.Docker;

public class ContainerOptionBuilder
{
    private string _optionString;

    public void Set(string optionString)
    {
        _optionString = optionString;
    }

    public string Build()
    {
        return _optionString;
    }
}