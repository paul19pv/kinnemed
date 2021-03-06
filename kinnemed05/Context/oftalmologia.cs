﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class oftalmologia
    {
        [Display(Name = "Paciente")]
        [Required(ErrorMessage = "El campo es requerido")]
        public int oft_paciente { get; set; }
        [Display(Name = "OD")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string oft_con_od { get; set; }
        [Display(Name = "OI")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string oft_con_oi { get; set; }
        [Display(Name = "OD")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string oft_sin_od { get; set; }
        [Display(Name = "OI")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string oft_sin_oi { get; set; }
        [Display(Name = "OD")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string oft_ref_od { get; set; }
        [Display(Name = "OI")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string oft_ref_oi { get; set; }
        [Display(Name = "Biomicroscopía")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string oft_biomiscroscopia { get; set; }
        [Display(Name = "Observación")]
        public string oft_bio_txt { get; set; }
        [Display(Name = "Fondo de Ojo")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string oft_fondo { get; set; }
        [Display(Name = "Observación")]
        public string oft_fon_txt { get; set; }
        [Display(Name = "Test de Colores")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string oft_colores { get; set; }
        [Display(Name = "Diagnóstico")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string oft_diagnostico { get; set; }
        [Display(Name = "Indicaciones")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string oft_indicaciones { get; set; }
        public string oft_dia_txt { get; set; }
        public string oft_ind_txt { get; set; }
        //[Display(Name = "Médico")]
        //public int oft_medico { get; set; }
        [Display(Name = "Otros")]
        public string oft_otros { get; set; }

        public string oft_fecha { get; set; }
        public Nullable<int> oft_responsable { get; set; }
        public Nullable<int> oft_perfil { get; set; }
        public Nullable<int> oft_orden { get; set; }

        
    }
}