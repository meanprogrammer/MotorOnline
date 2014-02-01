using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorOnline
{
    public class CarDetail
    {
        public int TransactionID { get; set; }

        public int CarCompany { get; set; }
        public int CarYear { get; set; }
        public int CarSeries { get; set; }
        public string CarMake { get; set; }

        public int CarTypeOfBodyID { get; set; }
        public int TypeOfCover { get; set; }
        public string EngineSeries { get; set; }
        public string MotorType { get; set; }
        public string EngineNo { get; set; }
        public string Color { get; set; }
        public string ConductionNo { get; set; }
        public string ChassisNo { get; set; }
        public string PlateNo { get; set; }
        public string Accessories { get; set; }
  

        //This text/value pair are mostly used for json
        public string CarCompanyText { get; set; }
        public int CarCompanyValue { get; set; }

        public string CarMakeText { get; set; }
        public string CarMakeValue { get; set; }

        public string TypeOfCoverText { get; set; }
        public int TypeOfCoverValue { get; set; }

        public string CarSeriesText { get; set; }
        public int CarSeriesValue { get; set; }

        public string EngineSeriesText { get; set; }
        public int EngineSeriesValue { get; set; }
    }
}