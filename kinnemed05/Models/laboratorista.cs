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
    
    public partial class laboratorista
    {
        public laboratorista()
        {
            this.registro = new HashSet<registro>();
        }
    
        public int lab_id { get; set; }
    
        public virtual ICollection<registro> registro { get; set; }
    }
}