using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OmniPyme.Web.Data.Entities
{
    public class ProductDTO
    {
        [Key]
        public int Id { get; set; }


        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Nombre Del Producto")]
        public string ProductName { get; set; } = null!;


        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Descripcion")]
        public string ProductDecription { get; set; } = null!;

        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Precio")]
        public double ProductPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El campo {0} debe ser un valor positivo")]
        public double ProductPrice { get; set; }

        [MaxLength(45, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Codido de Barras")]
        public string ProductBarCode { get; set; } = null!;

        [Display(Name = "Impuestos")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public double ProductTax { get; set; }


        public ProductCategory? ProductCategory { get; set; }

        public int IdProductCategory { get; set; }
    }
}
