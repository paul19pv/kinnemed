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
    
    public partial class control
    {
        public int con_id { get; set; }
    
        public virtual perfil perfil { get; set; }
        public virtual examen examen { get; set; }
    }
}
