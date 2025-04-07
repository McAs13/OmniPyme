﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OmniPyme.Web.ViewModels
{
    public class InvoiceCreateViewModel
    {// Propiedades de Invoice
        public int? IdInvoice { get; set; }

        [Display(Name = "Numero de factura")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string InvoiceNumber { get; set; }

        [Display(Name = "Fecha de venta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        // Propiedades de Sale
        public int? IdSale { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public double SaleTotal { get; set; }

        [Display(Name = "Metodo de pago")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string SalePaymentMethod { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int IdClient { get; set; }
        public int IdUser { get; set; }

        // Lista de detalles de venta (productos)
        public List<SaleDetailViewModel> SaleDetails { get; set; } = new List<SaleDetailViewModel>();

        // Propiedades para la UI (listas desplegables)
        public IEnumerable<SelectListItem>? Clients { get; set; }
        public IEnumerable<SelectListItem>? Products { get; set; }
        public IEnumerable<SelectListItem>? PaymentMethods { get; set; }
    }

    public class SaleDetailViewModel
    {
        public int? IdSaleDetail { get; set; }
        public int IdSale { get; set; }
        public int IdProduct { get; set; }
        public int SaleDetailProductQuantity { get; set; }
        public double SaleDetailProductPrice { get; set; }
        public double SaleDetailSubtotal { get; set; }

        // Propiedades adicionales para la UI
        public string ProductName { get; set; }
    }
}
