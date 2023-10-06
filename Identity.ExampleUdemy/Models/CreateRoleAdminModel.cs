using System.ComponentModel.DataAnnotations;

namespace Identity.ExampleUdemy.Models
{
    public class CreateRoleAdminModel
    {
        [Required(ErrorMessage = "Rol adı boş olamaz.")]
        public string Name { get; set; }
    }
}
