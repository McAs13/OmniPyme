using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OmniPyme.Web.Core;
using OmniPyme.Web.Core.Attributes;
using OmniPyme.Web.Core.Pagination;
using OmniPyme.Web.Data.Entities;
using OmniPyme.Web.DTOs;
using OmniPyme.Web.Helpers;
using OmniPyme.Web.Services;


namespace OmniPyme.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly INotyfService _notifyService;
        private readonly ICombosHelper _combosHelper;
        private readonly IMapper _mapper;

        public UsersController(IUsersService sectionsService, INotyfService notifyService, ICombosHelper combosHelper, IMapper mapper)
        {
            _usersService = sectionsService;
            _notifyService = notifyService;
            _combosHelper = combosHelper;
            _mapper = mapper;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showUsers", module: "Usuarios")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<UsersDTO>> response = await _usersService.GetPaginationAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createUsers", module: "Usuarios")]
        public async Task<IActionResult> Create()
        {
            IEnumerable<SelectListItem> items = await _combosHelper.GetComboRoles();

            UsersDTO dto = new UsersDTO
            {
                privateURoles = items,
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createUsers", module: "Usuarios")]
        public async Task<IActionResult> Create(UsersDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                dto.privateURoles = await _combosHelper.GetComboRoles();
                return View(dto);
            }

            Response<UsersDTO> response = await _usersService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                dto.privateURoles = await _combosHelper.GetComboRoles();
                return View(dto);
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        [CustomAuthorize(permission: "updateUsers", module: "Usuarios")]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (Guid.Empty.Equals(id))
            {
                return NotFound();
            }
            Users user = await _usersService.GetUserAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            UsersDTO dto = _mapper.Map<UsersDTO>(user);
            dto.privateURoles = await _combosHelper.GetComboRoles();

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateUsers", module: "Usuarios")]
        public async Task<IActionResult> Edit(UsersDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                dto.privateURoles = await _combosHelper.GetComboRoles();
                return View(dto);
            }

            Response<UsersDTO> response = await _usersService.UpdateUserAsync(dto);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                dto.privateURoles = await _combosHelper.GetComboRoles();
                return View(dto);
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));
            /**/
        }


    }
}