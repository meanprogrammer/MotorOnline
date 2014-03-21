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
    public class EndorsementData
    {
        Database db;
        public EndorsementData() 
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public Endorsement GetOneEndorsement(int endorsementCode)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_getendorsementbycode");
            db.AddInParameter(cmd, "@endtCode", System.Data.DbType.Int32, endorsementCode);
            IDataReader reader = db.ExecuteReader(cmd);
            Endorsement e = new Endorsement();
            using (reader)
            {
                int codeIdx = reader.GetOrdinal("endtCode");
                int titleIdx = reader.GetOrdinal("endtTitle");
                int textIdx = reader.GetOrdinal("endtText");
                while (reader.Read())
                {
                    e.EndorsementCode = reader.GetInt32(codeIdx);
                    e.EndorsementTitle = reader.IsDBNull(titleIdx) ? string.Empty : reader.GetString(titleIdx);
                    e.EndorsementText = reader.IsDBNull(textIdx) ? string.Empty : reader.GetString(textIdx);
                }
            }
            cmd.Dispose();
            return e;
        }

        public EndorsementDetail GetEndorsementDetail(int id)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_getendorsementdetailbyid");
            db.AddInParameter(cmd, "@NewTransactionID", DbType.Int32, id);
            EndorsementDetail endorsementDetail = null;
            using (cmd)
            {
                IDataReader reader = db.ExecuteReader(cmd);
                
                using (reader)
                {
                    int parentIdIdx = reader.GetOrdinal("ParentTransactionID");
                    int newIdIdx = reader.GetOrdinal("NewTransactionID");
                    int eTextIdx = reader.GetOrdinal("EndorsementText");
                    int dateEndorsedId = reader.GetOrdinal("DateEndorsed");
                    int effectivityDateIdx = reader.GetOrdinal("EffectivityDate");
                    int expDateIdx = reader.GetOrdinal("ExpiryDate");
                    int endorsementTypeIdx = reader.GetOrdinal("EndorsementType");
                    while (reader.Read())
                    {
                        endorsementDetail = new EndorsementDetail();
                        endorsementDetail.ParentTransactionID = reader.GetInt32(parentIdIdx);
                        endorsementDetail.NewTransactionID = reader.GetInt32(newIdIdx);
                        endorsementDetail.EndorsementText = reader.GetString(eTextIdx);
                        endorsementDetail.DateEndorsed = reader.GetDateTime(dateEndorsedId);
                        endorsementDetail.EffectivityDate = reader.GetDateTime(effectivityDateIdx);
                        endorsementDetail.ExpiryDate = reader.GetDateTime(expDateIdx);
                        endorsementDetail.EndorsementType = reader.GetInt32(endorsementTypeIdx);
                    }
                }
            }
            return endorsementDetail;

        }

        public bool SaveEndorsementDetails(int transactionId, int newTransactionId,
                    string endorsementText, DateTime dateEndorsed,
                    DateTime effectivityDate, DateTime expiryDate,
                    int endorsementType)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_saveendorsementdetails");
            db.AddInParameter(cmd, "@ParentTransactionID", DbType.Int32, transactionId);
            db.AddInParameter(cmd, "@NewTransactionID", DbType.Int32, newTransactionId);
            db.AddInParameter(cmd, "@EndorsementText", DbType.String, endorsementText);
            db.AddInParameter(cmd, "@DateEndorsed", DbType.DateTime, dateEndorsed);
            db.AddInParameter(cmd, "@EffectivityDate", DbType.DateTime, effectivityDate);
            db.AddInParameter(cmd, "@ExpiryDate", DbType.DateTime, expiryDate);
            db.AddInParameter(cmd, "@EndorsementType", DbType.Int32, endorsementType);
            int result = 0;
            using (cmd)
            {
                result = db.ExecuteNonQuery(cmd);
            }
            return result > 0;

        }


        public List<Endorsement> GetAllEndorsement()
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_getallendorsement");
            List<Endorsement> list = new List<Endorsement>();
            using (cmd)
            {
                IDataReader reader = db.ExecuteReader(cmd);
                using (reader)
                {
                    int codeIdx = reader.GetOrdinal("endtCode");
                    int titleIdx = reader.GetOrdinal("endtTitle");
                    while (reader.Read())
                    {
                        Endorsement e = new Endorsement();
                        e.EndorsementCode = reader.GetInt32(codeIdx);
                        e.EndorsementTitle = reader.IsDBNull(titleIdx) ? string.Empty : reader.GetString(titleIdx);
                        list.Add(e);
                    }
                }
            }
            return list;
        }

        public Dictionary<string, EndorsementHistory> GetEndorsementHistory(int transactionId)
        {
            Dictionary<string, EndorsementHistory> history = new Dictionary<string, EndorsementHistory>();
            DbCommand cmd = db.GetStoredProcCommand("sp_getendorsementhistory");
            db.AddInParameter(cmd, "@CurrentTransactionID", DbType.Int32, transactionId);
            using (cmd)
            {
                IDataReader reader = db.ExecuteReader(cmd);
                using (reader)
                {
                    int parentIdIdx = reader.GetOrdinal("ParentTransactionID");
                    int newIdIdx = reader.GetOrdinal("NewTransactionID");
                    int eTextIdx = reader.GetOrdinal("EndorsementText");
                    int dateEndorsedId = reader.GetOrdinal("DateEndorsed");
                    int effectivityDateIdx = reader.GetOrdinal("EffectivityDate");
                    int expDateIdx = reader.GetOrdinal("ExpiryDate");
                    int endorsementTypeIdx = reader.GetOrdinal("EndorsementType");
                    int endorsementTitleIdx = reader.GetOrdinal("endtTitle");

                    while (reader.Read())
                    {
                        EndorsementHistory parent = new EndorsementHistory();
                        parent.Endorsement.ParentTransactionID = reader.GetInt32(parentIdIdx);
                        parent.Endorsement.NewTransactionID = reader.GetInt32(newIdIdx);
                        parent.Endorsement.EndorsementText = reader.GetString(eTextIdx);
                        parent.Endorsement.DateEndorsed = reader.GetDateTime(dateEndorsedId);
                        parent.Endorsement.EffectivityDate = reader.GetDateTime(effectivityDateIdx);
                        parent.Endorsement.ExpiryDate = reader.GetDateTime(expDateIdx);
                        parent.Endorsement.EndorsementType = reader.GetInt32(endorsementTypeIdx);
                        parent.EndorsementTitle = reader.GetString(endorsementTitleIdx);
                        history.Add("Parent", parent);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        EndorsementHistory child = new EndorsementHistory();
                        child.Endorsement.ParentTransactionID = reader.GetInt32(parentIdIdx);
                        child.Endorsement.NewTransactionID = reader.GetInt32(newIdIdx);
                        child.Endorsement.EndorsementText = reader.GetString(eTextIdx);
                        child.Endorsement.DateEndorsed = reader.GetDateTime(dateEndorsedId);
                        child.Endorsement.EffectivityDate = reader.GetDateTime(effectivityDateIdx);
                        child.Endorsement.ExpiryDate = reader.GetDateTime(expDateIdx);
                        child.Endorsement.EndorsementType = reader.GetInt32(endorsementTypeIdx);
                        child.EndorsementTitle = reader.GetString(endorsementTitleIdx);
                        history.Add("Child", child);
                    }

                }
            }
            return history;
        }

        public int SaveTransactionWithUpdatedAddress(int transactionId,
                    string newPolicyNo, int customerId, out int newId,
                    string newAddress)
        {

            DbCommand cmd = db.GetStoredProcCommand("sp_saveendorsement");
            db.AddInParameter(cmd, "@OldTransID", DbType.Int32, transactionId);
            db.AddInParameter(cmd, "@NewPolicyNo", DbType.String, newPolicyNo);
            db.AddOutParameter(cmd, "@NewTransID", DbType.Int32, int.MaxValue);

            db.ExecuteNonQuery(cmd);
            newId = 0;
            newId = (int)db.GetParameterValue(cmd, "@NewTransID");
            cmd.Dispose();
            return UpdateAddress(customerId, newAddress);

        }

        public int SaveTransactionWithUpdatedMortgagee(int transactionId,
            string newPolicyNo, out int newId,
            string mortgagee)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_saveendorsement");
            db.AddInParameter(cmd, "@OldTransID", DbType.Int32, transactionId);
            db.AddInParameter(cmd, "@NewPolicyNo", DbType.String, newPolicyNo);
            db.AddOutParameter(cmd, "@NewTransID", DbType.Int32, int.MaxValue);

            db.ExecuteNonQuery(cmd);
            newId = 0;
            newId = (int)db.GetParameterValue(cmd, "@NewTransID");
            cmd.Dispose();
            return UpdateMortgagee(newId, mortgagee);

        }

        public int SaveTransactionWithDeleteMortgagee(int transactionId,
            string newPolicyNo, out int newId)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_saveendorsement");
            db.AddInParameter(cmd, "@OldTransID", DbType.Int32, transactionId);
            db.AddInParameter(cmd, "@NewPolicyNo", DbType.String, newPolicyNo);
            db.AddOutParameter(cmd, "@NewTransID", DbType.Int32, int.MaxValue);

            db.ExecuteNonQuery(cmd);
            newId = 0;
            newId = (int)db.GetParameterValue(cmd, "@NewTransID");
            cmd.Dispose();
            return DeleteMortgagee(newId);

        }

        public int SaveTransactionWithNewOwner(int transactionId, string newPolicyNo, out int newId,
            int typeofinsurance, string designation, string lastname, string firstname, string mi, string multicorpname)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_saveendorsement");
            db.AddInParameter(cmd, "@OldTransID", DbType.Int32, transactionId);
            db.AddInParameter(cmd, "@NewPolicyNo", DbType.String, newPolicyNo);
            db.AddOutParameter(cmd, "@NewTransID", DbType.Int32, int.MaxValue);

            db.ExecuteNonQuery(cmd);
            newId = 0;
            newId = (int)db.GetParameterValue(cmd, "@NewTransID");

            cmd.Dispose();

            cmd = db.GetStoredProcCommand("sp_savecustomerinfo");
            cmd.Parameters.Clear();

            db.AddInParameter(cmd,"@Designation", DbType.String, designation);
            db.AddInParameter(cmd,"@FirstName", DbType.String,firstname);
            db.AddInParameter(cmd,"@LastName", DbType.String,lastname);
            db.AddInParameter(cmd,"@MiddleName", DbType.String,mi);
            db.AddInParameter(cmd,"@MultipleCorporateName", DbType.String,multicorpname);
            db.AddInParameter(cmd,"@Address", DbType.String,string.Empty);
            db.AddInParameter(cmd,"@Telephone", DbType.String,string.Empty);
            db.AddInParameter(cmd,"@MobileNo", DbType.String,string.Empty);
            db.AddInParameter(cmd,"@Email", DbType.String,string.Empty);

            int newCustomerId = Convert.ToInt32(db.ExecuteScalar(cmd));
            cmd.Dispose();
            return UpdateTransferOwnership(newId, newCustomerId, typeofinsurance);

        }


        public int SaveTransactionWithUpdatePolicyDate(int transactionId,
         string newPolicyNo, out int newId, DateTime from, DateTime to)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_saveendorsement");
            db.AddInParameter(cmd, "@OldTransID", DbType.Int32, transactionId);
            db.AddInParameter(cmd, "@NewPolicyNo", DbType.String, newPolicyNo);
            db.AddOutParameter(cmd, "@NewTransID", DbType.Int32, int.MaxValue);

            db.ExecuteNonQuery(cmd);
            newId = 0;
            newId = (int)db.GetParameterValue(cmd, "@NewTransID");

            cmd.Dispose();
            return UpdatePolicyPeriod(newId, from, to);

        }

        public int SaveTransactionWithUpdatedVehicleDescription(int transactionId,
             string newPolicyNo, out int newId, int carcompany, string carmake,
             int carseries, string engineSeries)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_saveendorsement");
            db.AddInParameter(cmd, "@OldTransID", DbType.Int32, transactionId);
            db.AddInParameter(cmd, "@NewPolicyNo", DbType.String, newPolicyNo);
            db.AddOutParameter(cmd, "@NewTransID", DbType.Int32, int.MaxValue);

            db.ExecuteNonQuery(cmd);
            newId = 0;
            newId = (int)db.GetParameterValue(cmd, "@NewTransID");

            cmd.Dispose();

            return UpdateVehicleDescription(newId, carcompany, carmake, carseries, engineSeries);

        }

        public int SaveTransactionWithUpdatedCOCNo(int transactionId, string newPolicyNo, string newCOCNo, out int newId)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_saveendorsement");

            db.AddInParameter(cmd, "@OldTransID", DbType.Int32, transactionId);
            db.AddInParameter(cmd, "@NewPolicyNo", DbType.String, newPolicyNo);
            db.AddOutParameter(cmd, "@NewTransID", DbType.Int32, int.MaxValue);

            int result = db.ExecuteNonQuery(cmd);
            newId = 0;
            newId = (int)db.GetParameterValue(cmd, "@NewTransID");
            cmd.Dispose();
            return UpdateCOCNo(newCOCNo, newId);

        }

        private int UpdateCOCNo(string newCocNo, int transactionId)
        {
            string sql = string.Format(
                "UPDATE mTransactionCarDetail SET [COCNo] = '{0}' WHERE TransactionID={1}",
                newCocNo, transactionId);
            DbCommand cmd = db.GetSqlStringCommand(sql);
            int result = db.ExecuteNonQuery(cmd);
            cmd.Dispose();
            return result;
        }

        public int SaveTransactionWithUpdatedInsuredName(int transactionId,
                            string newPolicyNo, int customerId, out int newId,
                            string newLastName, string newFirstName, string newMI)
        {

            DbCommand cmd = db.GetStoredProcCommand("sp_saveendorsement");

            db.AddInParameter(cmd, "@OldTransID", DbType.Int32, transactionId);
            db.AddInParameter(cmd, "@NewPolicyNo", DbType.String, newPolicyNo);
            db.AddOutParameter(cmd, "@NewTransID", DbType.Int32, int.MaxValue);

            int result = db.ExecuteNonQuery(cmd);
            newId = 0;
            newId = (int)db.GetParameterValue(cmd, "@NewTransID");
            cmd.Dispose();
            return UpdateInsuredName(customerId, newFirstName, newLastName, newMI);

        }


        private int UpdateInsuredName(int recordId, string newFirstName, string newLastName,
                                    string newMi)
        {
            int result = 0;
            string sql = string.Format(
                "UPDATE mTransactionCustomerInfo SET [FirstName] = '{0}',[LastName]='{1}',[MiddleName]='{2}' WHERE RecordID={3}",
                newFirstName, newLastName, newMi, recordId);
            DbCommand cmd = db.GetSqlStringCommand(sql);
            result = db.ExecuteNonQuery(cmd);
            cmd.Dispose();
            return result;
        }

        public int UpdateAddress(int recordId, string newAddress)
        {
            string sql = string.Format(
                "UPDATE mTransactionCustomerInfo SET [Address] = '{0}' WHERE RecordID={1}",
                 newAddress, recordId);
            return DataHelper.SafeSQLExecuteNonQuery(db, sql);
        }

        public int UpdateMortgagee(int transactionId, string mortgagee)
        {
            string sql = string.Format(
                "UPDATE mTransactions SET [mortgage] = {0} WHERE TransactionID={1}",
                 mortgagee, transactionId);
            return DataHelper.SafeSQLExecuteNonQuery(db, sql);
        }

        public int DeleteMortgagee(int transactionId)
        {
            string sql = string.Format(
                "UPDATE mTransactions SET [mortgage] = 0 WHERE TransactionID={0}", transactionId);
            return DataHelper.SafeSQLExecuteNonQuery(db, sql);
        }

        public int UpdatePolicyPeriod(int transactionId, DateTime from, DateTime to)
        {
            string sql = string.Format(
                "UPDATE mTransactions SET [policyPeriodFrom] = @PolicyPeriodFrom,[policyPeriodTo] = @PolicyPeriodTo WHERE TransactionID={0}", transactionId);
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@PolicyPeriodFrom", DbType.DateTime, from);
            db.AddInParameter(cmd, "@PolicyPeriodTo", DbType.DateTime, to);
            int result = db.ExecuteNonQuery(cmd);
            cmd.Dispose();
            return result;
        }

        public int UpdateVehicleDescription(int transactionId, int carcompany,
            string carmake, int carseries, string engineseries)
        {
            string sql = string.Format(
                "UPDATE mTransactionCarDetail SET [CarCompany] = {0},[CarMake] = '{1}',[CarSeries]={2},[EngineSeries]='{3}' WHERE TransactionID={4}",
                        carcompany, carmake, carseries, engineseries, transactionId);
            return DataHelper.SafeSQLExecuteNonQuery(db, sql);
        }

        public int UpdateTransferOwnership(int transactionId, int customerId, int typeofinsurance)
        {
            string sql = string.Format(
                "UPDATE mTransactions SET [CustomerID] = @CustomerID,[typeOfInsured]=@typeOfInsured WHERE TransactionID={0}", transactionId);
            DbCommand cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "@CustomerID", DbType.Int32, customerId);
            db.AddInParameter(cmd, "@typeOfInsured", DbType.Int32, typeofinsurance);
            int result = db.ExecuteNonQuery(cmd);
            cmd.Dispose();
            return result;
        }

    }
}
