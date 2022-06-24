﻿using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Account;

public class AccountApiServiceFixture : ApiServiceFixture
{
    public AccountApiServiceFixture() : base(
        new Dictionary<string, string>
        {
            {"ConnectionStrings:MessageBus", "ampq://"},
            {"JwtOption:SecretKey", "abcdef123455676785412312312312312"},
            {"JwtOption:Issuer", "SampleApiService"},
            {"JwtOption:Audience", "SampleUser"},
            {"JwtOption:DurationMinutes", "50000"},
            {"Validation:Account:User:IdMinimumLength", "5"},
            {"Validation:Account:User:IdMaximumLength", "15"},
            {"Validation:Account:User:PasswordMinimumLength", "8"},
            {"Validation:Account:User:PasswordMaximumRepeated", "3"},
            {"Lock:Timeout", "1000"},
        }
    )
    {
    }
}

[CollectionDefinition("AccountIntegrationTest")]
public class AccountServiceIntegrationTestFixtures : ICollectionFixture<AccountApiServiceFixture>
{   
}