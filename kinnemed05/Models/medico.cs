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
    
    public partial class medico
    {
        public medico()
        {
            this.historia = new HashSet<historia>();
            this.registro = new HashSet<registro>();
        }
    
        public int med_id { get; set; }
    
        public virtual especialidad especialidad { get; set; }
        public virtual ICollection<historia> historia { get; set; }
        public virtual ICollection<registro> registro { get; set; }
    }
}
