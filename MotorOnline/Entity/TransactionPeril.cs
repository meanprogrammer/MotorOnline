using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorOnline
{
    public class TransactionPeril : Perils
    {
        public double NewLimitSI { get; set; }
        public double NewRate { get; set; }
        public double NewPremium { get; set; }
        public double NewPolicyRate { get; set; }
        public double NewPolicyPremium { get; set; }

        public int TransactionID { get; set; }
    }
}