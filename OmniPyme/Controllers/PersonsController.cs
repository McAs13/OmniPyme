using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OmniPyme.Web.Core;
using OmniPyme.Web.DTOs;
using OmniPyme.Web.Services;

namespace OmniPyme.Web.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;

        public PersonsController(IPersonsService personsService)
        {
            _personsService = personsService;
        }

        public async Task<IActionResult> Index()
        {
            Response<List<PersonDTO>> response = await _personsService.GetListAsync();
            return View(response.Result);
        }
    }
}
