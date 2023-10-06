using Microsoft.AspNetCore.Identity;

namespace Identity.ExampleUdemy.Data.Entites
{
    public class AppRole : IdentityRole<int>
    {
        public DateTime CreatedTime { get; set; }
    }
}
