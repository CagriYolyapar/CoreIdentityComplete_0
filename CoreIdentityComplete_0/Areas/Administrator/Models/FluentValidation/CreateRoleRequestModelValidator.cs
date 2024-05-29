using CoreIdentityComplete_0.Areas.Administrator.Models.AppRoles.RequestModels;
using FluentValidation;

namespace CoreIdentityComplete_0.Areas.Administrator.Models.FluentValidation
{
    public class CreateRoleRequestModelValidator:AbstractValidator<CreateRoleRequestModel>
    {
        public CreateRoleRequestModelValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Rol ismi bos gecilemez");
        }
    }
}
