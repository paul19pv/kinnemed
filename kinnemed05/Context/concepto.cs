using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class concepto
    {
        [Display(Name = "Resultado")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string con_resultado { get; set; }
        [Display(Name = "Observación")]
        public string con_observacion { get; set; }
        [Display(Name = "Valor")]
        public string con_valor { get; set; }
    }
}