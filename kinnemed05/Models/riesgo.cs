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
    
    public partial class riesgo
    {
        public riesgo()
        {
            this.laboral = new HashSet<laboral>();
        }
    
        public int rie_id { get; set; }
        public string rie_grupo { get; set; }
        public string rie_nombre { get; set; }
    
        public virtual ICollection<laboral> laboral { get; set; }
    }
}
