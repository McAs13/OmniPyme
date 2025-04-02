using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using OmniPyme.Web.Core;
using OmniPyme.Web.Core.Pagination;
using OmniPyme.Web.DTOs;
using OmniPyme.Web.Services;

namespace OmniPyme.Web.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IInvoicesService _invoicesService;
        private readonly INotyfService _notyfService;

        public InvoicesController(IInvoicesService invoicesService, INotyfService notyfService)
        {
            _invoicesService = invoicesService;
            _notyfService = notyfService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<InvoiceDTO>> response = await _invoicesService.GetPaginationAsync(request);
            return View(response.Result);
        }
    }
}
