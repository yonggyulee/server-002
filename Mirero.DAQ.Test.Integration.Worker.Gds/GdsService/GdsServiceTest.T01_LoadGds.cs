using Grpc.Core;
using Mirero.DAQ.Domain.Gds.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Mirero.DAQ.Domain.Gds.Protos.V1.Worker;
using System;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Worker.Gds.GdsService;

public partial class GdsServiceTest
{
    [Fact]
    public async void T01_LoadGds()
    {
        var loadGdsRequest = new LoadGdsRequest
        {
            Uri = "/home/mirero/temp/testdata/test1.gds",
            Layers = {
                new Layer {
                    DataType = "0",
                    Name = "layer_4",
                    Layer_ = "4"
                },
                new Layer {
                    DataType = "0",
                    Name = "layer_7",
                    Layer_ = "7"
                }
            },
            SkipText = true,
            SkipProperty = true,
            SkipBox = true,
            SkipNode = true,
            ConvertPathToPolygon = true,
            IndexIn = "",
            IndexOut = ""
        };

        using (var call = _client.LoadGds(loadGdsRequest))
        {
            var stream = call.ResponseStream;

            while (await stream.MoveNext())
            {
                var progress = stream.Current;
                Console.WriteLine(progress);
            }
        }
    }
}
