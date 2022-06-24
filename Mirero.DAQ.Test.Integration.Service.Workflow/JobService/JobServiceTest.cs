using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirero.DAQ.Infrastructure.Redis;
using Xunit;
using Xunit.Abstractions;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.JobService
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Collection("WorkflowTest")]
    [TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
    public partial class JobServiceTest
    {
        private readonly WorkflowApiServiceFixture _fixture;
        private readonly ITestOutputHelper _output;
        private readonly Domain.Workflow.Protos.V1.JobService.JobServiceClient _jobServiceClient;
        private readonly Domain.Account.Protos.V1.SignInService.SignInServiceClient _signInServiceClient;
        public JobServiceTest(WorkflowApiServiceFixture fixture
            , ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
            _jobServiceClient = fixture.JobService;
            _signInServiceClient = fixture.SignInService;
        }
    }
}
