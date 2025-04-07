using AspNetCoreHero.ToastNotification.Abstractions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OmniPyme.Web.Core;
using OmniPyme.Web.Core.Pagination;
using OmniPyme.Web.DTOs;
using OmniPyme.Web.Helpers;
using OmniPyme.Web.Services;
using OmniPyme.Web.ViewModels;



namespace OmniPyme.Web.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IInvoicesService _invoicesService;
        private readonly ISalesService _salesService;
        private readonly ISaleDetailService _saleDetailService;
        private readonly INotyfService _notyfService;
        private readonly ICombosHelper _combosHelper;

        public InvoicesController(IInvoicesService invoicesService, INotyfService notyfService, ICombosHelper combosHelper, ISalesService salesService, ISaleDetailService saleDetailService)
        {
            _invoicesService = invoicesService;
            _notyfService = notyfService;
            _combosHelper = combosHelper;
            _salesService = salesService;
            _saleDetailService = saleDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<InvoiceDTO>> response = await _invoicesService.GetPaginationAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Obtener el último registro ordenado por InvoiceNumber
            string nextInvoiceNumber = await _invoicesService.GetNextInvoiceNumberAsync();

            InvoiceCreateViewModel viewModel = new InvoiceCreateViewModel
            {
                InvoiceNumber = nextInvoiceNumber,
                InvoiceDate = DateTime.Now,
                SaleDate = DateTime.Now,
                Clients = await _combosHelper.GetComboCliente()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InvoiceCreateViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validacion");
                viewModel.Clients = await _combosHelper.GetComboCliente();
                return View(viewModel);
            }

            //Mapea de InvoiceCreateViewModel a SaleDTO
            SaleDTO dto = new SaleDTO
            {
                IdClient = viewModel.IdClient,
                SaleDate = viewModel.SaleDate,
                SaleTotal = (decimal)viewModel.SaleTotal,
                SalePaymentMethod = viewModel.SalePaymentMethod
            };

            // Verificar que hay detalles de venta
            if (viewModel.SaleDetails == null || !viewModel.SaleDetails.Any())
            {
                _notyfService.Error("La factura debe tener al menos un producto");
                viewModel.Clients = await _combosHelper.GetComboCliente();
                return View(viewModel);
            }

            Response<SaleDTO> saleResponse = await _salesService.CreateAsync(dto);

            if (!saleResponse.IsSuccess)
            {
                _notyfService.Error("Error al crear la venta: " + saleResponse.Message);
                viewModel.Clients = await _combosHelper.GetComboCliente();
                return View(viewModel);
            }

            viewModel.IdSale = saleResponse.Result.Id;
            viewModel.SaleCode = saleResponse.Result.SaleCode;

            if (viewModel.IdSale == null)
            {
                _notyfService.Error("Error al crear la venta: " + saleResponse.Message);
                viewModel.Clients = await _combosHelper.GetComboCliente();
                return View(viewModel);
            }

            int detailCounter = 1;

            foreach (SaleDetailViewModel detail in viewModel.SaleDetails)
            {
                detail.IdSale = viewModel.IdSale ?? 0;

                string generatedDetailCode = $"{viewModel.SaleCode}-{detailCounter}";

                await _saleDetailService.CreateAsync(new SaleDetailDTO
                {
                    IdSale = detail.IdSale,
                    SaleDetailCode = generatedDetailCode,
                    SaleDetailProductCode = detail.IdProduct,
                    SaleDetailProductQuantity = detail.SaleDetailProductQuantity,
                    SaleDetailProductPrice = (decimal)detail.SaleDetailProductPrice,
                    SaleDetailSubtotal = (decimal)detail.SaleDetailSubtotal
                });
                detailCounter++;
            }

            Response<InvoiceDTO> response = await _invoicesService.CreateAsync(new InvoiceDTO
            {
                InvoiceNumber = viewModel.InvoiceNumber,
                InvoiceDate = viewModel.InvoiceDate,
                IdSale = viewModel.IdSale ?? 0
            });

            if (!response.IsSuccess)
            {
                _notyfService.Error("Error al crear la factura: " + response.Message);
                viewModel.Clients = await _combosHelper.GetComboCliente();
                return View(viewModel);
            }

            _notyfService.Success("Factura creada correctamente");
            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public async Task<IActionResult> Edit([FromRoute] int id)
        //{
        //    Response<InvoiceDTO> response = await _invoicesService.GetOneAsync(id);

        //    if (!response.IsSuccess)
        //    {
        //        _notyfService.Error(response.Message);
        //        return RedirectToAction("Index");
        //    }

        //    InvoiceDTO dto = response.Result;

        //    int selectedClientId = dto.Sale?.IdClient ?? 0;

        //    InvoiceCreateViewModel viewModel = new InvoiceCreateViewModel
        //    {
        //        IdInvoice = dto.Id,
        //        InvoiceNumber = dto.InvoiceNumber,
        //        InvoiceDate = dto.InvoiceDate,
        //        IdSale = dto.Sale?.Id ?? 0,
        //        SaleDate = dto.Sale?.SaleDate ?? dto.InvoiceDate,
        //        SaleTotal = (double)(dto.Sale?.SaleTotal ?? 0),
        //        SalePaymentMethod = dto.Sale?.SalePaymentMethod, // Valor por defecto si no tiene
        //        IdClient = selectedClientId,

        //        Clients = await _combosHelper.GetComboCliente(selectedClientId)
        //    };

        //    return View(viewModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(InvoiceCreateViewModel viewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        _notyfService.Error("Debe ajustar los errores de validacion");
        //        viewModel.Clients = await _combosHelper.GetComboCliente();
        //        return View(viewModel);
        //    }

        //    //Mapea de InvoiceCreateViewModel a InvoiceDTO
        //    InvoiceDTO dto = new InvoiceDTO
        //    {
        //        Id = viewModel.IdInvoice ?? 0,
        //        InvoiceNumber = viewModel.InvoiceNumber,
        //        InvoiceDate = viewModel.InvoiceDate,
        //        IdClient = viewModel.IdClient,
        //        IdSale = viewModel.IdSale ?? 0,
        //    };

        //    Response<InvoiceDTO> response = await _invoicesService.EditAsync(dto);

        //    if (!response.IsSuccess)
        //    {
        //        _notyfService.Error(response.Message);
        //        dto.Clients = await _combosHelper.GetComboCliente();
        //        return View(viewModel);
        //    }

        //    _notyfService.Success(response.Message);
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //Response<object> response = await _invoicesService.DeleteAsync(id);
            //if (!response.IsSuccess)
            //{
            //    _notyfService.Error(response.Message);
            //}
            //else
            //{
            //    _notyfService.Success(response.Message);
            //}
            //return RedirectToAction("Index");

            // Obtener la factura para saber su IdSale asociado
            Response<InvoiceDTO> invoiceResponse = await _invoicesService.GetOneAsync(id);

            if (!invoiceResponse.IsSuccess)
            {
                _notyfService.Error("Error al obtener la factura: " + invoiceResponse.Message);
                return RedirectToAction("Index");
            }

            int idSale = invoiceResponse.Result.IdSale;

            //// Eliminar los detalles de la venta
            //Response<List<SaleDetailDTO>> detailResponse = await _saleDetailService.GetBySaleIdAsync(idSale);
            //if (detailResponse.IsSuccess)
            //{
            //    foreach (SaleDetailDTO detail in detailResponse.Result)
            //    {
            //        await _saleDetailService.DeleteAsync(detail.Id);
            //    }
            //}

            // Eliminar la venta
            Response<object> saleDeleteResponse = await _salesService.DeleteAsync(idSale);
            if (!saleDeleteResponse.IsSuccess)
            {
                _notyfService.Error(saleDeleteResponse.Message);
                return RedirectToAction("Index");
            }

            //// Eliminar la factura
            //Response<object> invoiceDeleteResponse = await _invoicesService.DeleteAsync(id);
            if (!saleDeleteResponse.IsSuccess)
            {
                _notyfService.Error(saleDeleteResponse.Message);
            }
            else
            {
                _notyfService.Success(saleDeleteResponse.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
