using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class actividad
    {
        [Display(Name = "Enfermedad Profesional")]
        public string act_enf_estado { get; set; }
        [Display(Name = "Cuál")]
        [StringLength(200, MinimumLength = 0, ErrorMessage = "La longitud máxima es 200")]
        public string act_enf_descripcion { get; set; }
        [Display(Name = "Fecha")]
        public string act_enf_fecha { get; set; }
        [Display(Name = "Empresa")]
        [StringLength(200, MinimumLength = 0, ErrorMessage = "La longitud máxima es 200")]
        public string act_enf_empresa { get; set; }
        [Display(Name = "Accidente de Trabajo")]
        public string act_acc_estado { get; set; }
        [Display(Name = "Cuál")]
        [StringLength(200, MinimumLength = 0, ErrorMessage = "La longitud máxima es 200")]
        public string act_acc_descripcion { get; set; }
        [Display(Name = "Capacidad Laboral")]
        [StringLength(200, MinimumLength = 0, ErrorMessage = "La longitud máxima es 200")]
        public string act_acc_capacidad { get; set; }
        [Display(Name = "Fecha")]
        public string act_acc_fecha { get; set; }
        [Display(Name = "Empresa")]
        [StringLength(200, MinimumLength = 0, ErrorMessage = "La longitud máxima es 200")]
        public string act_acc_empresa { get; set; }
    }
}