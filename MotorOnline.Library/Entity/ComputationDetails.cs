using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorOnline.Library.Entity
{
    public class ComputationDetails
    {
        public double BasicPremium { get; set; }
        public double DocumentaryStamps { get; set; }
        public double ValueAddedTax { get; set; }
        public double LocalGovernmentTax { get; set; }
        public double DSTonCOC { get; set; }
        public double LTOInterconnectivity { get; set; }
        public double GrandTotal { get; set; }
    }
}