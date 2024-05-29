using CoreIdentityComplete_0.Models.ViewModels.AppUsers;
using FluentValidation;

namespace CoreIdentityComplete_0.Models.FluentValidation
{
    public class UserSignInRequestModelValidator : AbstractValidator<UserSignInRequestModel>
    {
        public UserSignInRequestModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı ismi zorunludur");
            RuleFor(x => x.Password).MinimumLength(3).WithMessage("Test").NotEmpty().WithMessage("Sifre minimum 3 karakterli olmalıdır");
        }
    }
}
