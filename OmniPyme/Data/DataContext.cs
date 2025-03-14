using Microsoft.EntityFrameworkCore;
using OmniPyme.Data.Entities;

namespace OmniPyme.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Person> Personas { get; set; }
    }
}
