using System.ComponentModel.DataAnnotations;

namespace Identity.ExampleUdemy.Models
{
    public class CreateUserModel
    {
        [Required(ErrorMessage ="Kullanıcı adı boş geçilemez!")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Email boş geçilemez!")]
        [EmailAddress(ErrorMessage ="Email formatı doğru değil.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Parola boş geçilemez!")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Parola boş geçilemez!")]
        [Compare("Password",ErrorMessage = "Parolalar eşleşmiyor.")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Cinsiyet boş geçilemez!")]
        public string Gender { get; set; }
    }
}
