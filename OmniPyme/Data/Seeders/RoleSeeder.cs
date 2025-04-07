using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Web.Data.Entities;

namespace OmniPyme.Web.Data.Seeders
{
    public class RoleSeeder
    {


        private readonly DataContext _context;

        public RoleSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Role> roles = new List<Role>
            {
                new Role
                {
                    RolName = "Administrador"
                },
                new Role
                {
                    RolName = "Vendedor"
                },
                new Role
                {
                    RolName = "Gerente"
                }
            };

            foreach (Role role in roles)
            {
                bool exists = await _context.Roles.AnyAsync(x => x.Id == role.Id);

                if (!exists)
                {
                    await _context.Roles.AddAsync(role);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
