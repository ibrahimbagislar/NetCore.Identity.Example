using Microsoft.AspNetCore.Identity;

namespace NetCore.Identity.Example.Data.Entites
{
    public class AppRole : IdentityRole<int>
    {
        public DateTime CreatedTime { get; set; }
    }
}
