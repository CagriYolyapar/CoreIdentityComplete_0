using CoreIdentityComplete_0.Areas.Administrator.Models.AppUsers;
using FluentValidation;

namespace CoreIdentityComplete_0.Areas.Administrator.Models.FluentValidation
{
    public class CreateUserRequestModelValidator : AbstractValidator<CreateUserRequestModel>
    {
        public CreateUserRequestModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı ismi bos gecilemez");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email formatında giriş yapınız").NotEmpty().WithMessage("Email alanı bos gecilemez");
        }
    }
}
