using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("WorkflowTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class WorkflowServiceTest
{
    private readonly WorkflowApiServiceFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly Domain.Workflow.Protos.V1.WorkflowService.WorkflowServiceClient _workflowServiceClient;
    private readonly Domain.Account.Protos.V1.SignInService.SignInServiceClient _signInServiceClient;
    public WorkflowServiceTest(WorkflowApiServiceFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _workflowServiceClient = _fixture.WorkflowService;
        _signInServiceClient = _fixture.SignInService;
    }
}