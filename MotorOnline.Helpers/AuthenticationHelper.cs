using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MotorOnline.Library.Entity;

namespace MotorOnline.Helpers
{
    public class AuthenticationHelper
    {
        public static bool HasAuthenticatedUser()
        {
            bool result = false;
            if (HttpContext.Current.Session[string.Format("user_{0}",
                HttpContext.Current.Session.SessionID)] != null)
            {
                User u = HttpContext.Current.Session[string.Format("user_{0}",
                HttpContext.Current.Session.SessionID)] as User;

                if (u != null) { result = true; }
            }
            else
            {
                HttpContext.Current.Response.Redirect("LoginView.aspx", true);
            }
            return result;
        }

        public static User GetCurrentLoggedUser() {
            if (HttpContext.Current.Session[string.Format("user_{0}",
                HttpContext.Current.Session.SessionID)] != null)
            {
                User u = HttpContext.Current.Session[string.Format("user_{0}",
                HttpContext.Current.Session.SessionID)] as User;

                if (u != null) { return u; }
            }
            return null;
        }
    }
}
