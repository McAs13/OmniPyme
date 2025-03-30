using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Web.Data.Entities;

namespace OmniPyme.Web.Data.Seeders
{
    public class ClientSeeder
    {
        private readonly DataContext _context;

        public ClientSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Client> clients = new List<Client>
            {
                new Client
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "johndoe@correo.com",
                    Phone = "1234567890",
                    RegisterDate = DateTime.Now,
                    LastPurchaseDate = DateTime.Now
                },
                new Client
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = "janeDoe@correo.com",
                    Phone = "1234567890",
                    RegisterDate = DateTime.Now,
                    LastPurchaseDate = DateTime.Now
                }
                // Add more clients here

                //TODO: Agregar un atributo cedula a la entidad Client
            };

            foreach (Client client in clients)
            {
                bool exists = await _context.Clients.AnyAsync(c => c.Email == client.Email);

                if (!exists)
                {
                    await _context.Clients.AddAsync(client);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
