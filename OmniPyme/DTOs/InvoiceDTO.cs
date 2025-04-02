using OmniPyme.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace OmniPyme.Web.DTOs
{
    public class InvoiceDTO
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Fecha de venta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime InvoiceDate { get; set; }

        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Metodo de pago")]
        public string InvoiceNumber { get; set; } = null!;

        public Sale? Sale { get; set; }

        public int IdSale { get; set; }
    }
}
