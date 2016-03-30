//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace kinnemed05.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class paciente
    {
        public paciente()
        {
            this.registro = new HashSet<registro>();
            this.historia = new HashSet<historia>();
            this.audiometria = new HashSet<audiometria>();
            this.espirometria = new HashSet<espirometria>();
            this.rayos = new HashSet<rayos>();
            this.inmunizacion = new HashSet<inmunizacion>();
            this.ocupacional = new HashSet<ocupacional>();
        }
    
        public int pac_id { get; set; }
        public string pac_cedula { get; set; }
        public string pac_nombres { get; set; }
        public string pac_apellidos { get; set; }
        public string pac_genero { get; set; }
        public string pac_estadocivil { get; set; }
        public Nullable<int> pac_pais { get; set; }
        public string pac_fechanacimiento { get; set; }
        public int pac_edad { get; set; }
        public string pac_telefono { get; set; }
        public string pac_celular { get; set; }
        public string pac_correo { get; set; }
        public Nullable<int> pac_provincia { get; set; }
        public Nullable<int> pac_canton { get; set; }
        public string pac_direccion { get; set; }
        public string pac_instruccion { get; set; }
        public Nullable<int> pac_profesion { get; set; }
        public string pac_tipodiscapacidad { get; set; }
        public Nullable<int> pac_porcentajediscapacidad { get; set; }
        public Nullable<int> pac_empresa { get; set; }
        public string pac_estado { get; set; }
    
        public virtual canton canton { get; set; }
        public virtual empresa empresa { get; set; }
        public virtual pais pais { get; set; }
        public virtual profesion profesion { get; set; }
        public virtual provincia provincia { get; set; }
        public virtual ICollection<registro> registro { get; set; }
        public virtual familiar familiar { get; set; }
        public virtual ICollection<historia> historia { get; set; }
        public virtual personal personal { get; set; }
        public virtual ginecologico ginecologico { get; set; }
        public virtual ICollection<audiometria> audiometria { get; set; }
        public virtual ICollection<espirometria> espirometria { get; set; }
        public virtual ICollection<rayos> rayos { get; set; }
        public virtual ICollection<inmunizacion> inmunizacion { get; set; }
        public virtual ICollection<ocupacional> ocupacional { get; set; }
    }
}
