using System.ComponentModel.DataAnnotations;

namespace NetCore.Identity.Example.Models
{
    public class SignInUserModel
    {
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Parola boş geçilemez!")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
