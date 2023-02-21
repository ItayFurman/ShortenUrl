using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectUrlShort.Models;

namespace ProjectUrlShort.Data
{
    public class UrlDBcontext: IdentityDbContext
	{
        public UrlDBcontext (DbContextOptions<UrlDBcontext> options) :base(options) { }
        
        public virtual DbSet<URL> Urls { get; set; }
    }
}
