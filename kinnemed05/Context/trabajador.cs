using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class trabajador
    {
        [Display(Name = "Cédula")]
        public string tra_cedula { get; set; }
        [Display(Name = "Nombres")]
        public string tra_nombres { get; set; }
        [Display(Name = "Apellidos")]
        public string tra_apellidos { get; set; }
        [Display(Name = "Correo Electrónico")]
        public string tra_correo { get; set; }
        [Display(Name = "Empresa")]
        public int tra_empresa { get; set; }
        [Display(Name = "Estado")]
        public bool tra_estado { get; set; }
    }
}