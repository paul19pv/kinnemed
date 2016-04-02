using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class perfil
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([A-Za-z0-9 .&'-]+)$", ErrorMessage = "Solo se aceptan caracteres alfabéticos")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "La longitud mínima es 4 caracteres y la máxima 50")]
        public string per_nombre { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([A-Za-z0-9 .&'-]+)$", ErrorMessage = "Solo se aceptan caracteres alfabéticos")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "La longitud mínima es 4 caracteres y la máxima 150")]
        public string per_descripcion { get; set; }
    }
}