using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MotorOnline.Library.Entity;
using System.Web.Script.Serialization;
using MotorOnline.Helpers;

namespace MotorOnline
{
    public partial class Root : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AuthenticationHelper.HasAuthenticatedUser()) {
                User user = AuthenticationHelper.GetCurrentLoggedUser();
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