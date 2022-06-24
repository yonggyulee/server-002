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
    public async void T02_LoadOasis()
    {
        var loadOasisRequest = new LoadOasisRequest
        {
            Uri = "/home/mirero/temp/testdata/demo5.oas",
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
            ConvertPathToPolygon = true,
            MaxPolygonPointNum = 10,
            IndexIn = "",
            IndexOut = ""
        };

        using (var call = _client.LoadOasis(loadOasisRequest))
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
