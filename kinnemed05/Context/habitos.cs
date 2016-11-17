using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class habitos
    {
        [Display(Name = "Fumó")]
        [Required(ErrorMessage = "Campo requerido")]
        public string hab_fumo { get; set; }
        [Display(Name = "Fumá")]
        [Required(ErrorMessage = "Campo requerido")]
        public string hab_fuma { get; set; }
        [Display(Name = "Frecuencia")]
        public string hab_cigarillos { get; set; }
        [Display(Name = "Alcohol")]
        [Required(ErrorMessage = "Campo requerido")]
        public string hab_alcohol { get; set; }
        [Display(Name = "Frecuencia")]
        public string hab_alc_txt { get; set; }
        [Display(Name = "Drogas")]
        [Required(ErrorMessage = "Campo requerido")]
        public string hab_drogas { get; set; }
        [Display(Name = "Especifique")]
        public string hab_dro_txt { get; set; }
        [Display(Name = "Ejercicio")]
        [Required(ErrorMessage = "Campo requerido")]
        public string hab_ejercicio { get; set; }
    }
}