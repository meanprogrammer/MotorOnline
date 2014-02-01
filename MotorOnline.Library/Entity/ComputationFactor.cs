using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorOnline.Library.Entity
{
    public class ComputationFactor
    {
        public double DocumentaryStamps { get; set; }
        public double ValueAddedTax { get; set; }
        public double LocalGovtTax { get; set; }
        public double DSTonCOC { get; set; }
        public double LTOConnectivity { get; set; }
    }
}