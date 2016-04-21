using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kinnemed05.Security
{
    
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorize : AuthorizeAttribute
    {
        private string[] UserProfilesRequired { get; set; }

        public CustomAuthorize(params object[] userProfilesRequired)
        {
            if (userProfilesRequired.Any(p => p.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException("userProfilesRequired");

            this.UserProfilesRequired = userProfilesRequired.Select(p => Enum.GetName(p.GetType(), p)).ToArray();
        }

        public override void OnAuthorization(AuthorizationContext context)
        {
            bool authorized = false;

            foreach (var role in this.UserProfilesRequired)
                if (HttpContext.Current.User.IsInRole(role))
                {
                    authorized = true;
                    break;
                }

            if (!authorized)
            {
                var url = new UrlHelper(context.RequestContext);
                //var logonUrl = url.Action("Http", "Error", new { Id = 401, Area = "" });
                string url_logon = "";
                if (HttpContext.Current.Request.IsAuthenticated)
                    url_logon=url.Action("Message","Home",new {mensaje="Su perfil de acceso no tiene permisos para esta acción"});
                else
                    url_logon = url.Action("Login", "Account");
                context.Result = new RedirectResult(url_logon);

                return;
            }
        }
    }
}