using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class plan
    {
        [Display(Name = "Recomendaciones Farmácologicas")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string pla_texto1 { get; set; }
        [Display(Name = "Recomendaciones No Farmacológicas")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string pla_texto2 { get; set; }
    }
}