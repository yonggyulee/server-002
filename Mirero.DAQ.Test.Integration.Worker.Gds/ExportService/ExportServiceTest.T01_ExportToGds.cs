using Grpc.Core;
using Mirero.DAQ.Domain.Gds.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1.Worker;
using System;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Worker.Gds.ExportService;
public partial class ExportServiceTest
{
    [Fact]
    public async void T01_ExportToGds()
    {
        var exportToGdsRequest = new ExportToGdsRequest
        {
            OutputUri = "/home/mirero/temp/testdata/export_to_gds.gds",
            Layers =
            {
                new Layer
                {
                    Layer_ = "1",
                    Name = "layer_1",
                    DataType = "0"
                },
                new Layer
                {
                    Layer_ = "2",
                    Name = "layer_2",
                    DataType = "0"
                }
            },
            SkipText = false,
            SkipProperty = false,
            SkipUndefCell = false
        };

        using (var call = _client.ExportToGds(exportToGdsRequest))
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