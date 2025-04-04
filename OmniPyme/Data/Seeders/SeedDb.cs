﻿using OmniPyme.Data;

namespace OmniPyme.Web.Data.Seeders
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await new ClientSeeder(_context).SeedAsync();
        }
    }
}
