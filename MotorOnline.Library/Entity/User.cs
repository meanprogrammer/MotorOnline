using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Library.Entity
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public int RoleID { get; set; }
        public DateTime LastActivityDate { get; set; }
        public UserRole UserRole { get; set; }

        public string FormattedLastActivityDate {
            get {
                return this.LastActivityDate.ToString();
            }
        }
    }
}
