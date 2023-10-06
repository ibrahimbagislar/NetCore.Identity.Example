using Microsoft.AspNetCore.Identity;

namespace Identity.ExampleUdemy.Data.Entites
{
    public class AppUser : IdentityUser<int>
    {
        public string ImagePath { get; set; }
        public string Gender { get; set; }
    }
}
