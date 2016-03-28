using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class historia
    {
        [Required]
        [Display(Name="Paciente")]
        public int his_paciente { get; set; }
        [Display(Name = "Motivo")]
        public string his_motivo { get; set; }
        [Display(Name = "Problema")]
        public string his_problema { get; set; }
        [Display(Name = "Observacion")]
        public string his_observacion { get; set; }
    }
}