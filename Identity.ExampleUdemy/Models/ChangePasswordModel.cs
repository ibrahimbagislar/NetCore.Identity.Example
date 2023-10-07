using System.ComponentModel.DataAnnotations;

namespace NetCore.Identity.Example.Models
{
    public class ChangePasswordModel
    {

        [Required(ErrorMessage = "Parola boş geçilemez!")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Parola boş geçilemez!")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Parola boş geçilemez!")]
        [Compare("NewPassword", ErrorMessage = "Parolalar eşleşmiyor.")]
        public string NewConfirmPassword { get; set; }
        public bool IsCurrentPasswordMatch()
        {
            // Eğer mevcut şifre ve yeni şifre aynı ise true döndürür
            return string.Equals(CurrentPassword, NewPassword);
        }
    }
}
