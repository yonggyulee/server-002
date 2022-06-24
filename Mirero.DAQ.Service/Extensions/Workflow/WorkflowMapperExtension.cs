using System.Linq.Dynamic.Core;
using Google.Protobuf;
using Mapster;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using ServerEntity = Mirero.DAQ.Domain.Workflow.Entities.Server;
using VolumeEntity = Mirero.DAQ.Domain.Workflow.Entities.Volume;
using WorkerEntity = Mirero.DAQ.Domain.Workflow.Entities.Worker;
using WorkflowEntity = Mirero.DAQ.Domain.Workflow.Entities.Workflow;
using WorkflowVersionEntity = Mirero.DAQ.Domain.Workflow.Entities.WorkflowVersion;
using BatchJobEntity = Mirero.DAQ.Domain.Workflow.Entities.BatchJob;
using JobEntity = Mirero.DAQ.Domain.Workflow.Entities.Job;
using Google.Protobuf.WellKnownTypes;

namespace Mirero.DAQ.Service.Extensions.Workflow
{
    public static class WorkflowMapperExtension
    {
        public static TypeAdapterConfig AddWorkflowMapperConfig(this TypeAdapterConfig config)
        {
            _AddVolumeMapperConfig(config);
            _AddServerMapperConfig(config);
            _AddWorkerMapperConfig(config);
            _AddWorkflowMapperConfig(config);
            _AddWorkflowVersionMapperConfig(config);
            _AddBatchJobMapperConfig(config);
            _AddJobMapperConfig(config);

            return config;
        }

        private static void _AddVolumeMapperConfig(TypeAdapterConfig config)
        {
            config
                .NewConfig<VolumeEntity, Volume>();

            config
                .NewConfig<CreateVolumeRequest, VolumeEntity>();

            config
                .NewConfig<UpdateVolumeRequest, VolumeEntity>();
                
            config
                .NewConfig<(ListVolumesRequest Request
                        , IEnumerable<Volume> Volumes
                        , int Count)
                    , ListVolumesResponse>()
                .ConstructUsing(src => new ListVolumesResponse
                {
                    PageResult = new PageResult
                    {
                        PageIndex = src.Request.QueryParameter.PageIndex,
                        PageSize = src.Request.QueryParameter.PageSize,
                        Count = src.Count
                    },
                    Volumes = { src.Volumes }
                });
        }

        private static void _AddServerMapperConfig(TypeAdapterConfig config)
        {
            config
                .NewConfig<ServerEntity, Server>();
                // .Map(dest => dest.Address, src => src.Address.ToString());

            config
                .NewConfig<CreateServerRequest, ServerEntity>();
                // .Map(dest => dest.Address, src => IPAddress.Parse(src.Address));

            config
                .NewConfig<UpdateServerRequest, ServerEntity>();
                
            config
                .NewConfig<(ListServersRequest Request
                        , IEnumerable<Server> Servers
                        , int Count)
                    , ListServersResponse>()
                .ConstructUsing(src => new ListServersResponse
                {
                    PageResult = new PageResult
                    {
                        PageIndex = src.Request.QueryParameter.PageIndex,
                        PageSize = src.Request.QueryParameter.PageSize,
                        Count = src.Count
                    },
                    Servers = { src.Servers }
                });
        }

        private static void _AddWorkerMapperConfig(TypeAdapterConfig config)
        {
            config
                .NewConfig<WorkerEntity, Worker>();

            config
                .NewConfig<CreateWorkerRequest, WorkerEntity>();

            config
                .NewConfig<(ListWorkersRequest Request
                        , IEnumerable<Worker> Workers
                        , int Count)
                    , ListWorkersResponse>()
                .ConstructUsing(src => new ListWorkersResponse
                {
                    PageResult = new PageResult
                    {
                        PageIndex = src.Request.QueryParameter.PageIndex,
                        PageSize = src.Request.QueryParameter.PageSize,
                        Count = src.Count
                    },
                    Workers = { src.Workers }
                });
        }

