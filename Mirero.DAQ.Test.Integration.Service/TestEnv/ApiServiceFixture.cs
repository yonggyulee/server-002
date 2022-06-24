using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Gds.Protos.V1.Worker;
using Mirero.DAQ.Test.Integration.Service.TestEnv;

namespace Mirero.DAQ.Test.Integration.Service;

public class ApiServiceFixture : ApiServiceIntegrationTestFixture<TestStartup>
{
    public ApiServiceFixture(Dictionary<string, string> configuration ) : base(configuration)
    {
        Init();
    }

    private void Init()
    {
        Logger = this.LoggerFactory.CreateLogger("ApiServiceFixture");
        VolumeBaseUri = Environment.GetEnvironmentVariable("DAQ60_TEST_VOLUME_BASE_DIR") ?? "c:/mirero/volumes";
        if (!Directory.Exists(VolumeBaseUri))
        {
            Directory.CreateDirectory(VolumeBaseUri);
        }

        _testFileDataGenerator = new TestFileDataGenerator(VolumeBaseUri);
        _testFileDataGenerator.GenerateTestFileData();
    }

    public string VolumeBaseUri { get; set; }

    #region Account
    public SignInService.SignInServiceClient SignInService => CreateSignInService();
    public GroupService.GroupServiceClient GroupService => CreateGroupService();
    public UserService.UserServiceClient UserService => CreateUserService();
    #endregion
    
    private  GroupService.GroupServiceClient CreateGroupService()
    {
        return new GroupService.GroupServiceClient(GrpcChannel);
    }
    
    private  SignInService.SignInServiceClient CreateSignInService()
    {
        return new SignInService.SignInServiceClient(GrpcChannel);
    }
    
    private  UserService.UserServiceClient CreateUserService()
    {
        return new UserService.UserServiceClient(GrpcChannel);
    }

    public CurrentTestUser CurrentTestUser { get; set; }
    
    private ILogger Logger { get; set; }

    private TestFileDataGenerator _testFileDataGenerator;

    public TestSignInContext CreateSignInContext()
    {
        return new TestSignInContext(this);
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
        try
        {
            var response = await SignInService.SignInAsync(request);
            this.CurrentTestUser = new CurrentTestUser
            {
                Id = userId,
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
            };
            done = true;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "로그인 실패");
            //throw new Exception("login fail");
        }

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

    public override void Dispose()
    {
        _testFileDataGenerator.RemoveTestFileData();
        base.Dispose();
    }
}