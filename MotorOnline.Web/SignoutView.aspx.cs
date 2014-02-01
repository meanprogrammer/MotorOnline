using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MotorOnline.Web
{
    public partial class SignoutView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[string.Format("user_{0}", Session.SessionID)] != null) {
                Session.Remove(string.Format("user_{0}", Session.SessionID));
            }
        }
    }
}