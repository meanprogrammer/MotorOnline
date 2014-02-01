using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MotorOnline.Library.Entity;
namespace MotorOnline
{
    public class ReaderToEntity
    {

        internal static List<Perils> ConvertToListOfPerils(IDataReader reader)
        {
            List<Perils> ps = new List<Perils>();
            using (reader)
            {
                while (reader.Read())
                {
                    Perils p = new Perils();
                    p.PerilID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    p.LineID = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                    p.SubLineID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    p.PerilSName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                    p.PerilName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                    p.PerilLName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                    
                    p.IsActive = reader.IsDBNull(6) ? false : reader.GetBoolean(6);
                    p.PerilCode = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                    p.RI_COMM_RT = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8);
                    p.IsLimitFixed = reader.IsDBNull(9) ? false : reader.GetBoolean(9);
                    p.DefaultLimit = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                    p.RequiresLTOInterconn = reader.IsDBNull(11) ? false : reader.GetBoolean(11);
                    p.RequiresDSTonCOC = reader.IsDBNull(12) ? false : reader.GetBoolean(12);
                    p.LimitSI = reader.IsDBNull(13) ? 0 : reader.GetInt32(13);
                    p.Rate = reader.IsDBNull(14) ? 0 : reader.GetDouble(14);
                    p.Premium = reader.IsDBNull(15) ? 0 : reader.GetInt32(15);
                    p.PolicyRate = reader.IsDBNull(16) ? 0 : reader.GetDouble(16);
                    p.PolicyPremium = reader.IsDBNull(17) ? 0 : reader.GetDecimal(17);
                    p.Limit = reader.IsDBNull(18) ? 0 : reader.GetInt32(18);
                    p.PC = reader.IsDBNull(19) ? 0 : reader.GetInt32(19);
                    p.CVLightMedium = reader.IsDBNull(20) ? 0 : reader.GetInt32(20);
                    p.CVHeavy = reader.IsDBNull(21) ? 0 : reader.GetInt32(21);

                    p.PerilType = reader.IsDBNull(22) ? string.Empty : reader.GetString(22);
                    ps.Add(p);
                }
            }
            return ps;
        }

        public static List<TariffRate> ConvertToListOfTariffRate(IDataReader reader)
        {
            List<TariffRate> rates = new List<TariffRate>();
            using (reader)
            {
                while (reader.Read())
                {
                    TariffRate tr = new TariffRate();
                    tr.PeridID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    tr.Limit = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                    tr.PC = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    tr.CVLightMedium = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                    tr.CVHeavy = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    rates.Add(tr);
                }
            }
            return rates;
        }

        public static ComputationFactor ConvertToComputationFactor(IDataReader reader) 
        {
            ComputationFactor factor = new ComputationFactor();
            using (reader)
            {
                while (reader.Read())
                {
                    factor.DocumentaryStamps = reader.IsDBNull(0) ? 0 : reader.GetDouble(0);
                    factor.ValueAddedTax = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);
                    factor.LocalGovtTax = reader.IsDBNull(2) ? 0 : reader.GetDouble(2);
                    factor.DSTonCOC = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);
                    factor.LTOConnectivity = reader.IsDBNull(4) ? 0 : reader.GetDouble(4);
                    break;
                }
            }
            return factor;
        }


    }
}