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
    public class TransactionPerilsData
    {
        Database db;
        public TransactionPerilsData() {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<TransactionPeril> GetTransactionPerilsByTransactionId(int transactionId)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_get_perilsbytransactionid");
            db.AddInParameter(cmd, "@TransactionID", System.Data.DbType.Int32,  transactionId);
            List<TransactionPeril> perils = new List<TransactionPeril>();
            using (cmd)
            {
                IDataReader reader = db.ExecuteReader(cmd);
                using (reader)
                {
                    int limitsiEditableIdx = reader.GetOrdinal("LimitSIEditable");
                    int rateEditableIdx = reader.GetOrdinal("RateEditable");
                    int rateShowTariffIdx = reader.GetOrdinal("RateShowTariffText");
                    int policyrateEditableIdx = reader.GetOrdinal("PolicyRateEditable");
                    int policyrateShowTariffEditableIdx = reader.GetOrdinal("PolicyRateShowTariffText");

                    while (reader.Read())
                    {
                        TransactionPeril tp = new TransactionPeril();

                        tp.TransactionID = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                        tp.PerilID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                        tp.NewLimitSI = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);
                        tp.NewRate = reader.IsDBNull(4) ? 0 : reader.GetDouble(4);
                        tp.NewPremium = reader.IsDBNull(5) ? 0 : reader.GetDouble(5);
                        tp.NewPolicyRate = reader.IsDBNull(6) ? 0 : reader.GetDouble(6);
                        tp.NewPolicyPremium = reader.IsDBNull(7) ? 0 : reader.GetDouble(7);
                        //For the parent Peril
                        tp.LineID = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                        tp.SubLineID = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                        tp.PerilSName = reader.IsDBNull(11) ? string.Empty : reader.GetString(11);
                        tp.PerilName = reader.IsDBNull(12) ? string.Empty : reader.GetString(12);
                        tp.PerilLName = reader.IsDBNull(13) ? string.Empty : reader.GetString(13);



                        tp.IsActive = reader.IsDBNull(14) ? false : reader.GetBoolean(14);
                        tp.PerilCode = reader.IsDBNull(15) ? 0 : reader.GetInt32(15);
                        tp.RI_COMM_RT = reader.IsDBNull(16) ? 0 : reader.GetDecimal(16);
                        tp.IsLimitFixed = reader.IsDBNull(17) ? false : reader.GetBoolean(17);
                        tp.DefaultLimit = reader.IsDBNull(18) ? string.Empty : reader.GetString(18);
                        tp.RequiresLTOInterconn = reader.IsDBNull(19) ? false : reader.GetBoolean(19);
                        tp.RequiresDSTonCOC = reader.IsDBNull(20) ? false : reader.GetBoolean(20);
                        tp.LimitSI = reader.IsDBNull(21) ? 0 : reader.GetInt32(21);
                        tp.Rate = reader.IsDBNull(22) ? 0 : reader.GetDouble(22);
                        tp.Premium = reader.IsDBNull(23) ? 0 : reader.GetInt32(23);
                        tp.PolicyRate = reader.IsDBNull(24) ? 0 : reader.GetDouble(24);
                        tp.PolicyPremium = reader.IsDBNull(25) ? 0 : reader.GetInt32(25);
                        tp.Limit = reader.IsDBNull(26) ? 0 : reader.GetInt32(26);
                        tp.PC = reader.IsDBNull(27) ? 0 : reader.GetInt32(27);
                        tp.CVLightMedium = reader.IsDBNull(28) ? 0 : reader.GetInt32(28);
                        tp.CVHeavy = reader.IsDBNull(29) ? 0 : reader.GetInt32(29);

                        tp.PerilType = reader.IsDBNull(30) ? string.Empty : reader.GetString(30);

                        tp.LimitSIEditable = reader.GetBoolean(limitsiEditableIdx);
                        tp.RateEditable = reader.GetBoolean(rateEditableIdx);
                        tp.RateShowTariffText = reader.GetBoolean(rateShowTariffIdx);
                        tp.PolicyRateEditable = reader.GetBoolean(policyrateEditableIdx);
                        tp.PolicyRateShowTariffText = reader.GetBoolean(policyrateShowTariffEditableIdx);

                        perils.Add(tp);
                    }
                }
            }
            return perils;
        }
    }
}
