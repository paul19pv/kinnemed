using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class reposo
    {
        [Display(Name = "Fecha de Inicio")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string rep_inicio { get; set; }
        [Display(Name = "Fecha de Fin")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string rep_fin { get; set; }
        [Display(Name = "Tiempo de Reposo (Horas)")]
        [Required(ErrorMessage = "Campo Requerido")]
        public int rep_tiempo { get; set; }
        [Display(Name = "Fecha de Inicio en texto")]
        public string rep_ini_txt { get; set; }
        [Display(Name = "Tiempo de Fin en texto")]
        public string rep_fin_txt { get; set; }
    }
}