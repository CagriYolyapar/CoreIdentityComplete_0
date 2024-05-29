using System.ComponentModel.DataAnnotations;

namespace CoreIdentityComplete_0.Areas.Administrator.Models.AppRoles.RequestModels
{
    public class CreateRoleRequestModel
    {
        //[Required(ErrorMessage ="Rol ismi gereklidir")]
        public string? RoleName { get; set; }

    }
}
