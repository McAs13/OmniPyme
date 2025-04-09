using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using OmniPyme.Web.Models;
using OmniPyme.Web.Responses;
using OmniPyme.Web.Services.Interfaces;

namespace OmniPyme.Web.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly IProductCategoriesService _service;
        private readonly INotyfService _notyf;

        public ProductCategoriesController(
            IProductCategoriesService service,
            INotyfService notyf)
        {
            _service = service;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index(PaginationDTO model)
        {
            var response = await _service.GetPaginationAsync(model);
            return View(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCategoryDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _service.CreateAsync(model);

            if (!response.WasSuccess)
            {
                _notyf.Error(response.Message);
                return View(model);
            }

            _notyf.Success("Categoría creada exitosamente");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _service.GetOneAsync(id);
            if (!response.WasSuccess)
            {
                _notyf.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductCategoryDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _service.EditAsync(model);

            if (!response.WasSuccess)
            {
                _notyf.Error(response.Message);
                return View(model);
            }

            _notyf.Success("Categoría actualizada correctamente");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(id);

            if (!response.WasSuccess)
                _notyf.Error(response.Message);
            else
                _notyf.Success("Categoría eliminada correctamente");

            return RedirectToAction(nameof(Index));
        }
    }
}
