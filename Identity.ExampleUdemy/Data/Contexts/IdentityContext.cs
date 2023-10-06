    using Identity.ExampleUdemy.Data.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.ExampleUdemy.Data.Contexts
{
    public class IdentityContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }
    }
}
