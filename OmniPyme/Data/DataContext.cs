using Microsoft.EntityFrameworkCore;
using OmniPyme.Web.Data.Entities;

namespace OmniPyme.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
