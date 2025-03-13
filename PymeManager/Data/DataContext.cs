using Microsoft.EntityFrameworkCore;
using PymeManager.Data.Entities;

namespace PymeManager.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Person> Personas { get; set; }
    }
}
