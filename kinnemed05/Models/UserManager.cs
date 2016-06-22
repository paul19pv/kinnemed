using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace kinnemed05.Models
{
    public class UserManager
    {
        UsersContext db = new UsersContext();

        public int UpdateEmpresa(string usuario, int id) { 
            UserProfile updateUser=new UserProfile();
            var result = from u in db.UserProfiles where u.UserName==usuario select u;
            UserProfile original = result.First();
            original.UserEmpresa = id;
            db.SaveChanges();
            return original.UserId;
        }
        public int UpdateMedico(string usuario, int id) {
            UserProfile updateUser = new UserProfile();
            var result = from u in db.UserProfiles where u.UserName == usuario select u;
            UserProfile original = result.First();
            original.UserMedico = id;
            db.SaveChanges();
            return original.UserId;
        }
        public int UpdatePaciente(string usuario, int id)
        {
            UserProfile updateUser = new UserProfile();
            var result = from u in db.UserProfiles where u.UserName == usuario select u;
            UserProfile original = result.First();
            original.UserPaciente = id;
            db.SaveChanges();
            return original.UserId;
        }
        public int UpdateLaboratorista(string usuario, int id)
        {
            UserProfile updateUser = new UserProfile();
            var result = from u in db.UserProfiles where u.UserName == usuario select u;
            UserProfile original = result.First();
            original.UserLaboratorista = id;
            db.SaveChanges();
            return original.UserId;
        }
        public int UpdateTrabajador(string usuario, int id)
        {
            UserProfile updateUser = new UserProfile();
            var result = from u in db.UserProfiles where u.UserName == usuario select u;
            UserProfile original = result.First();
            original.UserTrabajador = id;
            db.SaveChanges();
            return original.UserId;
        }
        public int UserId(string usuario) {
            UserProfile updateUser = new UserProfile();
            var result = from u in db.UserProfiles where u.UserName == usuario select u;
            UserProfile original = result.First();
            return original.UserId;
        }

        public void DeleteUser(int user_id, int perfil) {
            UserProfile deleteuser = new UserProfile();

            if (perfil == 2)
                deleteuser = db.UserProfiles.Where(u => u.UserMedico == user_id).FirstOrDefault();
            else if (perfil == 3)
                deleteuser = db.UserProfiles.Where(u => u.UserPaciente == user_id).FirstOrDefault();
            else if (perfil == 4)
                deleteuser = db.UserProfiles.Where(u => u.UserEmpresa == user_id).FirstOrDefault();
            else if (perfil == 5)
                deleteuser = db.UserProfiles.Where(u => u.UserLaboratorista == user_id).FirstOrDefault();
            else if (perfil == 6)
                deleteuser = db.UserProfiles.Where(u => u.UserTrabajador == user_id).FirstOrDefault();

            if(deleteuser!=null)
                db.UserProfiles.Remove(deleteuser);
            db.SaveChanges();


        }
        public bool there_users() {
            return (from u in db.UserProfiles select u).Any();
        }

        //public string get_perfil(IPrincipal User)
        //{
        //    string perfil = String.Empty;
        //    if(User.IsInRole("admin"))
        //        perfil="admin";
        //    else if(User.IsInRole("medico"))
        //        perfil="medico";
        //    else if (User.IsInRole("paciente"))
        //        perfil = "paciente";
        //    else if (User.IsInRole("empresa"))
        //        perfil = "empresa";
        //    else if (User.IsInRole("laboratorista"))
        //        perfil = "laboratorista";
        //    else if (User.IsInRole("trabajador"))
        //        perfil = "trabajador";
        //    return perfil;
        //}

        public int get_perfil(IPrincipal User) {
            int perfil = 0;
            if (User.IsInRole("admin"))
                perfil = 1;
            else if (User.IsInRole("medico"))
                perfil = 2;
            else if (User.IsInRole("paciente"))
                perfil = 3;
            else if (User.IsInRole("empresa"))
                perfil = 4;
            else if (User.IsInRole("laboratorista"))
                perfil = 5;
            else if (User.IsInRole("trabajador"))
                perfil = 6;
            return perfil;
        }
        public int get_user_id(IPrincipal User) {
            int user_id = 0;
            UserProfile usuario = db.UserProfiles.Where(u => u.UserName == User.Identity.Name).First();


            if (User.IsInRole("medico"))
                user_id = (int)usuario.UserMedico;
            else if (User.IsInRole("paciente"))
                user_id = (int)usuario.UserPaciente;
            else if (User.IsInRole("empresa"))
                user_id = (int)usuario.UserEmpresa;
            else if (User.IsInRole("laboratorista"))
                user_id = (int)usuario.UserLaboratorista;
            else if (User.IsInRole("trabajador"))
                user_id = (int)usuario.UserTrabajador;
            return user_id;
        }

    }
}