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
        [Display(Name = "Observación")]
        public string his_observacion { get; set; }
        [Display(Name = "Tipo")]
        public int his_tipo { get; set; }
        [Display(Name = "Fecha")]
        public string his_fecha { get; set; }
        [Display(Name = "Número")]
        public int his_numero { get; set; }
        [Display(Name = "Médico")]
        public int his_medico { get; set; }

        [Display(Name = "Firma")]
        public byte[] his_firma { get; set; }
    }
}