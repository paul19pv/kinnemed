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
    
    public partial class historia
    {
        public historia()
        {
            this.diagnostico = new HashSet<diagnostico>();
            this.subsecuente = new HashSet<subsecuente>();
        }
    
        public int his_id { get; set; }
    
        public virtual ICollection<diagnostico> diagnostico { get; set; }
        public virtual fisico fisico { get; set; }
        public virtual plan plan { get; set; }
        public virtual revision revision { get; set; }
        public virtual signos signos { get; set; }
        public virtual ICollection<subsecuente> subsecuente { get; set; }
        public virtual concepto concepto { get; set; }
        public virtual medico medico { get; set; }
        public virtual paciente paciente { get; set; }
    }
}
