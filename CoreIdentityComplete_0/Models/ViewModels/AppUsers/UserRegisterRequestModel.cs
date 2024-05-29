using System.ComponentModel.DataAnnotations;

namespace CoreIdentityComplete_0.Models.ViewModels.AppUsers
{
    public class UserRegisterRequestModel
    {
        //[Required(ErrorMessage ="Kullanıcı ismi girilmesi zorunludur")]
        public string? UserName { get; set; }

        //[EmailAddress(ErrorMessage ="Lutfen email formatında giriş yapınız")]
        public string? Email { get; set; }

        //[Required(ErrorMessage ="Sifre alanı girilmesi zorunludur")]
        //[MinLength(3,ErrorMessage ="Minimum 3 karakger girmelisiniz")]
        public string? Password { get; set; }

        //[Compare("Password",ErrorMessage ="Sifreler uyusmuyor")]
        public string? ConfirmPassword { get; set; }

    }
}