        private static void _AddWorkflowMapperConfig(TypeAdapterConfig config)
        {
            config
                .NewConfig<WorkflowEntity, Domain.Workflow.Protos.V1.Workflow>()
                .Map(dest => dest.CreateDate, src => DateTimeToTimestamp(src.CreateDate))
                .Map(dest => dest.UpdateDate, src => DateTimeToTimestamp(src.UpdateDate));

            config
                .NewConfig<CreateWorkflowRequest, WorkflowEntity>();

            config
                .NewConfig<UpdateWorkflowRequest, WorkflowEntity>();

            config
                .NewConfig<(ListWorkflowsRequest Request
                        , IEnumerable<Domain.Workflow.Protos.V1.Workflow> Workflows
                        , int Count)
                    , ListWorkflowsResponse>()
                .ConstructUsing(src => new ListWorkflowsResponse 
                {
                    PageResult = new PageResult
                    {
                        PageIndex = src.Request.QueryParameter.PageIndex,
                        PageSize = src.Request.QueryParameter.PageSize,
                        Count = src.Count
                    },
                    Workflows = { src.Workflows }
                });
        }

        private static void _AddWorkflowVersionMapperConfig(TypeAdapterConfig config)
        {
            config
                .NewConfig<WorkflowVersionEntity, WorkflowVersion>()
                .Map(dest => dest.CreateDate, src => DateTimeToTimestamp(src.CreateDate))
                .Map(dest => dest.UpdateDate, src => DateTimeToTimestamp(src.UpdateDate));

            config
                .NewConfig<CreateWorkflowVersionRequest, WorkflowVersionEntity>();
            
            config
                .NewConfig<UpdateWorkflowVersionRequest, WorkflowVersionEntity>();

            config
                .NewConfig<(ListWorkflowVersionsRequest Request
                        , IEnumerable<WorkflowVersion> WorkflowVersions
                        , int Count)
                    , ListWorkflowVersionsResponse>()
                .ConstructUsing(src => new ListWorkflowVersionsResponse
                {
                    PageResult = new PageResult
                    {
                        PageIndex = src.Request.QueryParameter.PageIndex,
                        PageSize = src.Request.QueryParameter.PageSize,
                        Count = src.Count
                    },
                    WorkflowVersions = { src.WorkflowVersions }
                });
        }

        private static void _AddBatchJobMapperConfig(TypeAdapterConfig config)
        {
            config
                .NewConfig<BatchJobEntity, BatchJob>()
                .Map(dest => dest.RegisterDate, src => DateTimeToTimestamp(src.RegisterDate))
                .Map(dest => dest.StartDate, src => DateTimeToTimestamp(src.StartDate))
                .Map(dest => dest.EndDate, src => DateTimeToTimestamp(src.EndDate));

            config
                .NewConfig<CreateBatchJobRequest, BatchJobEntity>();

            config
                .NewConfig<(ListBatchJobsRequest Request
                        , IEnumerable<BatchJob> BatchJobs
                        , int Count)
                    , ListBatchJobsResponse>()
                .ConstructUsing(src => new ListBatchJobsResponse
                {
                    PageResult = new PageResult
                    {
                        PageIndex = src.Request.QueryParameter.PageIndex,
                        PageSize = src.Request.QueryParameter.PageSize,
                        Count = src.Count
                    },
                    BatchJobs = { src.BatchJobs }
                });
        }

        private static void _AddJobMapperConfig(TypeAdapterConfig config)
        {
            config
                .NewConfig<JobEntity, Job>()
                .Map(dest => dest.RegisterDate, src => DateTimeToTimestamp(src.RegisterDate))
                .Map(dest => dest.StartDate, src => DateTimeToTimestamp(src.StartDate))
                .Map(dest => dest.EndDate, src => DateTimeToTimestamp(src.EndDate))
                .Map(dest => dest.Parameter, src => ByteString.CopyFrom(src.Parameter));

            config
                .NewConfig<StartJobRequest, JobEntity>()
                .Map(dest => dest.Id, src => src.JobId)
                .Map(dest => dest.Type, src => src.JobType)
                .Map(dest => dest.Parameter, src => src.Parameter.ToByteArray());

            config
                .NewConfig<(ListJobsRequest Request
                        , IEnumerable<Job> Jobs
                        , int Count
                        , bool IsCompleted)
                    , ListJobsResponse>()
                .ConstructUsing(src => new ListJobsResponse
                {
                    PageResult = new PageResult
                    {
                        PageIndex = src.Request.QueryParameter.PageIndex,
                        PageSize = src.Request.QueryParameter.PageSize,
                        Count = src.Count
                    },
                    Jobs = { src.Jobs },
                    IsCompleted = src.IsCompleted
                });
        }
        
        private static Timestamp? DateTimeToTimestamp(DateTime? dateTime)
        {
            return dateTime?.ToTimestamp();
        }
    }
}
