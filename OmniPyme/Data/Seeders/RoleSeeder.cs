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
                    Name = "Administrador"
                },
                new Role
                {
                    Name = "Vendedor"
                },
                new Role
                {
                    Name = "Gerente"
                }
            };

            foreach (Role role in roles)
            {
                bool exists = await _context.Roles.AnyAsync(x => x.Name == role.Name);

                if (!exists)
                {
                    await _context.Roles.AddAsync(role);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}

