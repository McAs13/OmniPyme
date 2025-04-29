using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using OmniPyme.Web.Core;
using OmniPyme.Web.Core.Pagination;
using OmniPyme.Web.DTOs;
using OmniPyme.Web.Services;

namespace OmniPyme.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly INotyfService _notyfService;

        public RolesController(IRolesService rolesService, INotyfService notyfService)
        {
            _rolesService = rolesService;
            _notyfService = notyfService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {

            // _notyfService.Information("It Works!");

            Response<PaginationResponse<RoleDTO>> response = await _rolesService.GetPaginationAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            Response<RoleDTO> response = await _rolesService.CreateAsync(dto);

            if (response.IsSuccess)
            {
                _notyfService.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _notyfService.Error(response.Message);
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<RoleDTO> response = await _rolesService.GetOneAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            _notyfService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            Response<RoleDTO> response = await _rolesService.EditAsync(dto);

            if (response.IsSuccess)
            {
                _notyfService.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _notyfService.Error(response.Message);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<object> response = await _rolesService.DeleteAsync(id);

            if (response.IsSuccess)
            {
                _notyfService.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _notyfService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Response<RoleDTO> response = await _rolesService.GetOneAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            _notyfService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
