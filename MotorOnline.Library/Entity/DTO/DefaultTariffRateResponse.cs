using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorOnline.Library.Entity
{
    public class DefaultTariffRateResponse
    {
        public DefaultTariffRateResponse() {
            this.DropdownValues = new Dictionary<string, List<DropDownListItem>>();
        }

        public Dictionary<string,List<DropDownListItem>> DropdownValues { get; set; }
        public string CTPLDefault { get; set; }
    }
}