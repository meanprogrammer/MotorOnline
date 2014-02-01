using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorOnline
{
    public class TariffRate
    {
        public int PeridID { get; set; }
        public int Limit { get; set; }
        public int PC { get; set; }
        public int CVLightMedium { get; set; }
        public int CVHeavy { get; set; }
    }
}