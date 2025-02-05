using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace devGamesAPI.Data
{
    public class DevGamesContext : IdentityDbContext
    {
        public DevGamesContext(DbContextOptions<DevGamesContext> options) : base(options)
        {
        }

        //Sobrescrever o método OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
