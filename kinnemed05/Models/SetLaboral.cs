using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public class SetLaboral
    {
        public IEnumerable<laboral> laboral { get; set; }
        public IEnumerable<laboral> biologicos { get; set; }
        public IEnumerable<laboral> biomecanicos { get; set; }
        public IEnumerable<laboral> fisico { get; set; }
        public IEnumerable<laboral> mecanicos { get; set; }
        public IEnumerable<laboral> psicosociales { get; set; }
        public IEnumerable<laboral> quimicos { get; set; }
    }
}