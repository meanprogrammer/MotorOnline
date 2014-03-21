using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotorOnline.Library.Entity;
using MotorOnline.Data;

namespace MotorOnline.Business
{
    public class MiscBusiness
    {
        public ComputationFactor GetComputationFactors()
        {
            return DataFacade.Data.MiscData.GetComputationFactors();
        }

        public string GetLastParNo()
        {
            return DataFacade.Data.MiscData.GetLastParNo();
        }

        public string GetLastPolicyNo()
        {
            return DataFacade.Data.MiscData.GetLastPolicyNo();
        }
    }
}
