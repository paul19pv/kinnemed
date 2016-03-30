using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class audiometria
    {
        [Display(Name = "Paciente")]
        [Required(ErrorMessage = "El campo Paciente es requerido")]
        public int aud_paciente { get; set; }
        [Display(Name = "Archivo")]
        [Required(ErrorMessage = "El campo Archivo es requerido")]
        public string aud_archivo { get; set; }
        [Display(Name = "Observación")]
        [Required(ErrorMessage = "El campo Observación es requerido")]
        public string aud_observacion { get; set; }
    }
}