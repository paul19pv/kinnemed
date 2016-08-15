using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class enfermedad
    {
        [Display(Name = "Descripción")]
        public string enf_descripcion { get; set; }
        [Display(Name = "Fecha")]
        public string enf_fecha { get; set; }
        [Display(Name = "Empresa")]
        public string enf_empresa { get; set; }
        [Display(Name = "Paciente")]
        public int enf_paciente { get; set; }
    }
}