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
    
    public partial class area
    {
        public area()
        {
            this.examen = new HashSet<examen>();
            this.codigo = new HashSet<codigo>();
        }
    
        public int are_id { get; set; }
        public string are_nombre { get; set; }
        public string are_tipo { get; set; }
    
        public virtual ICollection<examen> examen { get; set; }
        public virtual ICollection<codigo> codigo { get; set; }
    }
}
