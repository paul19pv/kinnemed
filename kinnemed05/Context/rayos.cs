using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class rayos
    {
        [Display(Name = "Paciente")]
        [Required(ErrorMessage = "El campo Paciente es requerido")]
        public int ray_paciente { get; set; }
        [Display(Name = "Imagen")]
        //[Required(ErrorMessage = "El campo Imagen es requerido")]
        public string ray_imagen { get; set; }
        [Display(Name = "Observación")]
        public string ray_observacion { get; set; }
        //[Display(Name = "Médico")]
        //public int ray_medico { get; set; }
        public int ray_laboratorista { get; set; }

        public string ray_fecha { get; set; }
        public Nullable<int> ray_responsable { get; set; }
        public Nullable<int> ray_perfil { get; set; }
        public Nullable<int> ray_orden { get; set; }
    }
}