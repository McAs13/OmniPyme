using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;

namespace OmniPyme.Web.Helpers
{
    public interface ICombosHelper
    {
        public Task<IEnumerable<SelectListItem>> GetComboCliente(int selectedId = 0);
    }

    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCliente(int selectedId = 0)
        {
            List<SelectListItem> list = await _context.Clients
                .Select(c => new SelectListItem
                {
                    Text = $"{c.FirstName} {c.LastName}",
                    Value = c.Id.ToString(),
                    Selected = c.Id == selectedId
                }).ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un cliente...]",
                Value = "0"
            });
            return list;
        }

    }
}
