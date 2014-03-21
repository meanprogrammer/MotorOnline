using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotorOnline.Data;
using MotorOnline.Library.Entity;

namespace MotorOnline.Business
{
    public class DefaultPerilsBusiness
    {
        public DefaultPerilsBusiness() {
        }

        public bool UpdatePerilDefault(PerilsDefault peril)
        {
            return DataFacade.Data.DefaultPerilsData.UpdatePerilDefault(peril);
        }

        public List<PerilsDefault> GetPerilDefaults()
        {
            return DataFacade.Data.DefaultPerilsData.GetPerilDefaults();
        }

        public List<PerilsDefault> GetAllPerilsDefaults()
        {
            return DataFacade.Data.DefaultPerilsData.GetAllPerilsDefaults();
        }
    }
}
