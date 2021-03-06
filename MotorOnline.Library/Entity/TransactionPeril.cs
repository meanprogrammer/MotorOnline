﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorOnline.Library.Entity
{
    public class TransactionPeril : Perils
    {
        public double NewLimitSI { get; set; }
        public double NewRate { get; set; }
        public double NewPremium { get; set; }
        public double NewPolicyRate { get; set; }
        public double NewPolicyPremium { get; set; }

        public bool LimitSIEditable { get; set; }
        public bool RateEditable { get; set; }
        public bool RateShowTariffText { get; set; }
        public bool PolicyRateEditable { get; set; }
        public bool PolicyRateShowTariffText { get; set; }
        
        public int TransactionID { get; set; }
    }
}