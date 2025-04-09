using Microsoft.AspNetCore.Mvc;
using OmniPyme.Shared.Helpers;
using OmniPyme.Shared.Responses;
using OmniPyme.Web.Data.DTOs;
using OmniPyme.Web.Helpers;
using OmniPyme.Web.Services.Interfaces;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace OmniPyme.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ICombosHelper _combosHelper;
        private readonly INotyfService _notyf;

        public ProductsController(
            IProductsService productsService,
            ICombosHelper combosHelper,
            INotyfService notyf)
        {
            _productsService = productsService;
            _combosHelper = combosHelper;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index(string Filter = "", int RecordsPerPage = 10, int Page = 1)
        {
            var model = await _productsService.GetPaginationAsync(Filter, RecordsPerPage, Page);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new ProductDTO
            {
                ProductCategory = null,
                IdProductCategory = 0
            };

            ViewBag.ProductCategories = await _combosHelper.GetComboProductCategories();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductCategories = await _combosHelper.GetComboProductCategories(model.IdProductCategory);
                return View(model);
            }

            var response = await _productsService.CreateAsync(model);

            if (response.Succeeded)
            {
                _notyf.Success("Producto creado exitosamente");
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            ViewBag.ProductCategories = await _combosHelper.GetComboProductCategories(model.IdProductCategory);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _productsService.GetOneAsync(id);
            if (!response.Succeeded || response.Result == null)
            {
                return NotFound();
            }

            ViewBag.ProductCategories = await _combosHelper.GetComboProductCategories(response.Result.IdProductCategory);
            return View(response.Result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductCategories = await _combosHelper.GetComboProductCategories(model.IdProductCategory);
                return View(model);
            }

            var response = await _productsService.EditAsync(model);

            if (response.Succeeded)
            {
                _notyf.Success("Producto editado correctamente");
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            ViewBag.ProductCategories = await _combosHelper.GetComboProductCategories(model.IdProductCategory);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productsService.DeleteAsync(id);
            if (response.Succeeded)
            {
                _notyf.Success("Producto eliminado correctamente");
            }
            else
            {
                _notyf.Error(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
