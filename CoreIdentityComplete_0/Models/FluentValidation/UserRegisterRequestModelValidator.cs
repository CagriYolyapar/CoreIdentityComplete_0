using CoreIdentityComplete_0.Models.ViewModels.AppUsers;
using FluentValidation;

namespace CoreIdentityComplete_0.Models.FluentValidation
{
    public class UserRegisterRequestModelValidator : AbstractValidator<UserRegisterRequestModel>
    {
        public UserRegisterRequestModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName alanı gereklidir");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(3).WithMessage("Minimum 3 karakter gecerlidir");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Sifreler uyusmuyor");
            //RuleFor(x => x.Email).EmailAddress().NotEmpty().WithMessage("Email giriniz").When(x => x.UserName == null).MinimumLength(3).WithMessage("Karakter");
            RuleFor(x => x.Email).EmailAddress().NotEmpty().WithMessage("Email giriniz").When(x => x.UserName == null).WithMessage("Karakter");


        }
    }
}
