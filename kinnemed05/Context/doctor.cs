using kinnemed05.DataAnnotation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class doctor
    {
        [Display(Name = "Cédula")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([0-9.&'-]+)$", ErrorMessage = "Solo se aceptan números")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "La longitud debe ser de 10 caracteres")]
        //[IsUnique("doc_cedula",ErrorMessage="La cédula ya existe")]
        public string doc_cedula { get; set; }
        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([A-Za-z0-9Ññ .&'-]+)$", ErrorMessage = "Solo se aceptan caracteres alfabéticos")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "La longitud mínima es 4 caracteres y la máxima 150")]
        public string doc_nombres { get; set; }
        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([A-Za-z0-9Ññ .&'-]+)$", ErrorMessage = "Solo se aceptan caracteres alfabéticos")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "La longitud mínima es 4 caracteres y la máxima 150")]
        public string doc_apellidos { get; set; }
        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([A-Za-z0-9 .&'-]+)$", ErrorMessage = "Solo se aceptan caracteres alfabéticos")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "La longitud mínima es 1 caracteres y la máxima 50")]
        public string doc_codigo { get; set; }
        [Display(Name = "Especialidad")]
        [Required(ErrorMessage = "Campo Requerido")]
        public int doc_especialidad { get; set; }
        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = "Campo Requerido")]
        [EmailAddress(ErrorMessage = "El correo no tiene el formato correcto")]
        [StringLength(150, ErrorMessage = "La longitud máxima es 150 caracteres")]
        public string doc_correo { get; set; }
        [Display(Name = "Firma")]
        public byte[] doc_firma { get; set; }
        public bool doc_estado { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Campo Requerido")]
        public int doc_empresa { get; set; }
    }
}