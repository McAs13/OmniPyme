﻿using System.ComponentModel.DataAnnotations;

namespace OmniPyme.Web.DTOs
{
    public class PersonDTO
    {
        [Key]
        public int IdPersona { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string NombrePersona { get; set; } = null!;
        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El campo Apellido es obligatorio")]
        public string ApellidoPersona { get; set; } = null!;
        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo Email es obligatorio")]
        public string EmailPersona { get; set; } = null!;
        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo Telefono es obligatorio")]
        public string TelefonoPersona { get; set; } = null!;

    }
}
