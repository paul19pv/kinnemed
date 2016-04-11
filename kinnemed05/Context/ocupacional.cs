using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class ocupacional
    {
        [Display(Name = "Empresa")]
        public string ocu_empresa { get; set; }
        [Display(Name = "Sección")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string ocu_seccion { get; set; }
        [Display(Name = "Cargo")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string ocu_cargo { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string ocu_descripcion { get; set; }
        [Display(Name = "Jornada")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string ocu_jornada { get; set; }
        [Display(Name = "Fecha de Inicio")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string ocu_inicio { get; set; }
        [Display(Name = "Fecha de Fin")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string ocu_fin { get; set; }
        [Display(Name = "Tiempo")]
        [Required(ErrorMessage = "Campo Requerido")]
        public decimal ocu_tiempo { get; set; }
        [Display(Name = "Maquinaria/Equipos Utilizados")]
        public string ocu_maquinaria { get; set; }
        [Display(Name = "Materiales de Uso")]
        public string ocu_materiales { get; set; }
        [Display(Name = "Sustancias Químicas Utilizadas")]
        public string ocu_sustancias { get; set; }
        [Display(Name = "Equipo de Protección")]
        public string ocu_equipo { get; set; }
        public string ocu_tipo { get; set; }
        public bool ocu_estado { get; set; }
        [Display(Name = "Paciente")]
        [Required(ErrorMessage = "Campo Requerido")]
        public int ocu_paciente { get; set; }
    }
}