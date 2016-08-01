using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class personal
    {
        [Display(Name = "Patológicas")]
        public bool per_patologicas { get; set; }
        [Display(Name = "")]
        public string per_pat_txt { get; set; }
        [Display(Name = "Quirúrgicas")]
        public bool per_quirurgicas { get; set; }
        [Display(Name = "")]
        public string per_qui_txt { get; set; }
        [Display(Name = "Traumáticos")]
        public bool per_traumaticos { get; set; }
        [Display(Name = "")]
        public string per_tra_txt { get; set; }
        [Display(Name = "Alergías")]
        public bool per_alergias { get; set; }
        [Display(Name = "")]
        public string per_ale_txt { get; set; }
        [Display(Name = "Vacunas")]
        public bool per_vacunas { get; set; }
        [Display(Name = "")]
        public string per_vac_txt { get; set; }
        [Display(Name = "Otros")]
        public bool per_otros { get; set; }
        [Display(Name = "")]
        public string per_otr_txt { get; set; }
        [Display(Name = "Lateralidad")]
        public string per_lateralidad { get; set; }

    }
}