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
    public class TransactionComputationData
    {
        Database db;
        public TransactionComputationData()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public TransactionComputation GetTransactionComputationsByTransactionId(int transactionId)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_get_transactioncomputationsbytransactionid");
            db.AddInParameter(cmd, "@TransactionID", DbType.Int32, transactionId);
            TransactionComputation c = new TransactionComputation();
            using (cmd)
            {
                IDataReader reader = db.ExecuteReader(cmd);
                using (reader)
                {
                    while (reader.Read())
                    {
                        c.TransactionID = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                        //NET
                        c.NetComputationDetails.BasicPremium = reader.IsDBNull(2) ? 0 : reader.GetDouble(2);
                        c.NetComputationDetails.DocumentaryStamps = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);
                        c.NetComputationDetails.ValueAddedTax = reader.IsDBNull(4) ? 0 : reader.GetDouble(4);
                        c.NetComputationDetails.DSTonCOC = reader.IsDBNull(5) ? 0 : reader.GetDouble(5);
                        c.NetComputationDetails.LTOInterconnectivity = reader.IsDBNull(6) ? 0 : reader.GetDouble(6);
                        c.NetComputationDetails.GrandTotal = reader.IsDBNull(7) ? 0 : reader.GetDouble(7);
                        //GROSS
                        c.GrossComputationDetails.BasicPremium = reader.IsDBNull(8) ? 0 : reader.GetDouble(8);
                        c.GrossComputationDetails.DocumentaryStamps = reader.IsDBNull(9) ? 0 : reader.GetDouble(9);
                        c.GrossComputationDetails.ValueAddedTax = reader.IsDBNull(10) ? 0 : reader.GetDouble(10);
                        c.GrossComputationDetails.DSTonCOC = reader.IsDBNull(11) ? 0 : reader.GetDouble(11);
                        c.GrossComputationDetails.LTOInterconnectivity = reader.IsDBNull(12) ? 0 : reader.GetDouble(12);
                        c.GrossComputationDetails.GrandTotal = reader.IsDBNull(13) ? 0 : reader.GetDouble(13);
                    }
                }
            }
            return c;
        }
    }
}
