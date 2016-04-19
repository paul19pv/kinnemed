using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class laboratorista
    {
        [Display(Name = "Cédula")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([0-9.&'-]+)$", ErrorMessage = "Solo se aceptan números")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "La longitud debe ser de 10 caracteres")]
        public string lab_cedula { get; set; }
        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([A-Za-z0-9 .&'-]+)$", ErrorMessage = "Solo se aceptan caracteres alfabéticos")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "La longitud mínima es 4 caracteres y la máxima 150")]
        public string lab_nombres { get; set; }
        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([A-Za-z0-9 .&'-]+)$", ErrorMessage = "Solo se aceptan caracteres alfabéticos")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "La longitud mínima es 4 caracteres y la máxima 150")]
        public string lab_apellidos { get; set; }
        [Display(Name = "Coreo")]
        [Required(ErrorMessage = "Campo Requerido")]
        [EmailAddress(ErrorMessage = "El correo no tiene el formato correcto")]
        [StringLength(100, ErrorMessage = "La longitud máxima es 100 caracteres")]
        public string lab_correo { get; set; }
        [Display(Name = "Firma")]
        public string lab_firma { get; set; }
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Campo Requerido")]
        public bool lab_estado { get; set; }
    }
}