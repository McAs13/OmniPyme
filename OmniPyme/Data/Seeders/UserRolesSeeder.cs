using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Web.Core;
using OmniPyme.Web.Data.Entities;
using OmniPyme.Web.Services;
using System.Drawing.Text;
using System.Numerics;

namespace OmniPyme.Web.Data.Seeders
{
    public class UserRolesSeeder
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;

        public UserRolesSeeder(DataContext context, IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        public async Task SeedAsync()
        {
            await CheckRoles();
            await BasicRoleAsync();
            await CheckUsers();

        }

        private async Task BasicRoleAsync()
        {
            bool exists = await _context.PrivateURoles.AnyAsync(r => r.Name == "Basic");

            if (!exists)
            {
                PrivateURole role = new PrivateURole { Name = "Basic" };
                await _context.PrivateURoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckUsers()
        {
            Users? users = await _usersService.GetUserAsync("manuel@yopmail.com");

            if (users is null)
            {
                PrivateURole adminRole = await _context.PrivateURoles.FirstOrDefaultAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

                users = new Users
                {
                    Email = "manuel@yopmail.com",
                    FirstName = "Manuel",
                    LastName = "Moreno",
                    PhoneNumber = "300000000",
                    UserName = "manuel@yopmail.com",
                    Document = "11111122",
                    PrivateURole = adminRole
                };

                await _usersService.AddUserAsync(users, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(users);
                await _usersService.ConfirmEmailAsync(users, token);

            }
            // Content Manager
            users = await _usersService.GetUserAsync("Luisa@yopmail.com");

            if (users is null)
            {
                PrivateURole contentManagerRole = await _context.PrivateURoles.FirstOrDefaultAsync(r => r.Name == "Gestor de contenido");

                users = new Users
                {
                    Email = "Luisa@yopmail.com",
                    FirstName = "Luisa",
                    LastName = "Perez",
                    PhoneNumber = "3454657",
                    UserName = "Luisa@yopmail.com",
                    Document = "1324454",
                    PrivateURole = contentManagerRole
                };

                await _usersService.AddUserAsync(users, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(users);
                await _usersService.ConfirmEmailAsync(users, token);

            }
            // basic
            users = await _usersService.GetUserAsync("prueba@yopmail.com");

            if (users is null)
            {
                PrivateURole basicRole = await _context.PrivateURoles.FirstOrDefaultAsync(r => r.Name == "Basic");

                users = new Users
                {
                    Email = "prueba@yopmail.com",
                    FirstName = "prueba",
                    LastName = "prueba",
                    PhoneNumber = "3454657",
                    UserName = "prueba@yopmail.com",
                    Document = "176767565657",
                    PrivateURole = basicRole
                };

                await _usersService.AddUserAsync(users, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(users);
                await _usersService.ConfirmEmailAsync(users, token);

            }
        }
        private async Task CheckRoles()
        {
            await AdminRolesAsync();
            await ContentManagerRoleAsync();
        }

        private async Task ContentManagerRoleAsync()
        {
            bool exists = await _context.PrivateURoles.AnyAsync(r => r.Name == "Gestor de contenido");

            if (!exists)
            {
                PrivateURole role = new PrivateURole { Name = "Gestor de contenido" };
                await _context.PrivateURoles.AddAsync(role);

                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Clientes" || p.Module == "Product")
                                                                         .ToListAsync();


                foreach (Permission permission in permissions)
                {

                    await _context.RolePermissions.AddAsync(new RolePermission { Permission = permission, Role = role });

                }

                await _context.SaveChangesAsync();
            }
        }

        private async Task AdminRolesAsync()
        {
            bool exists = await _context.PrivateURoles.AnyAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

            if (!exists)
            {
                PrivateURole role = new PrivateURole { Name = Env.SUPER_ADMIN_ROLE_NAME };
                await _context.PrivateURoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }
    }
}
