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
    
    public partial class audiometria
    {
        public int aud_id { get; set; }

        public virtual medico medico { get; set; }
        public virtual paciente paciente { get; set; }
    }
}
