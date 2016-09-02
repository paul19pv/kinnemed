using kinnemed05.DataAnnotation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class empresa
    {
        [Display(Name = "RUC")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([0-9.&'-]+)$", ErrorMessage = "Solo se aceptan números")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "La longitud del campo debe ser 13 caracteres")]
        [IsUnique("emp_cedula")]
        public string emp_cedula { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([A-Za-z0-9Ññ .&'-]+)$", ErrorMessage = "Solo se aceptan caracteres alfabéticos")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "La longitud mínima es 4 caracteres y la máxima 50")]
        public string emp_nombre { get; set; }
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "Campo Requerido")]
        [DataType(DataType.Text, ErrorMessage = "El campo no tiene el formato correcto")]
        [StringLength(200, MinimumLength = 4, ErrorMessage = "La longitud mínima es 4 caracteres y la máxima 200")]
        public string emp_direccion { get; set; }
        [Display(Name = "Telefono")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([0-9.&'-]+)$", ErrorMessage = "Solo se aceptan números")]
        [StringLength(10, MinimumLength = 7, ErrorMessage = "La longitud del campo debe ser 10 caracteres")]
        public string emp_telefono { get; set; }
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "Campo Requerido")]
        [EmailAddress(ErrorMessage = "El correo no tiene el formato correcto")]
        [StringLength(100, ErrorMessage = "La longitud máxima es 100 caracteres")]
        public string emp_mail { get; set; }
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string emp_estado { get; set; }
    }
}