using System.ComponentModel.DataAnnotations;

namespace OmniPyme.Web.Data.Entities
{
    public class Role
    {
        [Key]
        public int IdRol { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Nombre del Rol")]
        public string RolName { get; set; } = null!;
    }
}
