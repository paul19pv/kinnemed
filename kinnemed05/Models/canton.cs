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
    
    public partial class canton
    {
        public canton()
        {
            this.paciente = new HashSet<paciente>();
        }
    
        public int can_id { get; set; }
        public string can_nombre { get; set; }
        public Nullable<int> can_provincia { get; set; }
    
        public virtual provincia provincia { get; set; }
        public virtual ICollection<paciente> paciente { get; set; }
    }
}
