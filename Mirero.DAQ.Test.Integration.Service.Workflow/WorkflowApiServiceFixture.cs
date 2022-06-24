using Microsoft.Extensions.Logging;
using Mirero.DAQ.Test.Integration.Service.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Infrastructure.Redis;
using Mirero.DAQ.Test.Integration.Service.TestEnv;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow
{
    public class WorkflowApiServiceFixture : ApiServiceIntegrationTestFixture<TestStartup>
    {
        private ILogger _logger { get; set; }
        public CurrentTestUser CurrentTestUser { get; set; }
        public Connection RedisConnection { get; private set; }
        
        public Domain.Workflow.Protos.V1.ServerService.ServerServiceClient ServerService { get; private set; }
        public Domain.Workflow.Protos.V1.VolumeService.VolumeServiceClient VolumeService { get; private set; }
        public Domain.Workflow.Protos.V1.WorkerService.WorkerServiceClient WorkerService { get; private set; }
        public Domain.Workflow.Protos.V1.WorkflowService.WorkflowServiceClient WorkflowService { get; private set; }
        public Domain.Workflow.Protos.V1.JobService.JobServiceClient JobService { get; private set; }
        public Domain.Account.Protos.V1.SignInService.SignInServiceClient SignInService { get; private set; }
    
        public string VolumeBaseUri { get; private set; }
        public TestFileDataGenerator FileDataGenerator { get; private set; }

        public WorkflowApiServiceFixture() 
            : base( new Dictionary<string, string> { })
        {
            Init();
            InitService();
            InitEnv();
        }

        private void Init()
        {
            _logger = LoggerFactory.CreateLogger("WorkflowApiServiceFixture");
            VolumeBaseUri = Environment.GetEnvironmentVariable("DAQ60_TEST_VOLUME_BASE_DIR") ?? @"c:\mirero\volumes";
            FileDataGenerator = new TestFileDataGenerator(VolumeBaseUri);
        }

        private void InitService()
        {
            ServerService = new Domain.Workflow.Protos.V1.ServerService.ServerServiceClient(GrpcChannel);
            VolumeService = new Domain.Workflow.Protos.V1.VolumeService.VolumeServiceClient(GrpcChannel);
            WorkerService = new Domain.Workflow.Protos.V1.WorkerService.WorkerServiceClient(GrpcChannel);
            WorkflowService = new Domain.Workflow.Protos.V1.WorkflowService.WorkflowServiceClient(GrpcChannel);
            JobService = new Domain.Workflow.Protos.V1.JobService.JobServiceClient(GrpcChannel);
            SignInService = new SignInService.SignInServiceClient(GrpcChannel);
        }

        private void InitEnv()
        {
            //seed volume 위치
            if (!Directory.Exists(@"c:\mirero\volumes\TestVolume"))
            {
                Directory.CreateDirectory(@"c:\mirero\volumes\TestVolume");
            }
            if (!Directory.Exists(@"c:\mirero\volumes\TestVolume2"))
            {
                Directory.CreateDirectory(@"c:\mirero\volumes\TestVolume2");
            }
            
            //seed Worker consumer group
            RedisConnection = new Connection(new ConnectionConfig()
                { Uris = new[] { new Uri("redis://default:redispw@localhost:55005") } });

            var streamName = NameHandler.GetStartJobStreamName(JobType.Default);
            var notExistGroup = !RedisConnection.CreateDatabase().KeyExists(streamName)
                                || (RedisConnection.CreateDatabase().StreamGroupInfo(streamName)).All(x=>x.Name!=streamName);
            if (notExistGroup)
            {
                RedisConnection.CreateDatabase().StreamCreateConsumerGroup(
                    NameHandler.GetStartJobStreamName(JobType.Default)
                    , NameHandler.GetStartJobStreamName(JobType.Default)
                    , createStream: true);
            }
        }
        
        public async Task<bool> SignInAsync(string userId, string password, double lifeTimeHours = 24.0)
        {
            var request = new SignInRequest
            {
                UserId = userId,
                Password = password,
                Lifetime = Duration.FromTimeSpan(TimeSpan.FromHours(lifeTimeHours))
            };

            var done = false;
            CurrentTestUser = CurrentTestUser.InvalidUser;
           
            var response = await SignInService.SignInAsync(request);
            CurrentTestUser = new CurrentTestUser
            {
                Id = userId,
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
            };
            done = true;
            return done;
        }
    
        public Task<bool> SignOut()
        {
            CurrentTestUser = CurrentTestUser.InvalidUser;
            return Task.FromResult(true);
        }

        public CallOptions OptionsWithAuthHeader(CallOptions options = default)
        {
            if (CurrentTestUser == CurrentTestUser.InvalidUser)
            {
                return options;
            }

            var headers = new Metadata()
            {
                { "Authorization", $"Bearer {CurrentTestUser.AccessToken}" }
            };
            return options.WithHeaders(headers);
        }

    }
}

[CollectionDefinition("WorkflowTest")]
public class WorkflowServiceIntegrationTestFixtures 
    : ICollectionFixture<WorkflowApiServiceFixture>
{
}

public static class TimestampExtension
{
    public static DateTime TimestampToDateTime(this Timestamp timestamp)
    {
        var toDateTime = timestamp.ToDateTime();
        return new DateTime(toDateTime.Year, toDateTime.Month, toDateTime.Day, toDateTime.Hour, toDateTime.Minute,
            toDateTime.Second);
    }
}
