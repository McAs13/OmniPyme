using System.ComponentModel.DataAnnotations;

namespace OmniPyme.Web.DTOs
{
    public class ClientDTO
    {
        [Key]
        public int IdClient { get; set; }

        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; } = null!;

        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; } = null!;

        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Telefono")]
        public string Phone { get; set; } = null!;

        [Display(Name = "Fecha de Registro")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime RegisterDate { get; set; }

        [Display(Name = "Fecha de ultima compra")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime LastPurchaseDate { get; set; }

    }
}
