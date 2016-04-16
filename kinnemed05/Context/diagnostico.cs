using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class diagnostico
    {
        [Display(Name="Historia")]
        [Required(ErrorMessage = "El campo es requerido")]
        public int dia_historia { get; set; }
        [Display(Name = "Observación")]
        public string dia_descripcion { get; set; }
        [Display(Name = "CIE10")]
        [Required(ErrorMessage = "El campo es requerido")]
        public int dia_subcie10 { get; set; }
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string dia_tipo { get; set; }
    }
}