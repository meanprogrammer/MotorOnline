using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Library.Entity
{
    public class TransactionExtendedDTO
    {
        public Transaction Transaction { get; set; }
        public List<DropDownListItem> CarCompanies { get; set; }
        public List<DropDownListItem> CarMakes { get; set; }
        public List<DropDownListItem> CarEngines { get; set; }
        public DefaultTariffRateResponse Tariff { get; set; }
    }
}
