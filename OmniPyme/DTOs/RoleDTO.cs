using System.ComponentModel.DataAnnotations;

namespace OmniPyme.Web.DTOs
{
    public class RoleDTO
    {
        [Key]
        [Required(ErrorMessage = "El campo IdRol es obligatorio")] // Asegura que IdRol tenga un valor
        public int Id { get; set; }

        [Display(Name = "Nombre del Roll")]
        [Required(ErrorMessage = "El campo Nombre del Rol es obligatorio")]
        [StringLength(100, ErrorMessage = "El Nombre del Rol no puede tener más de 100 caracteres")]
        public string RolName { get; set; } = null!;
    }
}
