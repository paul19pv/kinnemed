using kinnemed05.DataAnnotation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class trabajador
    {
        [Display(Name = "Cédula")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([0-9.&'-]+)$", ErrorMessage = "Solo se aceptan números")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "La longitud debe ser de 10 caracteres")]
        //[IsUnique("tra_cedula")]
        public string tra_cedula { get; set; }
        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([A-Za-z0-9Ññ .&'-]+)$", ErrorMessage = "Solo se aceptan caracteres alfabéticos")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "La longitud mínima es 4 caracteres y la máxima 150")]
        public string tra_nombres { get; set; }
        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([A-Za-z0-9Ññ .&'-]+)$", ErrorMessage = "Solo se aceptan caracteres alfabéticos")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "La longitud mínima es 4 caracteres y la máxima 150")]
        public string tra_apellidos { get; set; }
        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "Campo Requerido")]
        [EmailAddress(ErrorMessage = "El correo no tiene el formato correcto")]
        [StringLength(150, ErrorMessage = "La longitud máxima es 150 caracteres")]
        public string tra_correo { get; set; }
        [Display(Name = "Empresa")]
        public int tra_empresa { get; set; }
        [Display(Name = "Estado")]
        public bool tra_estado { get; set; }
    }
}