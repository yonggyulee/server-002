using Google.Protobuf.WellKnownTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.UpdateBatchJob
{
    public class UpdateBatchJobCommand : IRequest<Empty>
    {
        public string BatchJobId { get; set; }

        public UpdateBatchJobCommand(string batchJobId)
        {
            BatchJobId = batchJobId;
        }
    }
}
