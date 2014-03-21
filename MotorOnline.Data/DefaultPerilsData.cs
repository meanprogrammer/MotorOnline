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
    public class DefaultPerilsData
    {
        Database db;
        public DefaultPerilsData() 
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public bool UpdatePerilDefault(PerilsDefault peril)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_updateperildefault");
            int result = 0;
            using (cmd)
            {
                db.AddInParameter(cmd, "@PerilID", System.Data.DbType.Int32, peril.PerilID);
                db.AddInParameter(cmd, "@LimitSIDefault", System.Data.DbType.Double, peril.LimitSIDefault);
                db.AddInParameter(cmd, "@LimitSIEditable", DbType.Boolean, peril.LimitSIEditable);
                db.AddInParameter(cmd, "@RateDefault", System.Data.DbType.Double, peril.RateDefault);
                db.AddInParameter(cmd, "@RateEditable", DbType.Boolean, peril.RateEditable);
                db.AddInParameter(cmd, "@RateShowTariffText", DbType.Boolean, peril.RateShowTariffText);
                db.AddInParameter(cmd, "@PremiumDefault", System.Data.DbType.Double, peril.PremiumDefault);
                db.AddInParameter(cmd, "@PolicyRateDefault", System.Data.DbType.Double, peril.PolicyRateDefault);
                db.AddInParameter(cmd, "@PolicyRateEditable", DbType.Boolean, peril.PolicyRateEditable);
                db.AddInParameter(cmd, "@PolicyRateShowTariffText", DbType.Boolean, peril.PolicyRateShowTariffText);
                db.AddInParameter(cmd, "@PolicyPremiumDefault", System.Data.DbType.Double, peril.PolicyPremiumDefault);
                db.AddInParameter(cmd, "@LastEditedBy", System.Data.DbType.Double, peril.LastEditedBy);
                result = db.ExecuteNonQuery(cmd);
            }
            return result > 0;
        }

        public List<PerilsDefault> GetPerilDefaults()
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_getallperildefault");
            IDataReader reader = db.ExecuteReader(cmd);
            List<PerilsDefault> pDefaults = new List<PerilsDefault>();
            using (reader)
            {
                int perilIdIdx = reader.GetOrdinal("PerilID");
                int limitSiDefaultIdx = reader.GetOrdinal("LimitSIDefault");
                int limitSiEditableIdx = reader.GetOrdinal("LimitSIEditable");
                int rateDefaultIdx = reader.GetOrdinal("RateDefault");
                int rateEditableIdx = reader.GetOrdinal("RateEditable");
                int rateShowTariffIdx = reader.GetOrdinal("RateShowTariffText");
                int premiumDefaultIdx = reader.GetOrdinal("PremiumDefault");
                int policyRateDefaultIdx = reader.GetOrdinal("PolicyRateDefault");
                int policyRateEditableIdx = reader.GetOrdinal("PolicyRateEditable");
                int policyRateShowTariffIdx = reader.GetOrdinal("PolicyRateShowTariffText");
                int policyPremiumIdx = reader.GetOrdinal("PolicyPremiumDefault");
                int lastEditedByIdx = reader.GetOrdinal("LastEditedBy");

                while (reader.Read())
                {
                    PerilsDefault pd = new PerilsDefault();
                    pd.PerilID = reader.GetInt32(perilIdIdx);
                    pd.LimitSIDefault = reader.GetDouble(limitSiDefaultIdx);
                    pd.LimitSIEditable = reader.GetBoolean(limitSiEditableIdx);
                    pd.RateDefault = reader.GetDouble(rateDefaultIdx);
                    pd.RateEditable = reader.GetBoolean(rateEditableIdx);
                    pd.RateShowTariffText = reader.GetBoolean(rateShowTariffIdx);
                    pd.PremiumDefault = reader.GetDouble(premiumDefaultIdx);
                    pd.PolicyRateDefault = reader.GetDouble(policyRateDefaultIdx);
                    pd.PolicyRateEditable = reader.GetBoolean(policyRateEditableIdx);
                    pd.PolicyRateShowTariffText = reader.GetBoolean(policyRateShowTariffIdx);
                    pd.PolicyPremiumDefault = reader.GetDouble(policyPremiumIdx);
                    pd.LastEditedBy = reader.GetInt32(lastEditedByIdx);

                    pDefaults.Add(pd);
                }
            }
            cmd.Dispose();
            return pDefaults;
        }

        public List<PerilsDefault> GetAllPerilsDefaults()
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_getallperildefault");
            IDataReader reader = db.ExecuteReader(cmd);
            List<PerilsDefault> defaults = new List<PerilsDefault>();
            using (reader)
            {
                int perilIDIdx = reader.GetOrdinal("PerilID");
                int limitSIDefaultIdx = reader.GetOrdinal("LimitSIDefault");
                int limitSIEditableIdx = reader.GetOrdinal("LimitSIEditable");
                int rateDefaultIdx = reader.GetOrdinal("RateDefault");
                int rateEditableIdx = reader.GetOrdinal("RateEditable");
                int rateShowTariffTextIdx = reader.GetOrdinal("RateShowTariffText");
                int premiumDefaultIdx = reader.GetOrdinal("PremiumDefault");
                int policyRateDefaultIdx = reader.GetOrdinal("PolicyRateDefault");
                int policyRateEditableIdx = reader.GetOrdinal("PolicyRateEditable");
                int policyRateShowTariffTextIdx = reader.GetOrdinal("PolicyRateShowTariffText");
                int policyPremiumDefaultIdx = reader.GetOrdinal("PolicyPremiumDefault");
                int lastEditedByIdx = reader.GetOrdinal("LastEditedBy");
                while (reader.Read())
                {
                    PerilsDefault pd = new PerilsDefault();
                    pd.PerilID = reader.GetInt32(perilIDIdx);
                    pd.LimitSIDefault = reader.GetDouble(limitSIDefaultIdx);
                    pd.LimitSIEditable = reader.GetBoolean(limitSIEditableIdx);
                    pd.RateDefault = reader.GetDouble(rateDefaultIdx);
                    pd.RateEditable = reader.GetBoolean(rateEditableIdx);
                    pd.RateShowTariffText = reader.GetBoolean(rateShowTariffTextIdx);
                    pd.PremiumDefault = reader.GetDouble(premiumDefaultIdx);
                    pd.PolicyRateDefault = reader.GetDouble(policyRateDefaultIdx);
                    pd.PolicyRateEditable = reader.GetBoolean(policyRateEditableIdx);
                    pd.PolicyRateShowTariffText = reader.GetBoolean(policyRateShowTariffTextIdx);
                    pd.PolicyPremiumDefault = reader.GetDouble(policyPremiumDefaultIdx);
                    pd.LastEditedBy = reader.GetInt32(lastEditedByIdx);

                    defaults.Add(pd);
                }
            }
            cmd.Dispose();
            return defaults;
        }
    }
}
