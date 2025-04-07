using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Web.Data.Entities;

namespace OmniPyme.Web.Data.Seeders
{
    public class SaleDetailSeeder
    {
        private readonly DataContext _context;

        public SaleDetailSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<SaleDetail> saleDetails = new List<SaleDetail>
            {
                new SaleDetail
                {
                    SaleDetailCode = "V001-1",
                    SaleDetailProductQuantity = 2,
                    SaleDetailProductPrice = 500.00m,
                    SaleDetailSubtotal = 1000.00m,
                    SaleDetailProductCode = 1,
                    IdSale = 1
                },
                new SaleDetail
                {
                    SaleDetailCode = "V001-2",
                    SaleDetailProductQuantity = 1,
                    SaleDetailProductPrice = 258.50m,
                    SaleDetailSubtotal = 258.50m,
                    SaleDetailProductCode = 2,
                    IdSale = 1
                },
                new SaleDetail
                {
                    SaleDetailCode = "V002-1",
                    SaleDetailProductQuantity = 3,
                    SaleDetailProductPrice = 291.75m,
                    SaleDetailSubtotal = 875.25m,
                    SaleDetailProductCode = 3,
                    IdSale = 2
                },
                new SaleDetail
                {
                    SaleDetailCode = "V003-1",
                    SaleDetailProductQuantity = 2,
                    SaleDetailProductPrice = 1620.00m,
                    SaleDetailSubtotal = 3240.00m,
                    SaleDetailProductCode = 1,
                    IdSale = 3
                },
                new SaleDetail
                {
                    SaleDetailCode = "V004-1",
                    SaleDetailProductQuantity = 1,
                    SaleDetailProductPrice = 950.75m,
                    SaleDetailSubtotal = 950.75m,
                    SaleDetailProductCode = 2,
                    IdSale = 4
                },
                new SaleDetail
                {
                    SaleDetailCode = "V005-1",
                    SaleDetailProductQuantity = 4,
                    SaleDetailProductPrice = 445.08m,
                    SaleDetailSubtotal = 1780.32m,
                    SaleDetailProductCode = 3,
                    IdSale = 5
                },
                new SaleDetail
                {
                    SaleDetailCode = "V006-1",
                    SaleDetailProductQuantity = 1,
                    SaleDetailProductPrice = 520.00m,
                    SaleDetailSubtotal = 520.00m,
                    SaleDetailProductCode = 1,
                    IdSale = 6
                }
            };

            foreach (SaleDetail detail in saleDetails)
            {
                bool exists = await _context.SaleDetails.AnyAsync(sd =>
                    sd.SaleDetailCode == detail.SaleDetailCode);

                if (!exists)
                {
                    await _context.SaleDetails.AddAsync(detail);
                }

                await _context.SaveChangesAsync();

            }
        }
    }
}
