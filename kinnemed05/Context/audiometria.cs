﻿using System;
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
        public string aud_archivo { get; set; }
        [Display(Name = "Observación")]
        public string aud_observacion { get; set; }
    }
}