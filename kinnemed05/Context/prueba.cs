using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class prueba
    {
        [Display(Name = "Exámen")]
        //[Required(ErrorMessage = "Campo Requerido")]
        public int pru_examen { get; set; }
        [Display(Name = "Registro")]
        //[Required(ErrorMessage = "Campo Requerido")]
        public int pru_registro { get; set; }
        [Display(Name = "Resultado")]
        public string pru_resultado { get; set; }
        [Display(Name = "Valor")]
        public string pru_valor { get; set; }
        [Display(Name = "Fuera de Rango")]
        public string pru_fuera { get; set; }
    
    }
}