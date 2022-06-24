using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.VolumeService
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Collection("WorkflowTest")]
    [TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
    public partial class VolumeServiceTest
    {
        private readonly WorkflowApiServiceFixture _fixture;
        private readonly ITestOutputHelper _output;
        private readonly Domain.Workflow.Protos.V1.VolumeService.VolumeServiceClient _volumeServiceClient;
        public VolumeServiceTest(WorkflowApiServiceFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
            _volumeServiceClient = _fixture.VolumeService;
        }
    }

    //1. create & get
    //2. list & get & update 
    //3. create & list & get & delete
}
