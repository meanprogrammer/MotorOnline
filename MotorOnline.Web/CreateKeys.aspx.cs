﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MotorOnline.Library.Entity;

namespace MotorOnline.Web
{
    public partial class CreateKeys : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //cls_data_access_layer dl = new cls_data_access_layer();
            //dl.SaveKeys();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            User u = new User();
            u.Username = "vdudan";
            u.Password = "pass1234";
            u.FirstName = "valiant";
            u.LastName = "dudan";
            u.MI = "a";
            u.RoleID = 0;
            u.LastActivityDate = DateTime.Now;

            cls_data_access_layer da = new cls_data_access_layer();
            //da.SaveUser(u);
        }
    }
}