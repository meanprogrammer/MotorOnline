using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotorOnline.Library.Entity;
using MotorOnline.Data;

namespace MotorOnline.Business
{
    public class CustomerInfoBusiness
    {
        public List<CustomerInfo> GetNamesAutocomplete()
        {
            return DataFacade.Data.CustomerInfoData.GetNamesAutocomplete();
        }
    }
}
