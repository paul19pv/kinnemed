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
    
    public partial class empresa
    {
        public empresa()
        {
            this.trabajador = new HashSet<trabajador>();
            this.paciente = new HashSet<paciente>();
            this.doctor = new HashSet<doctor>();
        }
    
        public int emp_id { get; set; }
    
        public virtual ICollection<trabajador> trabajador { get; set; }
        public virtual ICollection<paciente> paciente { get; set; }
        public virtual ICollection<doctor> doctor { get; set; }
    }
}
