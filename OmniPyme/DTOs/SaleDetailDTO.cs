using System.ComponentModel.DataAnnotations;
using OmniPyme.Web.Data.Entities;

namespace OmniPyme.Web.DTOs
{
    public class SaleDetailDTO
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Codigo de Venta")]
        public string SaleDetailCode { get; set; }
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int SaleDetailProductQuantity { get; set; }
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public decimal SaleDetailProductPrice { get; set; }
        [Display(Name = "Subtotal")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public decimal SaleDetailSubtotal { get; set; }
        [Display(Name = "Codigo de Producto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int SaleDetailProductCode { get; set; }
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? ProductName { get; set; } = null!; //Esto es temporal ya que debe haber relacion a la tabla de productos
        public Sale? Sale { get; set; }
        public int IdSale { get; set; }
    }
}
