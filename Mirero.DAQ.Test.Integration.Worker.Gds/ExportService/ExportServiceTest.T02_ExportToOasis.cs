using Grpc.Core;
using Mirero.DAQ.Domain.Gds.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1.Worker;
using System;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Worker.Gds.ExportService;
public partial class ExportServiceTest
{
    [Fact]
    public async void T02_ExportToOasis()
    {
        var exportToOasisRequest = new ExportToOasisRequest
        {
            OutputUri = "/home/mirero/temp/testdata/test_result.oas",
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
            SkipText = true,
            SkipProperty = true,
            SkipUndefCell = true,
            UseCblock = true,
            UseRepeatBuilder = true,
            UseNameTableCblock = true

        };

        using (var call = _client.ExportToOasis(exportToOasisRequest))
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