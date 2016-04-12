using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class inmunizacion
    {
        [Display(Name="Vacuna")]
        public int inm_vacuna { get; set; }
        [Display(Name = "Fecha última dosis")]
        public string inm_fecha { get; set; }
        [Display(Name = "Dosis recibida")]
        public string inm_tipo { get; set; }
        [Display(Name = "Paciente")]
        public int inm_paciente { get; set; }
    }
}