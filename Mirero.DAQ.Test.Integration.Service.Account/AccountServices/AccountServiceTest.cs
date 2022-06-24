using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Abstractions;
using GroupService = Mirero.DAQ.Domain.Account.Protos.V1.GroupService;
using SignInService = Mirero.DAQ.Domain.Account.Protos.V1.SignInService;
using UserService = Mirero.DAQ.Domain.Account.Protos.V1.UserService;

namespace Mirero.DAQ.Test.Integration.Service.Account.AccountServices;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[Collection("AccountIntegrationTest")]
[TestCaseOrderer("AlphabeticalOrderer", "Mirero.DAQ.Test.Integration.Service")]
public partial class AccountServiceTest
{
    private readonly ApiServiceFixture _fixture;
    private readonly ITestOutputHelper _output;
    private readonly SignInService.SignInServiceClient  _signInServiceClient;
    private readonly GroupService.GroupServiceClient _groupServiceClient;
    private readonly UserService.UserServiceClient _userServiceClient;

    public AccountServiceTest(AccountApiServiceFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _groupServiceClient = _fixture.GroupService;
        _signInServiceClient = _fixture.SignInService;
        _userServiceClient = _fixture.UserService;
    }
}