using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmniPyme.Web.Data.Entities
{
    public class Users : IdentityUser<int>
    {
        
        [MaxLength(45, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; } = null!;

        [MaxLength(45, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Último acceso")]
        public DateTime? UserLastAccess { get; set; }



        // ✔ Relación con la entidad Role
        [Display(Name = "Rol")]
        public int Roleid { get; set; }

        [ForeignKey(nameof(Roleid))]
        public Role Role { get; set; } = null!;

        // ✔ Propiedad calculada
        [NotMapped]
        public string FullName => $"{FirstName}{LastName}";

    }
}
