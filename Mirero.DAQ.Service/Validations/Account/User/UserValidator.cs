using FluentValidation;
using Microsoft.Extensions.Configuration;
using User = Mirero.DAQ.Domain.Account.Protos.V1.User;

namespace Mirero.DAQ.Service.Validations.Account;

public class UserValidator : BaseValidator<User>
{
    private readonly int _idMinimumLength;
    private readonly int _idMaximumLength;
    private readonly int _passwordMinimumLength;
    private readonly int _maximumPasswordRepeated;
    
    public UserValidator(IConfiguration configuration) : base(configuration)
    {
        _idMinimumLength = Math.Max(4, Configuration.GetValue<int>("Validation:Account:User:IdMinimumLength"));
        _idMaximumLength = Math.Min(30, Configuration.GetValue<int>("Validation:Account:User:IdMaximumLength"));
        _passwordMinimumLength = Math.Max(4, Configuration.GetValue<int>("Validation:Account:User:PasswordMinimumLength"));
        _maximumPasswordRepeated = Math.Min(20, Configuration.GetValue<int>("Validation:Account:User:PasswordMaximumRepeated"));

        _Initialize();
    }

    private void _Initialize()
    {
        RuleFor(u => u.Id).Length(_idMinimumLength, _idMaximumLength);
        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(_passwordMinimumLength)
            .Matches(@"[A-Z]+").WithMessage($"{nameof(User.Password)}는 최소한 한 글자 이상 대문자가 포함되어야 합니다.")
            .Matches(@"[a-z]+").WithMessage($"{nameof(User.Password)}는 최소한 한 글자 이상 소문자가 포함되어야 합니다.")
            .Matches(@"[0-9]+").WithMessage($"{nameof(User.Password)}는 최소한 한 개 이상 숫자가 포함되어야 합니다.")
            .Matches(@"[\!\?\*\.]+").WithMessage($"{nameof(User.Password)}는 최소한 한 개 이상 특수문자(! ? * .)가 포함되어야 합니다.") 
            .Must(_CheckPassword).WithMessage($"{nameof(User.Password)}는 같은 문자가 {_maximumPasswordRepeated}번 이상 반복되면 안됩니다");
    }

    private bool _CheckPassword(string password)
    {
        for (int i = 0; i < password.Length - (_maximumPasswordRepeated - 1); i++)
        {
            if ((password[i] == password[i + 1]) && (password[i + 1] == password[i + 2]))
                return false;
        }

        return true;
    }
}