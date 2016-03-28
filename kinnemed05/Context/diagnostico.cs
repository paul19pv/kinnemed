using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class diagnostico
    {
        [Display(Name="Historia")]
        public int dia_historia { get; set; }
        [Display(Name = "Descripcion")]
        public string dia_descripcion { get; set; }
        [Display(Name = "CIE10")]
        public int dia_subcie10 { get; set; }
        [Display(Name = "Tipo")]
        public string dia_tipo { get; set; }
    }
}