using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class ginecologico
    {
        [Display(Name="Fecha Última Menstruación")]
        public string gin_fum { get; set; }
        [Display(Name = "Ciclos Menstruales")]
        public Nullable<int> gin_ciclos { get; set; }
        [Display(Name = "Gestas")]
        public Nullable<int> gin_gestas { get; set; }
        [Display(Name = "Partos")]
        public Nullable<int> gin_partos { get; set; }
        [Display(Name = "Cesáreas")]
        public Nullable<int> gin_cesarea { get; set; }
        [Display(Name = "Abortos")]
        public Nullable<int> gin_abortos { get; set; }
        [Display(Name = "Hijos")]
        public Nullable<int> gin_hijos { get; set; }
        [Display(Name = "PLanificación")]
        public bool gin_planificacion { get; set; }
        [Display(Name = "Especifique")]
        public string gin_pla_txt { get; set; }
        [Display(Name = "Paptest (fecha)")]
        public string gin_paptest { get; set; }
    }
}