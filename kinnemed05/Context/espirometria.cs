using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class espirometria
    {
        [Display(Name="Paciente")]
        [Required(ErrorMessage="El campo Paciente es requerido")]
        public int esp_paciente { get; set; }
        [Display(Name = "Archivo")]
        public string esp_archivo { get; set; } 
        [Display(Name = "Observación")]
        public string esp_observacion { get; set; }
        //[Display(Name = "Médico")]
        //public int esp_medico { get; set; }
        [Display(Name = "Fecha")]
        public string esp_fecha { get; set; }
        public int esp_laboratorista { get; set; }

        public Nullable<int> esp_responsable { get; set; }
        public Nullable<int> esp_perfil { get; set; }
        public Nullable<int> esp_orden { get; set; }
    }
}