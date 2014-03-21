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
    public class TransactionData
    {
        Database db;
        public TransactionData()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public bool SaveTransaction(Transaction transaction, out int transactionId)
        {
            int result = 0;
            int perilResult = 0;
            int computationResult = 0;
            int carDetailResult = 0;
            int customerInfoResult = 0;
            transactionId = 0;
            DbTransaction dbTransaction = null;

            //Commands
            DbCommand cmd = null;
            DbCommand customerInfoCmd = null;
            DbCommand carDetailCmd = null;
            DbCommand perilsCmd = null;
            DbCommand computationCmd = null;
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (dbTransaction = connection.BeginTransaction())
                {

                    cmd = db.GetStoredProcCommand("sp_savetransaction");

                    //TODO: Hardcoding the user id since there is no implementation for user accounts
                    db.AddInParameter(cmd, "@userId", DbType.Int32, 1);
                    db.AddInParameter(cmd, "@creditingBranch", DbType.Int32, transaction.CreditingBranch);
                    db.AddInParameter(cmd, "@parNo", DbType.String, transaction.ParNo);
                    db.AddInParameter(cmd, "@policyNo", DbType.String, transaction.PolicyNo);
                    db.AddInParameter(cmd, "@geniisysNo", DbType.String, transaction.GeniisysNo);
                    db.AddInParameter(cmd, "@dateCreated", DbType.Date, transaction.DateCreated);

                    db.AddInParameter(cmd, "@policyPeriodFrom", DbType.Date, transaction.PolicyPeriodFrom);
                    db.AddInParameter(cmd, "@policyPeriodTo", DbType.Date, transaction.PolicyPeriodTo);
                    db.AddInParameter(cmd, "@businessType", DbType.String, transaction.BussinessType);
                    db.AddInParameter(cmd, "@policyStatus", DbType.String, transaction.PolicyStatus);
                    db.AddInParameter(cmd, "@sublineCode", DbType.String, transaction.SubLineCode);
                    db.AddInParameter(cmd, "@mortgage", DbType.Int32, transaction.MortgageCode);
                    db.AddInParameter(cmd, "@intermediaryCode", DbType.Int32, transaction.IntermediaryCode);
                    db.AddInParameter(cmd, "@typeOfInsured", DbType.Int32, transaction.TypeOfInsurance);
                    db.AddInParameter(cmd, "@isPosted", DbType.Boolean, transaction.IsPosted);
                    db.AddInParameter(cmd, "@isPrinted", DbType.Boolean, transaction.IsPrinted);
                    db.AddInParameter(cmd, "@isEndorse", DbType.Boolean, transaction.IsEndorsed);
                    db.AddInParameter(cmd, "@Remarks", DbType.String, transaction.Remarks);

                    customerInfoCmd = null;
                    if (transaction.CustomerID <= 0)
                    {
                        customerInfoCmd = db.GetStoredProcCommand("sp_savecustomerinfo");

                        db.AddInParameter(customerInfoCmd, "@Designation", DbType.String, transaction.Customer.Designation);
                        db.AddInParameter(customerInfoCmd, "@LastName", DbType.String, transaction.Customer.LastName);
                        db.AddInParameter(customerInfoCmd, "@FirstName", DbType.String, transaction.Customer.FirstName);
                        db.AddInParameter(customerInfoCmd, "@MiddleName", DbType.String, transaction.Customer.MiddleName);
                        db.AddInParameter(customerInfoCmd, "@Address", DbType.String, transaction.Customer.Address);
                        db.AddInParameter(customerInfoCmd, "@Telephone", DbType.String, transaction.Customer.Telephone);
                        db.AddInParameter(customerInfoCmd, "@MobileNo", DbType.String, transaction.Customer.MobileNo);
                        db.AddInParameter(customerInfoCmd, "@Email", DbType.String, transaction.Customer.Email);
                        db.AddInParameter(customerInfoCmd, "@MultipleCorporateName", DbType.String, transaction.Customer.MultipleCorporateName);
                    }

                    try
                    {
                        if (transaction.CustomerID <= 0)
                        {
                            using (customerInfoCmd)
                            {
                                customerInfoResult = Convert.ToInt32(db.ExecuteScalar(customerInfoCmd, dbTransaction));
                                transaction.CustomerID = customerInfoResult;
                            }

                            if (customerInfoResult <= 0)
                            {
                                throw new Exception("Customer Saving did not succeed. Will not continue.");
                            }
                        }

                        //Add the Referenced customer Id
                        db.AddInParameter(cmd, "@CustomerID", DbType.Int32, transaction.CustomerID);

                        result = Convert.ToInt32(db.ExecuteScalar(cmd, dbTransaction));
                        if (result > 0)
                        {
                            transactionId = result;
                            carDetailCmd = db.GetStoredProcCommand("sp_savecardetail");

                            db.AddInParameter(carDetailCmd, "@TransactionID", DbType.Int32, result);
                            db.AddInParameter(carDetailCmd, "@CarCompany", DbType.Int32, transaction.CarDetail.CarCompany);
                            db.AddInParameter(carDetailCmd, "@CarYear", DbType.Int32, transaction.CarDetail.CarYear);
                            db.AddInParameter(carDetailCmd, "@CarSeries", DbType.Int32, transaction.CarDetail.CarSeries);
                            db.AddInParameter(carDetailCmd, "@CarMake", DbType.String, transaction.CarDetail.CarMake);
                            db.AddInParameter(carDetailCmd, "@CarTypeOfBody", DbType.Int32, transaction.CarDetail.CarTypeOfBodyID);
                            db.AddInParameter(carDetailCmd, "@TypeOfCover", DbType.Int32, transaction.CarDetail.TypeOfCover);
                            db.AddInParameter(carDetailCmd, "@EngineSeries", DbType.String, transaction.CarDetail.EngineSeries);
                            db.AddInParameter(carDetailCmd, "@MotorType", DbType.String, transaction.CarDetail.MotorType);
                            db.AddInParameter(carDetailCmd, "@EngineNo", DbType.String, transaction.CarDetail.EngineNo);
                            db.AddInParameter(carDetailCmd, "@Color", DbType.String, transaction.CarDetail.Color);
                            db.AddInParameter(carDetailCmd, "@ConductionNo", DbType.String, transaction.CarDetail.ConductionNo);
                            db.AddInParameter(carDetailCmd, "@ChassisNo", DbType.String, transaction.CarDetail.ChassisNo);
                            db.AddInParameter(carDetailCmd, "@PlateNo", DbType.String, transaction.CarDetail.PlateNo);
                            db.AddInParameter(carDetailCmd, "@Accessories", DbType.String, transaction.CarDetail.Accessories);
                            db.AddInParameter(carDetailCmd, "@AuthenticationNo", DbType.String, transaction.CarDetail.AuthenticationNo);
                            db.AddInParameter(carDetailCmd, "@CocNo", DbType.String, transaction.CarDetail.COCNo);

                            carDetailResult = db.ExecuteNonQuery(carDetailCmd, dbTransaction);

                            string perilSql = SqlGenerator.CreateSQLforPerils(transaction.Perils, result);
                            perilsCmd = db.GetSqlStringCommand(perilSql);
                            perilResult = db.ExecuteNonQuery(perilsCmd, dbTransaction);

                            string computationsSql = SqlGenerator.CreateSQLforComputations(transaction.Computations.NetComputationDetails, transaction.Computations.GrossComputationDetails, result);

                            computationCmd = db.GetSqlStringCommand(computationsSql);
                            computationResult = db.ExecuteNonQuery(computationCmd, dbTransaction);

                            dbTransaction.Commit();
                        }
                        else
                        {

                        }
                    }
                    catch
                    {
                        dbTransaction.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                        dbTransaction.Dispose();
                        DataHelper.DisposeIfNotNull(cmd);
                        DataHelper.DisposeIfNotNull(customerInfoCmd);
                        DataHelper.DisposeIfNotNull(carDetailCmd);
                        DataHelper.DisposeIfNotNull(perilsCmd);
                        DataHelper.DisposeIfNotNull(computationCmd);
                    }
                }
            }
            return transaction.CustomerID > 0 && result > 0 && perilResult > 0 && computationResult > 0 && carDetailResult > 0;
        }
                
        public int PostTransaction(int transactionId)
        {
            int result = 0;
            DbCommand cmd = db.GetStoredProcCommand("sp_posttransaction");
            db.AddInParameter(cmd, "@TransactionID", DbType.Int32, transactionId);
            using (cmd)
            {
                result = db.ExecuteNonQuery(cmd);
            }
            return result;
        }

        public Transaction GetTransactionById(int transactionId)
        {
            DbCommand cmd = db.GetStoredProcCommand("sp_get_transactionbyid");
            db.AddInParameter(cmd, "@TransactionID", DbType.Int32, transactionId);
            Transaction t = new Transaction();
            using (cmd)
            {
                IDataReader reader = db.ExecuteReader(cmd);
                using (reader)
                {
                    int transactionIdIdx = reader.GetOrdinal("TransactionID");
                    int userIdIdx = reader.GetOrdinal("userId");
                    int creditingBranchIdx = reader.GetOrdinal("creditingBranch");
                    int parNoIdx = reader.GetOrdinal("parNo");
                    int policyNoIdx = reader.GetOrdinal("policyNo");
                    int geniisysNoIdx = reader.GetOrdinal("geniisysNo");
                    int dateCreatedIdx = reader.GetOrdinal("dateCreated");
                    int policyPeriodFromIdx = reader.GetOrdinal("policyPeriodFrom");
                    int policyPeriodToIdx = reader.GetOrdinal("policyPeriodTo");
                    int businessTypeIdx = reader.GetOrdinal("businessType");
                    int policyStatusIdx = reader.GetOrdinal("policyStatus");
                    int sublineCodeIdx = reader.GetOrdinal("sublineCode");
                    int mortgageIdx = reader.GetOrdinal("mortgage");
                    int intermediaryCodeIdx = reader.GetOrdinal("intermediaryCode");
                    int typeOfInsuredIdx = reader.GetOrdinal("typeOfInsured");
                    int isPostedIdx = reader.GetOrdinal("isPosted");
                    int isPrintedIdx = reader.GetOrdinal("isPrinted");
                    int isEndorseIdx = reader.GetOrdinal("isEndorse");
                    int remarksIdx = reader.GetOrdinal("Remarks");
                    int designationIdx = reader.GetOrdinal("Designation");
                    int firstNameIdx = reader.GetOrdinal("FirstName");
                    int lastNameIdx = reader.GetOrdinal("LastName");
                    int middleNameIdx = reader.GetOrdinal("MiddleName");
                    int addressIdx = reader.GetOrdinal("Address");
                    int telephoneIdx = reader.GetOrdinal("Telephone");
                    int mobileNoIdx = reader.GetOrdinal("MobileNo");
                    int emailIdx = reader.GetOrdinal("Email");
                    int multipleCorporateNameIdx = reader.GetOrdinal("MultipleCorporateName");
                    int customerIdIdx = reader.GetOrdinal("CustomerID");
                    int sublineNameIdx = reader.GetOrdinal("SublineName");
                    int creditingBranchNameIdx = reader.GetOrdinal("creditingBranchName");
                    int mortgageeNameIdx = reader.GetOrdinal("MortgName");
                    int intmNameIdx = reader.GetOrdinal("intmName");
                    int typeOfInsuranceNameIdx = reader.GetOrdinal("typeOfInsName");
                    int hasEndorsementIdx = reader.GetOrdinal("HasEndorsement");
                    while (reader.Read())
                    {
                        t.TransactionID = reader.IsDBNull(transactionIdIdx) ? 0 : reader.GetInt32(transactionIdIdx);
                        t.UserID = reader.IsDBNull(userIdIdx) ? 0 : reader.GetInt32(userIdIdx);
                        t.CreditingBranch = reader.IsDBNull(creditingBranchIdx) ? 0 : reader.GetInt32(creditingBranchIdx);
                        t.ParNo = reader.IsDBNull(parNoIdx) ? string.Empty : reader.GetString(parNoIdx);
                        t.PolicyNo = reader.IsDBNull(policyNoIdx) ? string.Empty : reader.GetString(policyNoIdx);
                        t.GeniisysNo = reader.IsDBNull(geniisysNoIdx) ? string.Empty : reader.GetString(geniisysNoIdx);
                        t.DateCreated = reader.IsDBNull(dateCreatedIdx) ? DateTime.Now : reader.GetDateTime(dateCreatedIdx);
                        t.PolicyPeriodFrom = reader.IsDBNull(policyPeriodFromIdx) ? DateTime.Now : reader.GetDateTime(policyPeriodFromIdx);
                        t.PolicyPeriodTo = reader.IsDBNull(policyPeriodToIdx) ? DateTime.Now : reader.GetDateTime(policyPeriodToIdx);
                        t.BussinessType = reader.IsDBNull(businessTypeIdx) ? string.Empty : reader.GetString(businessTypeIdx);
                        t.PolicyStatus = reader.IsDBNull(policyStatusIdx) ? string.Empty : reader.GetString(policyStatusIdx);
                        t.SubLineCode = reader.IsDBNull(sublineCodeIdx) ? string.Empty : reader.GetString(sublineCodeIdx);
                        t.MortgageCode = reader.IsDBNull(mortgageIdx) ? 0 : reader.GetInt32(mortgageIdx);
                        t.IntermediaryCode = reader.IsDBNull(intermediaryCodeIdx) ? 0 : reader.GetInt32(intermediaryCodeIdx);
                        t.TypeOfInsurance = reader.IsDBNull(typeOfInsuredIdx) ? 0 : reader.GetInt32(typeOfInsuredIdx);
                        t.IsPosted = reader.IsDBNull(isPostedIdx) ? false : Convert.ToBoolean(reader.GetInt32(isPostedIdx));
                        t.IsPrinted = reader.IsDBNull(isPrintedIdx) ? false : Convert.ToBoolean(reader.GetInt32(isPrintedIdx));
                        t.IsEndorsed = reader.IsDBNull(isEndorseIdx) ? false : Convert.ToBoolean(reader.GetInt32(isEndorseIdx));
                        t.Remarks = reader.IsDBNull(remarksIdx) ? string.Empty : reader.GetString(remarksIdx);
                        t.Customer.Designation = reader.IsDBNull(designationIdx) ? string.Empty : reader.GetString(designationIdx);
                        t.Customer.FirstName = reader.IsDBNull(firstNameIdx) ? string.Empty : reader.GetString(firstNameIdx);
                        t.Customer.LastName = reader.IsDBNull(lastNameIdx) ? string.Empty : reader.GetString(lastNameIdx);
                        t.Customer.MiddleName = reader.IsDBNull(middleNameIdx) ? string.Empty : reader.GetString(middleNameIdx);
                        t.Customer.Address = reader.IsDBNull(addressIdx) ? string.Empty : reader.GetString(addressIdx);
                        t.Customer.Telephone = reader.IsDBNull(telephoneIdx) ? string.Empty : reader.GetString(telephoneIdx);
                        t.Customer.MobileNo = reader.IsDBNull(mobileNoIdx) ? string.Empty : reader.GetString(mobileNoIdx);
                        t.Customer.Email = reader.IsDBNull(emailIdx) ? string.Empty : reader.GetString(emailIdx);
                        t.Customer.MultipleCorporateName = reader.IsDBNull(multipleCorporateNameIdx) ? string.Empty : reader.GetString(multipleCorporateNameIdx);
                        t.CustomerID = reader.IsDBNull(customerIdIdx) ? 0 : reader.GetInt32(customerIdIdx);
                        t.Customer.CustomerID = t.CustomerID;
                        t.SublineText = reader.IsDBNull(sublineNameIdx) ? string.Empty : reader.GetString(sublineNameIdx);
                        t.CreditingBranchName = reader.IsDBNull(creditingBranchNameIdx) ? string.Empty : reader.GetString(creditingBranchNameIdx);
                        t.MortgageeName = reader.IsDBNull(mortgageeNameIdx) ? string.Empty : reader.GetString(mortgageeNameIdx);
                        t.IntermediaryName = reader.IsDBNull(intmNameIdx) ? string.Empty : reader.GetString(intmNameIdx);
                        t.TypeOfInsuranceName = reader.IsDBNull(typeOfInsuranceNameIdx) ? string.Empty : reader.GetString(typeOfInsuranceNameIdx);
                        t.HasEndorsement = reader.IsDBNull(hasEndorsementIdx) ? false : Convert.ToBoolean(reader.GetInt32(hasEndorsementIdx));
                    }
                }
                t.CarDetail = DataFacade.Data.CarDetailsData.GetCarDetailByTransactionID(t.TransactionID);
                t.Computations = DataFacade.Data.TransactionComputationData.GetTransactionComputationsByTransactionId(t.TransactionID);
                t.Perils = DataFacade.Data.TransactionPerilsData.GetTransactionPerilsByTransactionId(t.TransactionID);
            }
            return t;
        }


        public bool UpdateTransaction(Transaction transaction)
        {
            int result = 0;
            int perilResult = 0;
            int computationResult = 0;
            int carDetailResult = 0;
            int deleteCarDetailResult = 0;
            int deleteComputationResult = 0;
            int deletePerilResult = 0;
            DbTransaction dbTransaction = null;

            DbCommand cmd = null;
            DbCommand deleteCarDetailCmd = null;
            DbCommand deletePerilCmd = null;
            DbCommand deleteComputationCmd = null;
            DbCommand carDetailCmd = null;
            DbCommand perilsCmd = null;
            DbCommand computationCmd = null;

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (dbTransaction = connection.BeginTransaction())
                {

                    cmd = db.GetStoredProcCommand("sp_updatetransaction");

                    //TODO: Hardcoding the user id since there is no implementation for user accounts
                    db.AddInParameter(cmd,"@userId",DbType.Int32, 1 );
                    db.AddInParameter(cmd,"@creditingBranch", DbType.Int32, transaction.CreditingBranch);
                    db.AddInParameter(cmd,"@parNo", DbType.String, transaction.ParNo );
                    db.AddInParameter(cmd,"@policyNo", DbType.String, transaction.PolicyNo );
                    db.AddInParameter(cmd,"@geniisysNo", DbType.String, transaction.GeniisysNo );
                    db.AddInParameter(cmd,"@dateCreated", DbType.Date, transaction.DateCreated  );
                    db.AddInParameter(cmd,"@policyPeriodFrom", DbType.Date, transaction.PolicyPeriodFrom  );
                    db.AddInParameter(cmd,"@policyPeriodTo", DbType.Date, transaction.PolicyPeriodTo );
                    db.AddInParameter(cmd,"@businessType", DbType.String, transaction.BussinessType );
                    db.AddInParameter(cmd,"@policyStatus", DbType.String, transaction.PolicyStatus );
                    db.AddInParameter(cmd,"@sublineCode", DbType.String, transaction.SubLineCode );
                    db.AddInParameter(cmd,"@mortgage", DbType.Int32, transaction.MortgageCode );
                    db.AddInParameter(cmd,"@intermediaryCode", DbType.Int32, transaction.IntermediaryCode );
                    db.AddInParameter(cmd,"@typeOfInsured", DbType.Int32, transaction.TypeOfInsurance );
                    db.AddInParameter(cmd,"@isPosted", DbType.Boolean, transaction.IsPosted );
                    db.AddInParameter(cmd,"@isPrinted", DbType.Boolean, transaction.IsPrinted );
                    db.AddInParameter(cmd,"@isEndorse", DbType.Boolean, transaction.IsEndorsed );
                    db.AddInParameter(cmd,"@Remarks", DbType.String, transaction.Remarks );

                    db.AddInParameter(cmd,"@Original_TransactionID", DbType.Int32, transaction.TransactionID );

                    try
                    {
                        result = db.ExecuteNonQuery(cmd, dbTransaction);
                        if (result > 0)
                        {
                            deleteCarDetailCmd = db.GetSqlStringCommand(string.Format("DELETE FROM mTransactionCarDetail WHERE TransactionID={0}", transaction.TransactionID));
                            deleteCarDetailResult = db.ExecuteNonQuery(deleteCarDetailCmd, dbTransaction);

                            deletePerilCmd = db.GetSqlStringCommand(string.Format("DELETE FROM mTransactionPerils WHERE TransactionID={0}", transaction.TransactionID));
                            deletePerilResult = db.ExecuteNonQuery(deletePerilCmd, dbTransaction);

                            deleteComputationCmd = db.GetSqlStringCommand(string.Format("DELETE FROM mTransactionComputations WHERE TransactionID={0}", transaction.TransactionID));
                            deleteComputationResult = db.ExecuteNonQuery(deleteComputationCmd, dbTransaction);

                            carDetailCmd = db.GetStoredProcCommand("sp_savecardetail");

                            db.AddInParameter(carDetailCmd, "@TransactionID", DbType.Int32, transaction.TransactionID);
                            db.AddInParameter(carDetailCmd, "@CarCompany", DbType.Int32, transaction.CarDetail.CarCompany);
                            db.AddInParameter(carDetailCmd, "@CarYear", DbType.Int32, transaction.CarDetail.CarYear);
                            db.AddInParameter(carDetailCmd, "@CarSeries", DbType.Int32, transaction.CarDetail.CarSeries);
                            db.AddInParameter(carDetailCmd, "@CarMake", DbType.String, transaction.CarDetail.CarMake);
                            db.AddInParameter(carDetailCmd, "@CarTypeOfBody", DbType.Int32, transaction.CarDetail.CarTypeOfBodyID);
                            db.AddInParameter(carDetailCmd, "@TypeOfCover", DbType.Int32, transaction.CarDetail.TypeOfCover);
                            db.AddInParameter(carDetailCmd, "@EngineSeries", DbType.String, transaction.CarDetail.EngineSeries);
                            db.AddInParameter(carDetailCmd, "@MotorType", DbType.String, transaction.CarDetail.MotorType);
                            db.AddInParameter(carDetailCmd, "@EngineNo", DbType.String, transaction.CarDetail.EngineNo);
                            db.AddInParameter(carDetailCmd, "@Color", DbType.String, transaction.CarDetail.Color);
                            db.AddInParameter(carDetailCmd, "@ConductionNo", DbType.String, transaction.CarDetail.ConductionNo);
                            db.AddInParameter(carDetailCmd, "@ChassisNo", DbType.String, transaction.CarDetail.ChassisNo);
                            db.AddInParameter(carDetailCmd, "@PlateNo", DbType.String, transaction.CarDetail.PlateNo);
                            db.AddInParameter(carDetailCmd, "@Accessories", DbType.String, transaction.CarDetail.Accessories);
                            db.AddInParameter(carDetailCmd, "@AuthenticationNo", DbType.String, transaction.CarDetail.AuthenticationNo);
                            db.AddInParameter(carDetailCmd, "@COCNo", DbType.String, transaction.CarDetail.COCNo);


                            carDetailResult = db.ExecuteNonQuery(carDetailCmd, dbTransaction);


                            string perilSql = SqlGenerator.CreateSQLforPerils(transaction.Perils, transaction.TransactionID);
                            perilsCmd = db.GetSqlStringCommand(perilSql);
                            perilResult = db.ExecuteNonQuery(perilsCmd, dbTransaction);

                            string computationsSql = SqlGenerator.CreateSQLforComputations(transaction.Computations.NetComputationDetails, transaction.Computations.GrossComputationDetails, transaction.TransactionID);

                            computationCmd = db.GetSqlStringCommand(computationsSql);
                            computationResult = db.ExecuteNonQuery(computationCmd, dbTransaction);

                            dbTransaction.Commit();
                        }
                        else
                        {
                            dbTransaction.Rollback();
                        }
                    }
                    catch
                    {
                        dbTransaction.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                        dbTransaction.Dispose();
                        
                        DataHelper.DisposeIfNotNull(cmd);
                        DataHelper.DisposeIfNotNull(deleteCarDetailCmd);
                        DataHelper.DisposeIfNotNull(deletePerilCmd);
                        DataHelper.DisposeIfNotNull(deleteComputationCmd);
                        DataHelper.DisposeIfNotNull(carDetailCmd);
                        DataHelper.DisposeIfNotNull(perilsCmd);
                        DataHelper.DisposeIfNotNull(computationCmd);
                    }
                }
            }
            return result > 0 && perilResult > 0 && computationResult > 0 && carDetailResult > 0 && deleteCarDetailResult > 0 && deleteComputationResult > 0 && deletePerilResult > 0;
        }

        public Dictionary<string, List<DropDownListItem>> LoadAllSearchFilters()
        {
            Dictionary<string, List<DropDownListItem>> filters = new Dictionary<string, List<DropDownListItem>>();
            DbCommand cmd = db.GetStoredProcCommand("sp_get_all_searchfilters");
            using (cmd)
            {
                IDataReader reader = db.ExecuteReader(cmd);
                using (reader)
                {
                    //crediting branches
                    List<DropDownListItem> creditingBranches = new List<DropDownListItem>();
                    int branchIdIdx = reader.GetOrdinal("creditingBrID");
                    int branchNameIdx = reader.GetOrdinal("creditingBranchName");
                    while (reader.Read())
                    {
                        DropDownListItem item = new DropDownListItem();
                        item.Value = reader.IsDBNull(branchIdIdx) ? "0" : reader.GetInt32(branchIdIdx).ToString();
                        item.Text = reader.IsDBNull(branchNameIdx) ? string.Empty : reader.GetString(branchNameIdx);
                        creditingBranches.Add(item);
                    }

                    filters.Add("creditingbranches", creditingBranches);

                    if (reader.NextResult())
                    {
                        //sublines
                        List<DropDownListItem> sublines = new List<DropDownListItem>();
                        int sublineCodeIdx = reader.GetOrdinal("sublineCode");
                        int sublineNameIdx = reader.GetOrdinal("sublineName");
                        while (reader.Read())
                        {
                            DropDownListItem item = new DropDownListItem();
                            item.Value = reader.IsDBNull(sublineCodeIdx) ? string.Empty : reader.GetString(sublineCodeIdx);
                            item.Text = reader.IsDBNull(sublineNameIdx) ? string.Empty : reader.GetString(sublineNameIdx);
                            sublines.Add(item);
                        }
                        filters.Add("sublines", sublines);
                    }

                    if (reader.NextResult())
                    {
                        //mortagagee
                        List<DropDownListItem> mortgagee = new List<DropDownListItem>();
                        int mortgIdIdx = reader.GetOrdinal("mortgId");
                        int mortgNameIdx = reader.GetOrdinal("MortgName");
                        while (reader.Read())
                        {
                            DropDownListItem item = new DropDownListItem();
                            item.Value = reader.IsDBNull(mortgIdIdx) ? "0" : reader.GetInt32(mortgIdIdx).ToString();
                            item.Text = reader.IsDBNull(mortgNameIdx) ? string.Empty : reader.GetString(mortgNameIdx);
                            mortgagee.Add(item);
                        }
                        filters.Add("mortgagee", mortgagee);
                    }

                    if (reader.NextResult())
                    {
                        //intermediary
                        List<DropDownListItem> intermediary = new List<DropDownListItem>();
                        int intmIdIdx = reader.GetOrdinal("intmId");
                        int intmNameIdx = reader.GetOrdinal("intmName");
                        while (reader.Read())
                        {
                            DropDownListItem item = new DropDownListItem();
                            item.Value = reader.IsDBNull(intmIdIdx) ? "0" : reader.GetInt32(intmIdIdx).ToString();
                            item.Text = reader.IsDBNull(intmNameIdx) ? string.Empty : reader.GetString(intmNameIdx);
                            intermediary.Add(item);
                        }
                        filters.Add("intermediary", intermediary);
                    }

                    if (reader.NextResult())
                    {
                        //typeofcovers
                        List<DropDownListItem> typeofcovers = new List<DropDownListItem>();
                        int typeOfCoverIdIdx = reader.GetOrdinal("typeOfCoverId");
                        int coverNameIdx = reader.GetOrdinal("coverName");
                        while (reader.Read())
                        {
                            DropDownListItem item = new DropDownListItem();
                            item.Value = reader.IsDBNull(typeOfCoverIdIdx) ? "0" : reader.GetInt32(typeOfCoverIdIdx).ToString();
                            item.Text = reader.IsDBNull(coverNameIdx) ? string.Empty : reader.GetString(coverNameIdx);
                            typeofcovers.Add(item);
                        }
                        filters.Add("typeofcovers", typeofcovers);
                    }

                    if (reader.NextResult())
                    {
                        //carcompanies
                        List<DropDownListItem> carcompanies = new List<DropDownListItem>();
                        int carCompanyCodeIdx = reader.GetOrdinal("carCompanyCode");
                        int carCompanyIdx = reader.GetOrdinal("carCompany");
                        while (reader.Read())
                        {
                            DropDownListItem item = new DropDownListItem();
                            item.Value = reader.IsDBNull(carCompanyCodeIdx) ? "0" : reader.GetInt32(carCompanyCodeIdx).ToString();
                            item.Text = reader.IsDBNull(carCompanyIdx) ? string.Empty : reader.GetString(carCompanyIdx);
                            carcompanies.Add(item);
                        }
                        filters.Add("carcompanies", carcompanies);
                    }
                }
            }
            return filters;
        }

        public IEnumerable<TransactionSearchDTO> SearchTransaction(string whereClause)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(
                "SELECT t.*,tc.*, mtc.*, cb.*, sl.sublineName, sl.sublineCode as SublineCodeParent,cc.carCompany as CarCompanyName, " +
                "ct.coverName as CoverTypeName FROM mTransactions t " +
                " INNER JOIN mTransactionCarDetail tc " +
                " ON t.TransactionID = tc.TransactionID " +
                " INNER JOIN mTransactionCustomerInfo mtc " +
                " ON t.CustomerID = mtc.RecordID " +
                " INNER JOIN mCreditingBranches cb " +
                " ON t.creditingBranch = cb.creditingBrID " +
                " INNER JOIN mSublines sl " +
                " ON t.sublineCode = sl.sublineCode " +
                " INNER JOIN carCompanies cc " +
                " ON tc.CarCompany = cc.carCompanyCode " +
                " INNER JOIN coverTypes ct " +
                " ON tc.TypeOfCover = ct.typeOfCoverId ");

            if (!string.IsNullOrEmpty(whereClause))
            {
                sql.AppendFormat(" WHERE {0}", whereClause);
            }

            sql.Append(" ORDER BY t.dateCreated DESC ");

            DbCommand cmd = db.GetSqlStringCommand(sql.ToString());
            List<TransactionSearchDTO> ts = new List<TransactionSearchDTO>();
            using (cmd)
            {
                IDataReader reader = db.ExecuteReader(cmd);
                using (reader)
                {
                    int transactionIdIdx = reader.GetOrdinal("TransactionID");
                    int lastNameIdx = reader.GetOrdinal("LastName");
                    int firstNameIdx = reader.GetOrdinal("FirstName");
                    int middleNameIdx = reader.GetOrdinal("MiddleName");
                    int parNoIdx = reader.GetOrdinal("parNo");
                    int policyNoIdx = reader.GetOrdinal("policyNo");
                    //not used yet
                    int creditingBranchIdx = reader.GetOrdinal("creditingBranch");
                    int creditingBranchNameIdx = reader.GetOrdinal("creditingBranchName");
                    int sublineIdx = reader.GetOrdinal("sublineCode");
                    int sublineNameIdx = reader.GetOrdinal("sublineName");
                    int dateCreatedIdx = reader.GetOrdinal("dateCreated");
                    int policyPeriodFromIdx = reader.GetOrdinal("policyPeriodFrom");
                    int policyPeriodToIdx = reader.GetOrdinal("policyPeriodTo");
                    int typeOfCoverIdx = reader.GetOrdinal("CoverTypeName");
                    int carCompanyIdx = reader.GetOrdinal("CarCompanyName");
                    int motorTypeIdx = reader.GetOrdinal("MotorType");
                    int chassisNoIdx = reader.GetOrdinal("ChassisNo");
                    int engineNoIdx = reader.GetOrdinal("EngineNo");
                    int plateNoIdx = reader.GetOrdinal("PlateNo");

                    while (reader.Read())
                    {
                        TransactionSearchDTO t = new TransactionSearchDTO();
                        t.TransactionID = reader.IsDBNull(transactionIdIdx) ? 0 : reader.GetInt32(transactionIdIdx);
                        t.LastName = reader.IsDBNull(lastNameIdx) ? string.Empty : reader.GetString(lastNameIdx);
                        t.FirstName = reader.IsDBNull(firstNameIdx) ? string.Empty : reader.GetString(firstNameIdx);
                        t.MiddleName = reader.IsDBNull(middleNameIdx) ? string.Empty : reader.GetString(middleNameIdx);
                        t.ParNo = reader.IsDBNull(parNoIdx) ? string.Empty : reader.GetString(parNoIdx);
                        t.PolicyNo = reader.IsDBNull(policyNoIdx) ? string.Empty : reader.GetString(policyNoIdx);
                        t.CreditingBranch = reader.IsDBNull(creditingBranchNameIdx) ? string.Empty : reader.GetString(creditingBranchNameIdx);
                        t.SublineText = reader.IsDBNull(sublineNameIdx) ? string.Empty : reader.GetString(sublineNameIdx);
                        t.Subline = reader.IsDBNull(sublineIdx) ? string.Empty : reader.GetString(sublineIdx);
                        t.DateCreated = reader.IsDBNull(dateCreatedIdx) ? DateTime.MinValue : reader.GetDateTime(dateCreatedIdx);
                        t.PolicyPeriodFrom = reader.IsDBNull(policyPeriodFromIdx) ? DateTime.MinValue : reader.GetDateTime(policyPeriodFromIdx);
                        t.PolicyPeriodTo = reader.IsDBNull(policyPeriodToIdx) ? DateTime.MinValue : reader.GetDateTime(policyPeriodToIdx);
                        t.TypeOfCover = reader.IsDBNull(typeOfCoverIdx) ? string.Empty : reader.GetString(typeOfCoverIdx);
                        t.CarCompany = reader.IsDBNull(carCompanyIdx) ? string.Empty : reader.GetString(carCompanyIdx);
                        t.MotorType = reader.IsDBNull(motorTypeIdx) ? string.Empty : reader.GetString(motorTypeIdx);
                        t.ChassisNo = reader.IsDBNull(chassisNoIdx) ? string.Empty : reader.GetString(chassisNoIdx);
                        t.EngineNo = reader.IsDBNull(engineNoIdx) ? string.Empty : reader.GetString(engineNoIdx);
                        t.PlateNo = reader.IsDBNull(plateNoIdx) ? string.Empty : reader.GetString(plateNoIdx);
                        ts.Add(t);
                    }
                }
            }
            return ts;
        }
    }
}
