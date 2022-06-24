using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.ServerService
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Collection("WorkflowTest")]
    [TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
    public partial class ServerServiceTest
    {
        private readonly WorkflowApiServiceFixture _fixture;
        private readonly ITestOutputHelper _output;
        private readonly Domain.Workflow.Protos.V1.ServerService.ServerServiceClient _serverServiceClient;
       
        public ServerServiceTest(WorkflowApiServiceFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
            _serverServiceClient = _fixture.ServerService;
        }
    }
}
