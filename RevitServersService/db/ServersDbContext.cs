using Microsoft.EntityFrameworkCore;

namespace RevitServersService.db
{
    public class ServersDbContext : DbContext
    {
        public DbSet<RSToParse> RevitServers { get; set; }
        public ServersDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
