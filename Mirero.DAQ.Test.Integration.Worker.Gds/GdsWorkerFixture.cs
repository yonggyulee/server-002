using System.Collections.Generic;
using Mirero.DAQ.Test.Integration.Service;
using Xunit;
using Mirero.DAQ.Domain.Gds.Protos.V1.Worker;

namespace Mirero.DAQ.Test.Integration.Worker.Gds;

public class GdsWorkerFixture : ApiServiceFixture
{
    public Mirero.DAQ.Domain.Gds.Protos.V1.Worker.GdsService.GdsServiceClient GdsWorkerGdsService => new(GrpcChannel);
    public Mirero.DAQ.Domain.Gds.Protos.V1.Worker.ClipService.ClipServiceClient GdsWorkerClipService => new (GrpcChannel);
    public Mirero.DAQ.Domain.Gds.Protos.V1.Worker.ExportService.ExportServiceClient GdsWorkerExportService => new (GrpcChannel);
    
    public GdsWorkerFixture() : base(
        new Dictionary<string, string>
        {
            { "ConnectionStrings:MessageBus", "ampq://" },
            { "JwtOption:SecretKey", "abcdef123455676785412312312312312" },
            { "JwtOption:Issuer", "SampleApiService" },
            { "JwtOption:Audience", "SampleUser" },
            { "JwtOption:DurationMinutes", "50000" },
            { "Validation:Account:User:IdMinimumLength", "5" },
            { "Validation:Account:User:IdMaximumLength", "15" },
            { "Validation:Account:User:PasswordMinimumLength", "8" },
            { "Validation:Account:User:PasswordMaximumRepeated", "3" },
            { "Lock:Timeout", "1000" },
        }
    )
    {
    }
}

[CollectionDefinition("UpdateIntegrationTest")]
public class GdsWorkerIntegrationTestFixtures : ICollectionFixture<GdsWorkerFixture>
{
}