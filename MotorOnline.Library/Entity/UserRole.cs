using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Library.Entity
{
    public class UserRole
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public bool CanAddTransaction { get; set; }
        public bool CanEditTransaction { get; set; }

        public bool CanViewTransaction { get; set; }
        public bool CanDeleteTransaction { get; set; }
        public bool CanPostTransaction { get; set; }
        public bool CanAddUser { get; set; }
        public bool CanEditUser { get; set; }
        public bool CanDeleteUser { get; set; }
        public bool CanEditPerils { get; set; }
        public bool CanEndorse { get; set; }
    }
}
