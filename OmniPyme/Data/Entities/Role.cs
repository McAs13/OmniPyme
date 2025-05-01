using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OmniPyme.Web.Data.Entities
{
    public class Role : IdentityRole<int>, IId
    {
        [Key]
        //public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Display(Name = "Nombre del Rol")]
        public override string Name { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
