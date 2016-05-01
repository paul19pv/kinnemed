using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}