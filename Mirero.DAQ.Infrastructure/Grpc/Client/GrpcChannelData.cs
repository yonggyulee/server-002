using System.Net;
using Grpc.Net.Client;

namespace Mirero.DAQ.Infrastructure.Grpc.Client;

public class GrpcChannelData : IDisposable, ICloneable
{
    public string Address { get; private set; }
    public GrpcChannel Channel { get; private set; }

    public GrpcChannelData(string address)
    {
        Address = address;
        Channel = GrpcChannel.ForAddress(Address.ToString());
        
    }

    public object Clone()
    {
        return this;
    }

    private void ReleaseUnmanagedResources()
    {
        // TODO release unmanaged resources here
    }

    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing)
        {
            Channel.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~GrpcChannelData()
    {
        Dispose(false);
    }
}