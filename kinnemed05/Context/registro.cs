using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class registro
    {
        [Display(Name = "Paciente")]
        public int reg_paciente { get; set; }
        [Display(Name = "Orden")]
        public int reg_orden { get; set; }
        [Display(Name = "Fecha")]
        public string reg_fecha { get; set; }
        [Display(Name = "Médico")]
        public Nullable<int> reg_medico { get; set; }
        [Display(Name = "Estado")]
        public Nullable<bool> reg_estado { get; set; }
    }
}