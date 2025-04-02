using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Web.Data.Entities;

namespace OmniPyme.Web.Data.Seeders
{
    public class SaleSeeder
    {

        private readonly DataContext _context;

        public SaleSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Sale> sales = new List<Sale>
            {
                new Sale
                {
                    SaleDate = DateTime.Parse("2025-01-15 10:25:00"),
                    SalePaymentMethod = "Tarjeta de crédito",
                    SaleTotal = 1258.50m,
                    IdClient = 1
                },
                new Sale
                {
                    SaleDate = DateTime.Parse("2025-01-28 14:40:00"),
                    SalePaymentMethod = "Efectivo",
                    SaleTotal = 875.25m,
                    IdClient = 2
                },
                new Sale
                {
                    SaleDate = DateTime.Parse("2025-02-05 09:10:00"),
                    SalePaymentMethod = "Transferencia bancaria",
                    SaleTotal = 3240.00m,
                    IdClient = 1
                },
                new Sale
                {
                    SaleDate = DateTime.Parse("2025-02-12 16:15:00"),
                    SalePaymentMethod = "PayPal",
                    SaleTotal = 950.75m,
                    IdClient = 3
                },
                new Sale
                {
                    SaleDate = DateTime.Parse("2025-02-27 11:35:00"),
                    SalePaymentMethod = "Tarjeta de débito",
                    SaleTotal = 1780.30m,
                    IdClient = 4
                },
                new Sale
                {
                    SaleDate = DateTime.Parse("2025-03-08 13:00:00"),
                    SalePaymentMethod = "Efectivo",
                    SaleTotal = 520.00m,
                    IdClient = 2
                },
                new Sale
                {
                    SaleDate = DateTime.Parse("2025-03-14 15:25:00"),
                    SalePaymentMethod = "Tarjeta de crédito",
                    SaleTotal = 2340.60m,
                    IdClient = 5
                },
                new Sale
                {
                    SaleDate = DateTime.Parse("2025-03-22 09:55:00"),
                    SalePaymentMethod = "Transferencia bancaria",
                    SaleTotal = 1450.25m,
                    IdClient = 3
                },
                new Sale
                {
                    SaleDate = DateTime.Parse("2025-03-29 17:10:00"),
                    SalePaymentMethod = "PayPal",
                    SaleTotal = 680.90m,
                    IdClient = 1
                },
                new Sale
                {
                    SaleDate = DateTime.Parse("2025-04-01 12:20:00"),
                    SalePaymentMethod = "Tarjeta de débito",
                    SaleTotal = 3125.45m,
                    IdClient = 4
                }
            };

            foreach (Sale sale in sales)
            {
                bool exists = await _context.Sales.AnyAsync(x => x.Id == sale.Id);

                if (!exists)
                {
                    await _context.Sales.AddAsync(sale);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
