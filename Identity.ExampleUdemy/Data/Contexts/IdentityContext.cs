    using NetCore.Identity.Example.Data.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NetCore.Identity.Example.Data.Contexts
{
    public class IdentityContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }
    }
}
