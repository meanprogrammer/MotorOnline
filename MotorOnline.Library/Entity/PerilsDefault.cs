using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Library.Entity
{
    public class PerilsDefault
    {
        public int PerilID { get; set; }
        public double LimitSIDefault { get; set; }
        public bool LimitSIEditable { get; set; }
        public double RateDefault { get; set; }
        public bool RateEditable { get; set; }
        public bool RateShowTariffText { get; set; }
        public double PremiumDefault { get; set; }
        public double PolicyRateDefault { get; set; }
        public bool PolicyRateEditable { get; set; }
        public bool PolicyRateShowTariffText { get; set; }
        public double PolicyPremiumDefault { get; set; }
        public int LastEditedBy { get; set; }
    }
}
