using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MotorOnline.Library.Entity;
using System.Web.Script.Serialization;
using MotorOnline.Helpers;

namespace MotorOnline.Web
{
    public partial class Root : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //To avoid login for now
            User user = new User()
            {
                FirstName = "Valiant",
                LastName = "Dudan",
                MI = "A",
                Username = "meanprogrammer",
                RoleID = 1,
                UserRole = new UserRole()
                {
                    CanAddTransaction = true,
                    CanAddUser = true,
                    CanDeleteTransaction = true,
                    CanDeleteUser = true,
                    CanEditPerils = true,
                    CanEditTransaction = true,
                    CanEditUser = true,
                    CanPostTransaction = true,
                    CanViewTransaction = true,
                    CanEndorse = true
                }
            };

            Session.Add(string.Format("user_{0}", Session.SessionID), user);

            if (AuthenticationHelper.HasAuthenticatedUser())
            {
                //User user = AuthenticationHelper.GetCurrentLoggedUser();
                if (user != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "renderwelcome",
                        string.Format("renderwelcome('{0}');", serializer.Serialize(user)), true);
                }
            }
        }
    }
}