using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorOnline
{
    public class CustomerInfo
    {
        public int CustomerID { get; set; }
        public string Designation { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string MultipleCorporateName { get; set; }

        //For json purpose only
        public string label
        {
            get { return string.Format("{0}, {1} {2}", this.LastName, this.FirstName, this.MiddleName); }
        }

        public string value { get { return this.LastName; } }
    }
}