using System.ComponentModel.DataAnnotations;

namespace NetCore.Identity.Example.Models
{
    public class ProfileInfoModel
    {
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email boş geçilemez!")]
        [EmailAddress(ErrorMessage = "Email formatı doğru değil.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Cinsiyet boş geçilemez!")]
        public string Gender { get; set; }
    }
}
