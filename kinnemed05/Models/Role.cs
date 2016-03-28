using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public class Roles
    {
        public const string admin = "admin";
        public const string medico = "medico";
        public const string paciente = "paciente";
        public const string empresa = "empresa";
        
    }

    public enum UserRoles
    {
        admin = 1,
        medico = 2,
        paciente=3,
        empresa=4
    }
}