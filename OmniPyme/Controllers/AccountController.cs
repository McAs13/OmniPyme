using Microsoft.AspNetCore.Mvc;
using OmniPyme.Web.DTOs;
using OmniPyme.Web.Services;
using OmniPyme.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace OmniPyme.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyfService;

        public AccountController(IUsersService usersService, IMapper mapper, INotyfService notyfService)
        {
            _usersService = usersService;
            _mapper = mapper;
            _notyfService = notyfService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _usersService.LoginAsync(dto);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
            }

            return View(dto);
        }

        [HttpGet]
        [Route("Errors/{statusCode:int}")]
        public IActionResult Error(int statusCode)
        {
            string errorMessage = "Ha ocurrido un error";

            switch (statusCode)
            {
                case StatusCodes.Status401Unauthorized:
                    errorMessage = "Debes iniciar sesión";
                    break;

                case StatusCodes.Status403Forbidden:
                    errorMessage = "No tienes permiso para estar aquí";
                    break;

                case StatusCodes.Status404NotFound:
                    errorMessage = "La página que estás intentando acceder no existe";
                    break;
            }

            ViewBag.ErrorMessage = errorMessage;

            return View(statusCode);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _usersService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateUser()
        {
            Users user = await _usersService.GetUserAsync(User.Identity.Name);

            if (user is null)
            {
                return NotFound();
            }

            return View(_mapper.Map<AccountUserDTO>(user));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUser(AccountUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                int affectedRows = await _usersService.UpdateUserAsync(dto);

                if (affectedRows > 0)
                {
                    _notyfService.Success("Datos de usuario actualizados con éxito");
                }
                else
                {
                    _notyfService.Error("Error al actualizar los datos de usuario");
                }

                return RedirectToAction("Index", "Home");

            }

            _notyfService.Error("Debe ajustar los errores de validación");
            return View(dto);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notyfService.Error("Debe ajustar los errores de validación");
                    return View();
                }

                Users? user = await _usersService.GetUserAsync(User.Identity.Name);
                if (user is null)
                {
                    _notyfService.Error("Ha ocurrido un error. Por favor intente mas tarde");
                    return View();
                }

                bool isCorrectPassword = await _usersService.CheckPasswordAsync(user, dto.CurrentPassword);

                if (!isCorrectPassword)
                {
                    _notyfService.Error("Credenciales incorrectas");
                    return View();
                }

                string resetToken = await _usersService.GeneratePasswordResetTokenAsync(user);
                IdentityResult result = await _usersService.ResetPasswordAsync(user, resetToken, dto.NewPassword);

                if (!result.Succeeded)
                {
                    _notyfService.Error("Ha ocurrido un error al intantar actulizar la contraseña");
                    ViewBag.Message = $"Error al actualizar la contraseña {result.Errors}";
                    return View(dto);
                }

                _notyfService.Success("Contraseña actualizada con éxito");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _notyfService.Error("Ha ocurrido un error. Por favor intente mas tarde");
                return View();
            }
        }
    }
}