using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class signos
    {
        [Display(Name="Presión Arterial")]
        public string sig_presion { get; set; }
        [Display(Name = "Frecuencia Cardiaca. min")]
        public Nullable<decimal> sig_cardiaca { get; set; }
        [Display(Name = "Frecuencia Respiratoria. min")]
        public Nullable<decimal> sig_respiratoria { get; set; }
        [Display(Name = "Temperatura ºC")]
        public Nullable<decimal> sig_temperatura { get; set; }
        [Display(Name = "Peso Kg")]
        public Nullable<decimal> sig_peso { get; set; }
        [Display(Name = "Talla m")]
        public Nullable<decimal> sig_talla { get; set; }
        [Display(Name = "Indice Masa Corporal")]
        public Nullable<decimal> sig_masa { get; set; }
        [Display(Name = "Perímetro Cefalico cm")]
        public Nullable<decimal> sig_perimetro { get; set; }
        [Display(Name = "% Grasa Viceral")]
        public Nullable<decimal> sig_viceral { get; set; }
        [Display(Name = "% Grasa Corporal")]
        public Nullable<decimal> sig_corporal { get; set; }
        [Display(Name = "Kilocalorias")]
        public Nullable<decimal> sig_kilocalorias { get; set; }
        [Display(Name = "Edad/Peso")]
        public string sig_edadpeso { get; set; }
        [Display(Name = "% Masa Muscular")]
        public Nullable<decimal> sig_masamuscular { get; set; }
        

    }
}