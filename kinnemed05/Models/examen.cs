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
    
    public partial class examen
    {
        public examen()
        {
            this.control = new HashSet<control>();
            this.prueba = new HashSet<prueba>();
            this.valores = new HashSet<valores>();
            this.orden = new HashSet<orden>();
        }
    
        public int exa_id { get; set; }
    
        public virtual area area { get; set; }
        public virtual ICollection<control> control { get; set; }
        public virtual ICollection<prueba> prueba { get; set; }
        public virtual ICollection<valores> valores { get; set; }
        public virtual ICollection<orden> orden { get; set; }
    }
}
