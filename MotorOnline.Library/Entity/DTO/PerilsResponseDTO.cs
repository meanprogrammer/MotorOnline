using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotorOnline.Library.Entity
{
    public class PerilsResponseDTO
    {
        public List<PerilsDefault> PerilDefaults { get; set; }
        public List<Perils> Perils { get; set; }
        public DefaultTariffRateResponse Tariff { get; set; }
    }
}
