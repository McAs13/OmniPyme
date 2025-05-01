using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OmniPyme.Web.Data.Entities;
using OmniPyme.Data;

namespace OmniPyme.Data
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            // ⚠️ Usa tu cadena real de conexión aquí
            optionsBuilder.UseSqlServer("Server=DESKTOP-JNP1284\\SQLEXPRESS;Database=OmniPyme;User Id=omny;Password=D.odema18*;encrypt=false");

            return new DataContext(optionsBuilder.Options);
        }
    }
}
