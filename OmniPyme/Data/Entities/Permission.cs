using System.ComponentModel.DataAnnotations;

namespace OmniPyme.Web.Data.Entities
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nombre del Rol")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string PermisionName { get; set; } = null!;


        [Display(Name = "Módulo")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Module { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();


    }
}
