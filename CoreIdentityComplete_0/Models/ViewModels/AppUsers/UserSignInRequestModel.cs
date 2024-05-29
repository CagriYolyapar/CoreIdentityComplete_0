using System.ComponentModel.DataAnnotations;

namespace CoreIdentityComplete_0.Models.ViewModels.AppUsers
{
    public class UserSignInRequestModel
    {
        //[Required(ErrorMessage ="Kullanıcı ismi zorunludur")]
        public string? UserName { get; set; }
        //[Required(ErrorMessage ="Sifre alanı zorunludur")]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }

    }
}
