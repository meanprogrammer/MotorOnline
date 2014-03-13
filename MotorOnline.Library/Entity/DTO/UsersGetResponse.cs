using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Library.Entity
{
    public class UsersGetResponse
    {
        public List<User> Users { get; set; }
        public List<DropDownListItem> Roles { get; set; }
        public User CurrentUser { get; set; }
    }
}
