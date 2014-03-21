using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MotorOnline.Library.Entity;
using MotorOnline.Business;

namespace MotorOnline.Web
{
    public partial class LoginView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            UserBusiness userBusiness = new UserBusiness();
            User user = userBusiness.AuthenticateUser(this.UserName.Text.Trim(), this.Password.Text);
            if (user != null)
            {
                Session.Add(string.Format("user_{0}", Session.SessionID), user);
                Response.Redirect("~/AllTransactionsView.aspx", true);
            }
            else
            {
                this.FailureText.Text = "<div class=\"alert alert-danger\">Username/Password is incorrect. <strong>Access Denied.</strong></div>";
            }
        }
    }
}