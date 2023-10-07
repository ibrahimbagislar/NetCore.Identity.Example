using System.ComponentModel.DataAnnotations;

namespace NetCore.Identity.Example.Models
{
    public class ConfirmMailViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Onay kodunu giriniz.")]
        public int ConfirmCode { get; set; }
        public string Email { get; set; }
    }
}
