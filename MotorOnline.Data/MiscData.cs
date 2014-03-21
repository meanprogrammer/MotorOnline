using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MotorOnline.Library.Entity;
using System.Data.Common;
using System.Data;

namespace MotorOnline.Data
{
    public class MiscData
    {
        Database db;
        public MiscData() 
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public ComputationFactor GetComputationFactors()
        {
            ComputationFactor factor = new ComputationFactor();
            DbCommand cmd = db.GetStoredProcCommand("sp_getComputationFactors");
            using (cmd)
            {
                IDataReader reader = db.ExecuteReader(cmd);
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
            }
            return factor;
        }

        public string GetLastParNo()
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_getlastparno");
            object result;
            using (cmd)
            {
                result = db.ExecuteScalar(cmd);
            }
            return result.ToString();
        }

        public string GetLastPolicyNo()
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_getlastpolicyno");
            object result;
            using (cmd)
            {
                result = db.ExecuteScalar(cmd);
            }
            return result.ToString();
        }
    }
}
