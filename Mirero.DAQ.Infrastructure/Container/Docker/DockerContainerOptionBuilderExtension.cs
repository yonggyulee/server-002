namespace Mirero.DAQ.Infrastructure.Container.Docker;

public static class DockerContainerOptionBuilderExtension
{
    public static ContainerOptionBuilder UseDocker(this ContainerOptionBuilder builder, DockerContainerCreateOption option)
    {
        builder.Set(option.ToString());
        
        return builder;
    }
}