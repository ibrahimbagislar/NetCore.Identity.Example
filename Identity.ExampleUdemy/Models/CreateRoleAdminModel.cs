using System.ComponentModel.DataAnnotations;

namespace NetCore.Identity.Example.Models
{
    public class CreateRoleAdminModel
    {
        [Required(ErrorMessage = "Rol adı boş olamaz.")]
        public string Name { get; set; }
    }
}
