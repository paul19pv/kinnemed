using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class control
    {
        [Display(Name = "Perfil")]
        [Required(ErrorMessage = "El campo Perfil es requerido")]
        public int con_perfil { get; set; }
        [Display(Name = "Exámen")]
        [Required(ErrorMessage = "El campo Exámen es requerido")]
        public int con_examen { get; set; }
    }
}