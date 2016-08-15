using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class accidente
    {
        [Display(Name = "Descripción")]
        public string acc_descripción { get; set; }
        [Display(Name = "Capacidad Laboral")]
        public string acc_capacidad { get; set; }
        [Display(Name = "Fecha")]
        public string acc_fecha { get; set; }
        [Display(Name = "Empresa")]
        public string acc_empresa { get; set; }
        [Display(Name = "Paciente")]
        public int acc_paciente { get; set; }
    }
}