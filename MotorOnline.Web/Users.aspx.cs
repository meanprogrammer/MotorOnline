using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MotorOnline.Library.Entity;

namespace MotorOnline.Web
{
    public partial class Users : System.Web.UI.Page
    {
        private User _currentUser;
        public User CurrentUser
        {
            get
            {
                return Session[string.Format("user_{0}", Session.SessionID)] as User;
            }
            set
            {
                this._currentUser = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.CurrentUser.UserRole.CanAddUser &&
                !this.CurrentUser.UserRole.CanEditUser &&
                !this.CurrentUser.UserRole.CanDeleteUser) {
                Response.Redirect("NotAllowed.aspx", true);
            }
        }
    }
}