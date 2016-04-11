using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class subsecuente
    {
        [Display(Name="Historia")]
        [Required(ErrorMessage = "Campo Requerido")]
        public int sub_historia { get; set; }
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string sub_fecha { get; set; }
        [Display(Name = "Hora")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string sub_hora { get; set; }
        [Display(Name = "Subjetivo")]
        public string sub_subjetivo { get; set; }
        [Display(Name = "Objetivo")]
        public string sub_objetivo { get; set; }
        [Display(Name = "Análisis")]
        public string sub_analisis { get; set; }
        [Display(Name = "Plan")]
        public string sub_plan { get; set; }
    }
}