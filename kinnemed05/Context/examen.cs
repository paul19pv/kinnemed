using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class examen
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string exa_nombre { get; set; }
        [Display(Name = "Unidad")]
        public string exa_unidad { get; set; }
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string exa_tipo { get; set; }
        [Display(Name = "Area")]
        [Required(ErrorMessage = "Campo Requerido")]
        public int exa_area { get; set; }
        [Display(Name = "Item")]
        public Nullable<int> exa_item { get; set; }
        [Display(Name = "Valores")]
        public string exa_valores { get; set; }
        [Display(Name = "Inicial")]
        public string exa_inicial { get; set; }
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string exa_estado { get; set; }
        [Display(Name = "Orden")]
        [Required(ErrorMessage = "Campo Requerido")]
        public int exa_orden { get; set; }
    }
}