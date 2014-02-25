using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using MotorOnline.Library.Entity;
using Effortless.Net.Encryption;

namespace MotorOnline.Web
{
    public class cls_data_access_layer
    {
        cls_data_access_helper go_dah;
        SqlConnection go_sqlConnection;

        public cls_data_access_layer()
        {
            go_dah = new cls_data_access_helper();
            uf_connect();
        }

        public void uf_connect()
        {
            go_sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["dataConnection"].ConnectionString);
        }

        #region " Populators "

        public DataTable uf_pop_mCreditingBranches()
        {

            go_dah.uf_set_stored_procedure("sp_pop_mCreditingBranches", ref go_sqlConnection);
            return go_dah.uf_execute_data_table();
        }

        public DataTable GetAllTransactions() {
            go_dah.uf_set_sql_statement("SELECT mt.*,mtc.* FROM mTransactions mt INNER JOIN mTransactionCustomerInfo mtc ON mt.CustomerID = mtc.RecordID", ref go_sqlConnection);
            return go_dah.uf_execute_data_table();
        }

        public string GetLastParNo() {
            go_dah.uf_set_stored_procedure("sp_getlastparno", ref go_sqlConnection);
            object result = go_dah.uf_execute_scalar();
            return result.ToString();
        }

        public string GetLastPolicyNo()
        {
            go_dah.uf_set_stored_procedure("sp_getlastpolicyno", ref go_sqlConnection);
            object result = go_dah.uf_execute_scalar();
            return result.ToString();
        }

        public DataTable PopulateSublines()
        {
            go_dah.uf_set_stored_procedure("sp_pop_mSublines", ref go_sqlConnection);
            return go_dah.uf_execute_data_table();
        }

        public DataTable PopulateCarCompanies() {
            go_dah.uf_set_stored_procedure("sp_pop_mCarCompanies", ref go_sqlConnection);
            return go_dah.uf_execute_data_table();
        }

        public DataTable FilterCarMakeByCarCompany(int companyId) {
            go_dah.uf_set_stored_procedure("sp_filterCarMakesAndSeriesByCarCompany", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@CarCompanyID", companyId);
            return go_dah.uf_execute_data_table();
        }

        public DataTable FilterCarSeriesByCarMake(int makeId, int companyId)
        {
            go_dah.uf_set_stored_procedure("sp_filterCarSeriesByCarMake", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@MakeID", makeId);
            go_dah.uf_set_stored_procedure_param("@CompanyID", companyId);
            return go_dah.uf_execute_data_table();
        }

        public DataTable FilterCarEngineByCarSeries(int makeId, int companyId, int seriesId)
        {
            go_dah.uf_set_stored_procedure("sp_filterCarEngineByCarSeriesMakeAndCompany", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@MakeID", makeId);
            go_dah.uf_set_stored_procedure_param("@CompanyID", companyId);
            go_dah.uf_set_stored_procedure_param("@SeriesID", seriesId);
            return go_dah.uf_execute_data_table();
        }

        public DataTable GetUploadedPolicyStatusByEngineNo(string engineNo) 
        {
            go_dah.uf_set_stored_procedure("sp_getUploadedPolicyStatusByEngineNo", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@EngineNo", engineNo);
            return go_dah.uf_execute_data_table();
        }

        //public DataTable uf_pop_mBusinessTypes()
        //{
        //    go_dah.uf_set_stored_procedure("sp_pop_mBusinessType", ref go_sqlConnection);
        //    return go_dah.uf_execute_data_table();
        //}

        public DataTable PopulateCoverTypes()
        {
            go_dah.uf_set_stored_procedure("sp_pop_coverTypes", ref go_sqlConnection);
            return go_dah.uf_execute_data_table();
        }

        public DataTable PopulateMortgagee()
        {
            go_dah.uf_set_stored_procedure("sp_pop_mMortgagee", ref go_sqlConnection);
            return go_dah.uf_execute_data_table();
        }

        public DataTable sp_pop_mIntermediaries()
        {
            go_dah.uf_set_stored_procedure("sp_pop_mIntermediaries", ref go_sqlConnection);
            return go_dah.uf_execute_data_table();
        }

        public DataTable sp_pop_typeOfInsurance()
        {
            go_dah.uf_set_stored_procedure("sp_pop_typeOfInsurance",ref go_sqlConnection);
            return go_dah.uf_execute_data_table();
        }

        public DataTable sp_pop_carYears()
        {
            go_dah.uf_set_stored_procedure("sp_pop_carYears", ref go_sqlConnection);
            return go_dah.uf_execute_data_table();
        }

        public IDataReader sp_pop_carYears_reader()
        {
            go_dah.uf_set_stored_procedure("sp_pop_carYears", ref go_sqlConnection);
            return go_dah.uf_execute_reader();
        }

        public DataTable sp_pop_carTypesOfBody()
        {
            go_dah.uf_set_stored_procedure("sp_pop_carTypesOfBody", ref go_sqlConnection);
            return go_dah.uf_execute_data_table();
        }
        
        public IDataReader sp_getPerilsByTypeOfCover(int type)
        {
            string sp = string.Empty;
            switch (type) 
            {
                case 1:
                    sp = "sp_getPerilsForCTPLOnly";
                    break;
                case 2:
                    sp = "sp_getPerilsForWithCTPL";
                    break;
                case 3:
                    sp = "sp_getPerilsForWithoutCTPL";
                    break;
                default:
                    break;
            }
            go_dah.uf_set_stored_procedure(sp, ref go_sqlConnection);
            return go_dah.uf_execute_reader();
        }

        public IDataReader sp_getRatesByPerilID(int perilId)
        {
            go_dah.uf_set_stored_procedure("sp_getRatesByPerilID", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@PerilID", perilId);
            return go_dah.uf_execute_reader();
        }

        public IDataReader GetComputationFactors()
        {
            go_dah.uf_set_stored_procedure("sp_getComputationFactors", ref go_sqlConnection);
            return go_dah.uf_execute_reader();
        }

        public double GetCtplDefault(string subline) {
            go_dah.uf_set_stored_procedure("sp_getctpldefaultbysubline", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@SublineCode", subline.Trim().ToUpper());
            object result = go_dah.uf_execute_scalar();
            return ChangeTypeHelper.SafeParseToDouble(result.ToString());
        }

        #endregion

        public bool SaveKeys() {
            //string sql = string.Format("INSERT INTO [mKeys](PasswordIV, PasswordKey) VALUES({0},{1})",
            //    Bytes.GenerateIV(), Bytes.GenerateKey());
            //go_dah.uf_set_sql_statement(sql, ref go_sqlConnection);
            //return go_dah.uf_execute_non_query() > 0;

            SqlCommand cmd = go_sqlConnection.CreateCommand();
            cmd.Connection = go_sqlConnection;
            cmd.Connection.Open();
            cmd.CommandText = "INSERT INTO [mKeys](PasswordIV, PasswordKey) VALUES(@PasswordIV, @PasswordKey)";
            cmd.Parameters.Add(new SqlParameter() { DbType = DbType.Binary, ParameterName = "@PasswordIV", Value = Bytes.GenerateIV() });
            cmd.Parameters.Add(new SqlParameter() { DbType = DbType.Binary, ParameterName = "@PasswordKey", Value = Bytes.GenerateKey() });

            return cmd.ExecuteNonQuery() > 0;
        }

        public EncryptionKeys GetEncryptionKeys() {
            string sql = "SELECT * FROM [mKeys]";
            go_dah.uf_set_sql_statement(sql, ref go_sqlConnection);
            IDataReader reader = go_dah.uf_execute_reader();
            EncryptionKeys keys = null;
            using (reader)
            {
                int ivIdx = reader.GetOrdinal("PasswordIV");
                int keyIdx = reader.GetOrdinal("PasswordKey");
                while (reader.Read())
                {
                    keys = new EncryptionKeys();
                    keys.IV = (byte[])reader.GetValue(ivIdx);
                    keys.Key = (byte[])reader.GetValue(keyIdx);
                }
            }
            return keys;
        }

        public bool SaveUser(User user) {
            go_dah.uf_set_stored_procedure("sp_saveuser", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@Username", user.Username);
            go_dah.uf_set_stored_procedure_param("@Password", user.Password);
            go_dah.uf_set_stored_procedure_param("@LastName", user.LastName);
            go_dah.uf_set_stored_procedure_param("@FirstName", user.FirstName);
            go_dah.uf_set_stored_procedure_param("@MI", user.MI);
            go_dah.uf_set_stored_procedure_param("@RoleID", user.RoleID);
            go_dah.uf_set_stored_procedure_param("@LastActivityDate", user.LastActivityDate);

            return go_dah.uf_execute_non_query() > 0;
        }

        public User AuthenticateUser(string username, string password) {
            go_dah.uf_set_stored_procedure("sp_authenticateuser", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@Username", username);
            go_dah.uf_set_stored_procedure_param("@Password", password);

            IDataReader reader = go_dah.uf_execute_reader();
            User user = null;
            using (reader) {
                int userIdIdx = reader.GetOrdinal("UserID");
                int usernameIdx = reader.GetOrdinal("Username");
                //int userIdIdx = reader.GetOrdinal("Password");
                int firsnameIdx = reader.GetOrdinal("FirstName");
                int miIdx = reader.GetOrdinal("MI");
                int lastnameIdx = reader.GetOrdinal("LastName");
                int lastactivityIdx = reader.GetOrdinal("LastActivityDate");

                int roleNameIdx = reader.GetOrdinal("RoleName");
                int canAddTransactionIdx = reader.GetOrdinal("CanAddTransaction");
                int canEditTransactionIdx = reader.GetOrdinal("CanEditTransaction");
                int canViewTransactionIdx = reader.GetOrdinal("CanViewTransaction");
                int canDeleteTransactionIdx = reader.GetOrdinal("CanDeleteTransaction");
                int canPostTransactionIdx = reader.GetOrdinal("CanPostTransaction");
                int canAddUserIdx = reader.GetOrdinal("CanAddUser");
                int canEditUserIdx = reader.GetOrdinal("CanEditUser");
                int canDeleteUserIdx = reader.GetOrdinal("CanDeleteUser");
                int canEditPerilsIdx = reader.GetOrdinal("CanEditPerils");
                int canEndorseIdx = reader.GetOrdinal("CanEndorse");

                while (reader.Read())
                {
                    user = new User();
                    user.UserID = reader.GetInt32(userIdIdx);
                    user.Username = reader.GetString(usernameIdx);
                    user.FirstName = reader.IsDBNull(firsnameIdx) ? string.Empty : reader.GetString(firsnameIdx);
                    user.LastName = reader.IsDBNull(lastactivityIdx) ? string.Empty : reader.GetString(lastnameIdx);
                    user.MI = reader.IsDBNull(miIdx) ? string.Empty : reader.GetString(miIdx);

                    user.UserRole = new UserRole()
                    {
                        RoleName = reader.GetString(roleNameIdx),
                        CanAddTransaction = reader.GetBoolean(canAddTransactionIdx),
                        CanEditTransaction = reader.GetBoolean(canEditTransactionIdx),
                        CanViewTransaction = reader.GetBoolean(canViewTransactionIdx),
                        CanDeleteTransaction = reader.GetBoolean(canDeleteTransactionIdx),
                        CanPostTransaction = reader.GetBoolean(canPostTransactionIdx),
 
                        CanAddUser = reader.GetBoolean(canAddUserIdx),
                        CanEditUser = reader.GetBoolean(canEditUserIdx),
                        CanDeleteUser = reader.GetBoolean(canDeleteUserIdx),
                        CanEditPerils = reader.GetBoolean(canEditPerilsIdx),
                        CanEndorse = reader.GetBoolean(canEndorseIdx)
                    };
                }
            }
            return user;
        }

        public bool SaveTransaction(Transaction transaction) {
            int result = 0;
            int perilResult = 0;
            int computationResult = 0;
            int carDetailResult = 0;
            int customerInfoResult = 0;

            using (go_sqlConnection) {
                if (go_sqlConnection.State == ConnectionState.Closed)
                {
                    go_sqlConnection.Open();
                }

                SqlCommand cmd = go_sqlConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_savetransaction";

                SqlTransaction dataTransaction;
                dataTransaction = go_sqlConnection.BeginTransaction();

                cmd.Connection = go_sqlConnection;
                cmd.Transaction = dataTransaction;


                //TODO: Hardcoding the user id since there is no implementation for user accounts
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@userId", Value = 1, DbType = DbType.Int32 });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@creditingBranch", Value = transaction.CreditingBranch, DbType = DbType.Int32 });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@parNo", Value = transaction.ParNo, DbType = DbType.String });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@policyNo", Value = transaction.PolicyNo, DbType = DbType.String });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@geniisysNo", Value = transaction.GeniisysNo, DbType = DbType.String });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@dateCreated", Value = transaction.DateCreated, DbType = DbType.Date });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@policyPeriodFrom", Value = transaction.PolicyPeriodFrom, DbType = DbType.Date });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@policyPeriodTo", Value = transaction.PolicyPeriodTo, DbType = DbType.Date });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@businessType", Value = transaction.BussinessType, DbType = DbType.String });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@policyStatus", Value = transaction.PolicyStatus, DbType = DbType.String });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@sublineCode", Value = transaction.SubLineCode, DbType = DbType.String });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@mortgage", Value = transaction.MortgageCode, DbType = DbType.Int32 });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@intermediaryCode", Value = transaction.IntermediaryCode, DbType = DbType.Int32 });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@typeOfInsured", Value = transaction.TypeOfInsurance, DbType = DbType.Int32 });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@isPosted", Value = transaction.IsPosted, DbType = DbType.Boolean });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@isPrinted", Value = transaction.IsPrinted, DbType = DbType.Boolean });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@isEndorse", Value = transaction.IsEndorsed, DbType = DbType.Boolean });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Remarks", Value = transaction.Remarks, DbType = DbType.String });

                SqlCommand customerInfoCmd = null;
                if (transaction.CustomerID <= 0)
                {
                    customerInfoCmd = go_sqlConnection.CreateCommand();
                    customerInfoCmd.Connection = go_sqlConnection;
                    customerInfoCmd.Transaction = dataTransaction;
                    customerInfoCmd.CommandType = CommandType.StoredProcedure;
                    customerInfoCmd.CommandText = "sp_savecustomerinfo";
                    customerInfoCmd.CommandTimeout = 30;

                    customerInfoCmd.Parameters.Add(new SqlParameter() { ParameterName = "@Designation", Value = transaction.Customer.Designation, DbType = DbType.String });
                    customerInfoCmd.Parameters.Add(new SqlParameter() { ParameterName = "@LastName", Value = transaction.Customer.LastName, DbType = DbType.String });
                    customerInfoCmd.Parameters.Add(new SqlParameter() { ParameterName = "@FirstName", Value = transaction.Customer.FirstName, DbType = DbType.String });
                    customerInfoCmd.Parameters.Add(new SqlParameter() { ParameterName = "@MiddleName", Value = transaction.Customer.MiddleName, DbType = DbType.String });
                    customerInfoCmd.Parameters.Add(new SqlParameter() { ParameterName = "@Address", Value = transaction.Customer.Address, DbType = DbType.String });
                    customerInfoCmd.Parameters.Add(new SqlParameter() { ParameterName = "@Telephone", Value = transaction.Customer.Telephone, DbType = DbType.String });
                    customerInfoCmd.Parameters.Add(new SqlParameter() { ParameterName = "@MobileNo", Value = transaction.Customer.MobileNo, DbType = DbType.String });
                    customerInfoCmd.Parameters.Add(new SqlParameter() { ParameterName = "@Email", Value = transaction.Customer.Email, DbType = DbType.String });
                    customerInfoCmd.Parameters.Add(new SqlParameter() { ParameterName = "@MultipleCorporateName", Value = transaction.Customer.MultipleCorporateName, DbType = DbType.String });
                }

                try
                {
                    if (transaction.CustomerID <= 0)
                    {
                        using (customerInfoCmd)
                        {
                            customerInfoResult = Convert.ToInt32(customerInfoCmd.ExecuteScalar());
                            transaction.CustomerID = customerInfoResult;
                        }

                        if (customerInfoResult <= 0)
                        {
                            throw new Exception("Customer Saving did not succeed. Will not continue.");
                        }
                    }

                    //Add the Referenced customer Id
                    cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CustomerID", Value = transaction.CustomerID, DbType = DbType.Int32 });

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    if (result > 0)
                    {
                        SqlCommand carDetailCmd = go_sqlConnection.CreateCommand();
                        carDetailCmd.Connection = go_sqlConnection;
                        carDetailCmd.Transaction = dataTransaction;
                        carDetailCmd.CommandType = CommandType.StoredProcedure;
                        carDetailCmd.CommandText = "sp_savecardetail";
                        carDetailCmd.CommandTimeout = 30;

                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@TransactionID", Value = result, DbType = DbType.Int32 });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@CarCompany", Value = transaction.CarDetail.CarCompany, DbType = DbType.Int32 });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@CarYear", Value = transaction.CarDetail.CarYear, DbType = DbType.Int32 });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@CarSeries", Value = transaction.CarDetail.CarSeries, DbType = DbType.Int32 });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@CarMake", Value = transaction.CarDetail.CarMake, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@CarTypeOfBody", Value = transaction.CarDetail.CarTypeOfBodyID, DbType = DbType.Int32 });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@TypeOfCover", Value = transaction.CarDetail.TypeOfCover, DbType = DbType.Int32 });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@EngineSeries", Value = transaction.CarDetail.EngineSeries, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@MotorType", Value = transaction.CarDetail.MotorType, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@EngineNo", Value = transaction.CarDetail.EngineNo, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@Color", Value = transaction.CarDetail.Color, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@ConductionNo", Value = transaction.CarDetail.ConductionNo, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@ChassisNo", Value = transaction.CarDetail.ChassisNo, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@PlateNo", Value = transaction.CarDetail.PlateNo, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@Accessories", Value = transaction.CarDetail.Accessories, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@AuthenticationNo", Value = transaction.CarDetail.AuthenticationNo, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@CocNo", SourceColumn="COCNo", Direction = ParameterDirection.Input, Value = transaction.CarDetail.COCNo, DbType = DbType.String });

                        carDetailResult = carDetailCmd.ExecuteNonQuery();

                        string perilSql = cls_data_access_sqlgenerator.CreateSQLforPerils(transaction.Perils, result);
                        SqlCommand perilsCmd = go_sqlConnection.CreateCommand();
                        
                        perilsCmd.Connection = go_sqlConnection;
                        perilsCmd.Transaction = dataTransaction;

                        perilsCmd.CommandType = CommandType.Text;
                        perilsCmd.CommandText = perilSql;
                        perilsCmd.CommandTimeout = 30;

                        perilResult = perilsCmd.ExecuteNonQuery();

                        string computationsSql = cls_data_access_sqlgenerator.CreateSQLforComputations(transaction.Computations.NetComputationDetails, transaction.Computations.GrossComputationDetails, result);

                        SqlCommand computationCmd = go_sqlConnection.CreateCommand();

                        computationCmd.Connection = go_sqlConnection;
                        computationCmd.Transaction = dataTransaction;

                        computationCmd.CommandType = CommandType.Text;
                        computationCmd.CommandText = computationsSql;
                        computationCmd.CommandTimeout = 30;

                        computationResult = computationCmd.ExecuteNonQuery();

                        dataTransaction.Commit();
                    }
                    else
                    { 
                        
                    }
                }
                catch
                {
                    dataTransaction.Rollback();
                }
            }

            return transaction.CustomerID > 0 && result > 0 && perilResult > 0 && computationResult > 0 && carDetailResult > 0;
        }


        public int SaveTransactionWithUpdatedCOCNo(int transactionId, string newPolicyNo, string newCOCNo, out int newId)
        {
            //go_dah.uf_set_stored_procedure("sp_saveendorsement", ref go_sqlConnection);
            //go_dah.uf_set_stored_procedure_param(

            if (go_sqlConnection.State == ConnectionState.Closed)
            {
                go_sqlConnection.Open();
            }

            SqlCommand cmd = go_sqlConnection.CreateCommand();
            cmd.CommandText = "sp_saveendorsement";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = go_sqlConnection;

            cmd.Parameters.AddWithValue("@OldTransID", transactionId);
            cmd.Parameters.AddWithValue("@NewPolicyNo", newPolicyNo);
            SqlParameter newTransID = new SqlParameter() { ParameterName="@NewTransID", 
                DbType=System.Data.DbType.Int32, Value=0, Size = int.MaxValue , 
                Direction=ParameterDirection.Output };
            cmd.Parameters.Add(newTransID);

            int result = cmd.ExecuteNonQuery();
            //NOTE: Must be assigned before leaving the method
            newId = 0;
            //if(result > 0){
                newId = (int)newTransID.Value;
            //}

            return UpdateCOCNo(newCOCNo, newId);

        }

        public int SaveTransactionWithUpdatedInsuredName(int transactionId, 
            string newPolicyNo, int customerId, out int newId,
            string newLastName, string newFirstName, string newMI)
        {
            //go_dah.uf_set_stored_procedure("sp_saveendorsement", ref go_sqlConnection);
            //go_dah.uf_set_stored_procedure_param(

            if (go_sqlConnection.State == ConnectionState.Closed)
            {
                go_sqlConnection.Open();
            }

            SqlCommand cmd = go_sqlConnection.CreateCommand();
            cmd.CommandText = "sp_saveendorsement";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = go_sqlConnection;

            cmd.Parameters.AddWithValue("@OldTransID", transactionId);
            cmd.Parameters.AddWithValue("@NewPolicyNo", newPolicyNo);
            SqlParameter newTransID = new SqlParameter()
            {
                ParameterName = "@NewTransID",
                DbType = System.Data.DbType.Int32,
                Value = 0,
                Size = int.MaxValue,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(newTransID);

            int result = cmd.ExecuteNonQuery();
            //NOTE: Must be assigned before leaving the method
            newId = 0;
            //if(result > 0){
            newId = (int)newTransID.Value;
            //}

            return UpdateInsuredName(customerId, newFirstName, newLastName, newMI);

        }


        public int SaveTransactionWithUpdatedAddress(int transactionId,
            string newPolicyNo, int customerId, out int newId,
            string newAddress)
        {
            //go_dah.uf_set_stored_procedure("sp_saveendorsement", ref go_sqlConnection);
            //go_dah.uf_set_stored_procedure_param(

            if (go_sqlConnection.State == ConnectionState.Closed)
            {
                go_sqlConnection.Open();
            }

            SqlCommand cmd = go_sqlConnection.CreateCommand();
            cmd.CommandText = "sp_saveendorsement";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = go_sqlConnection;

            cmd.Parameters.AddWithValue("@OldTransID", transactionId);
            cmd.Parameters.AddWithValue("@NewPolicyNo", newPolicyNo);
            SqlParameter newTransID = new SqlParameter()
            {
                ParameterName = "@NewTransID",
                DbType = System.Data.DbType.Int32,
                Value = 0,
                Size = int.MaxValue,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(newTransID);

            int result = cmd.ExecuteNonQuery();
            //NOTE: Must be assigned before leaving the method
            newId = 0;
            //if(result > 0){
            newId = (int)newTransID.Value;
            //}

            return UpdateAddress(customerId, newAddress);

        }

        public int SaveTransactionWithUpdatedMortgagee(int transactionId,
            string newPolicyNo, out int newId,
            string mortgagee)
        {
            //go_dah.uf_set_stored_procedure("sp_saveendorsement", ref go_sqlConnection);
            //go_dah.uf_set_stored_procedure_param(

            if (go_sqlConnection.State == ConnectionState.Closed)
            {
                go_sqlConnection.Open();
            }

            SqlCommand cmd = go_sqlConnection.CreateCommand();
            cmd.CommandText = "sp_saveendorsement";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = go_sqlConnection;

            cmd.Parameters.AddWithValue("@OldTransID", transactionId);
            cmd.Parameters.AddWithValue("@NewPolicyNo", newPolicyNo);
            SqlParameter newTransID = new SqlParameter()
            {
                ParameterName = "@NewTransID",
                DbType = System.Data.DbType.Int32,
                Value = 0,
                Size = int.MaxValue,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(newTransID);

            int result = cmd.ExecuteNonQuery();
            //NOTE: Must be assigned before leaving the method
            newId = 0;
            //if(result > 0){
            newId = (int)newTransID.Value;
            //}

            return UpdateMortgagee(newId, mortgagee);

        }

        public int SaveTransactionWithDeleteMortgagee(int transactionId,
            string newPolicyNo, out int newId)
        {
            //go_dah.uf_set_stored_procedure("sp_saveendorsement", ref go_sqlConnection);
            //go_dah.uf_set_stored_procedure_param(

            if (go_sqlConnection.State == ConnectionState.Closed)
            {
                go_sqlConnection.Open();
            }

            SqlCommand cmd = go_sqlConnection.CreateCommand();
            cmd.CommandText = "sp_saveendorsement";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = go_sqlConnection;

            cmd.Parameters.AddWithValue("@OldTransID", transactionId);
            cmd.Parameters.AddWithValue("@NewPolicyNo", newPolicyNo);
            SqlParameter newTransID = new SqlParameter()
            {
                ParameterName = "@NewTransID",
                DbType = System.Data.DbType.Int32,
                Value = 0,
                Size = int.MaxValue,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(newTransID);

            int result = cmd.ExecuteNonQuery();
            //NOTE: Must be assigned before leaving the method
            newId = 0;
            //if(result > 0){
            newId = (int)newTransID.Value;
            //}

            return DeleteMortgagee(newId);

        }

        public int SaveTransactionWithNewOwner(int transactionId, string newPolicyNo, out int newId, 
            int typeofinsurance,string designation, string lastname, string firstname, string mi, string multicorpname)
        {
            if (go_sqlConnection.State == ConnectionState.Closed)
            {
                go_sqlConnection.Open();
            }

            SqlCommand cmd = go_sqlConnection.CreateCommand();
            cmd.CommandText = "sp_saveendorsement";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = go_sqlConnection;

            cmd.Parameters.AddWithValue("@OldTransID", transactionId);
            cmd.Parameters.AddWithValue("@NewPolicyNo", newPolicyNo);
            SqlParameter newTransID = new SqlParameter()
            {
                ParameterName = "@NewTransID",
                DbType = System.Data.DbType.Int32,
                Value = 0,
                Size = int.MaxValue,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(newTransID);

            int result = cmd.ExecuteNonQuery();
            //NOTE: Must be assigned before leaving the method
            newId = 0;
            //if(result > 0){
            newId = (int)newTransID.Value;
            //}

            cmd.Parameters.Clear();

            cmd.CommandText = "sp_savecustomerinfo";
            cmd.Parameters.AddWithValue("@Designation", designation);
            cmd.Parameters.AddWithValue("@FirstName", firstname);
            cmd.Parameters.AddWithValue("@LastName", lastname);
            cmd.Parameters.AddWithValue("@MiddleName", mi);
            cmd.Parameters.AddWithValue("@MultipleCorporateName", multicorpname);
            cmd.Parameters.AddWithValue("@Address", string.Empty);
            cmd.Parameters.AddWithValue("@Telephone", string.Empty);
            cmd.Parameters.AddWithValue("@MobileNo", string.Empty);
            cmd.Parameters.AddWithValue("@Email", string.Empty);

            int newCustomerId = Convert.ToInt32(cmd.ExecuteScalar());

            return UpdateTransferOwnership(newId, newCustomerId, typeofinsurance);
            
        }


        public int SaveTransactionWithUpdatePolicyDate(int transactionId,
         string newPolicyNo, out int newId, DateTime from, DateTime to)
        {
            //go_dah.uf_set_stored_procedure("sp_saveendorsement", ref go_sqlConnection);
            //go_dah.uf_set_stored_procedure_param(

            if (go_sqlConnection.State == ConnectionState.Closed)
            {
                go_sqlConnection.Open();
            }

            SqlCommand cmd = go_sqlConnection.CreateCommand();
            cmd.CommandText = "sp_saveendorsement";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = go_sqlConnection;

            cmd.Parameters.AddWithValue("@OldTransID", transactionId);
            cmd.Parameters.AddWithValue("@NewPolicyNo", newPolicyNo);
            SqlParameter newTransID = new SqlParameter()
            {
                ParameterName = "@NewTransID",
                DbType = System.Data.DbType.Int32,
                Value = 0,
                Size = int.MaxValue,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(newTransID);

            int result = cmd.ExecuteNonQuery();
            //NOTE: Must be assigned before leaving the method
            newId = 0;
            //if(result > 0){
            newId = (int)newTransID.Value;
            //}

            return UpdatePolicyPeriod(newId, from, to);

        }

        public int SaveTransactionWithUpdatedVehicleDescription(int transactionId,
             string newPolicyNo, out int newId, int carcompany, string carmake, 
             int carseries, string engineSeries)
        {
            //go_dah.uf_set_stored_procedure("sp_saveendorsement", ref go_sqlConnection);
            //go_dah.uf_set_stored_procedure_param(

            if (go_sqlConnection.State == ConnectionState.Closed)
            {
                go_sqlConnection.Open();
            }

            SqlCommand cmd = go_sqlConnection.CreateCommand();
            cmd.CommandText = "sp_saveendorsement";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = go_sqlConnection;

            cmd.Parameters.AddWithValue("@OldTransID", transactionId);
            cmd.Parameters.AddWithValue("@NewPolicyNo", newPolicyNo);
            SqlParameter newTransID = new SqlParameter()
            {
                ParameterName = "@NewTransID",
                DbType = System.Data.DbType.Int32,
                Value = 0,
                Size = int.MaxValue,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(newTransID);

            int result = cmd.ExecuteNonQuery();
            //NOTE: Must be assigned before leaving the method
            newId = 0;
            //if(result > 0){
            newId = (int)newTransID.Value;
            //}

            return UpdateVehicleDescription(newId, carcompany, carmake, carseries, engineSeries);

        }

        public bool UpdateTransaction(Transaction transaction) {
            int result = 0;
            int perilResult = 0;
            int computationResult = 0;
            int carDetailResult = 0;
            int deleteCarDetailResult = 0;
            int deleteComputationResult = 0;
            int deletePerilResult = 0;
            int customerInfoResult = 0;

            using (go_sqlConnection)
            {
                if (go_sqlConnection.State == ConnectionState.Closed)
                {
                    go_sqlConnection.Open();
                }

                SqlCommand cmd = go_sqlConnection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_updatetransaction";

                SqlTransaction dataTransaction;
                dataTransaction = go_sqlConnection.BeginTransaction();

                cmd.Connection = go_sqlConnection;
                cmd.Transaction = dataTransaction;


                //TODO: Hardcoding the user id since there is no implementation for user accounts
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@userId", Value = 1, DbType = DbType.Int32 });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@creditingBranch", Value = transaction.CreditingBranch, DbType = DbType.Int32 });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@parNo", Value = transaction.ParNo, DbType = DbType.String });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@policyNo", Value = transaction.PolicyNo, DbType = DbType.String });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@geniisysNo", Value = transaction.GeniisysNo, DbType = DbType.String });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@dateCreated", Value = transaction.DateCreated, DbType = DbType.Date });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@policyPeriodFrom", Value = transaction.PolicyPeriodFrom, DbType = DbType.Date });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@policyPeriodTo", Value = transaction.PolicyPeriodTo, DbType = DbType.Date });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@businessType", Value = transaction.BussinessType, DbType = DbType.String });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@policyStatus", Value = transaction.PolicyStatus, DbType = DbType.String });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@sublineCode", Value = transaction.SubLineCode, DbType = DbType.String });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@mortgage", Value = transaction.MortgageCode, DbType = DbType.Int32 });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@intermediaryCode", Value = transaction.IntermediaryCode, DbType = DbType.Int32 });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@typeOfInsured", Value = transaction.TypeOfInsurance, DbType = DbType.Int32 });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@isPosted", Value = transaction.IsPosted, DbType = DbType.Boolean });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@isPrinted", Value = transaction.IsPrinted, DbType = DbType.Boolean });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@isEndorse", Value = transaction.IsEndorsed, DbType = DbType.Boolean });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Remarks", Value = transaction.Remarks, DbType = DbType.String });

                //cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Designation", Value = transaction.Customer.Designation, DbType = DbType.String });
                //cmd.Parameters.Add(new SqlParameter() { ParameterName = "@LastName", Value = transaction.Customer.LastName, DbType = DbType.String });
                //cmd.Parameters.Add(new SqlParameter() { ParameterName = "@FirstName", Value = transaction.Customer.FirstName, DbType = DbType.String });
                //cmd.Parameters.Add(new SqlParameter() { ParameterName = "@MiddleName", Value = transaction.Customer.MiddleName, DbType = DbType.String });
                //cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Address", Value = transaction.Customer.Address, DbType = DbType.String });
                //cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Telephone", Value = transaction.Customer.Telephone, DbType = DbType.String });
                //cmd.Parameters.Add(new SqlParameter() { ParameterName = "@MobileNo", Value = transaction.Customer.MobileNo, DbType = DbType.String });
                //cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Email", Value = transaction.Customer.Email, DbType = DbType.String });
                //cmd.Parameters.Add(new SqlParameter() { ParameterName = "@MultipleCorporateName", Value = transaction.Customer.MultipleCorporateName, DbType = DbType.String });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Original_TransactionID", Value = transaction.TransactionID, DbType = DbType.Int32 });
                
                try
                {
                    result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        SqlCommand deleteCarDetailCmd = go_sqlConnection.CreateCommand();
                        deleteCarDetailCmd.Connection = go_sqlConnection;
                        deleteCarDetailCmd.Transaction = dataTransaction;
                        deleteCarDetailCmd.CommandType = CommandType.Text;
                        deleteCarDetailCmd.CommandText = string.Format("DELETE FROM mTransactionCarDetail WHERE TransactionID={0}", transaction.TransactionID);
                        deleteCarDetailCmd.CommandTimeout = 30;

                        using (deleteCarDetailCmd)
                        {
                            deleteCarDetailResult = deleteCarDetailCmd.ExecuteNonQuery();
                        }

                        SqlCommand deletePerilCmd = go_sqlConnection.CreateCommand();
                        deletePerilCmd.Connection = go_sqlConnection;
                        deletePerilCmd.Transaction = dataTransaction;
                        deletePerilCmd.CommandType = CommandType.Text;
                        deletePerilCmd.CommandText = string.Format("DELETE FROM mTransactionPerils WHERE TransactionID={0}", transaction.TransactionID);
                        deletePerilCmd.CommandTimeout = 30;

                        using (deletePerilCmd)
                        {
                            deletePerilResult = deletePerilCmd.ExecuteNonQuery();
                        }

                        SqlCommand deleteComputationCmd = go_sqlConnection.CreateCommand();
                        deleteComputationCmd.Connection = go_sqlConnection;
                        deleteComputationCmd.Transaction = dataTransaction;
                        deleteComputationCmd.CommandType = CommandType.Text;
                        deleteComputationCmd.CommandText = string.Format("DELETE FROM mTransactionComputations WHERE TransactionID={0}", transaction.TransactionID);
                        deleteComputationCmd.CommandTimeout = 30;

                        using (deleteComputationCmd)
                        {
                            deleteComputationResult = deleteComputationCmd.ExecuteNonQuery();
                        }

                        SqlCommand carDetailCmd = go_sqlConnection.CreateCommand();
                        carDetailCmd.Connection = go_sqlConnection;
                        carDetailCmd.Transaction = dataTransaction;
                        carDetailCmd.CommandType = CommandType.StoredProcedure;
                        carDetailCmd.CommandText = "sp_savecardetail";
                        carDetailCmd.CommandTimeout = 30;

                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@TransactionID", Value = transaction.TransactionID, DbType = DbType.Int32 });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@CarCompany", Value = transaction.CarDetail.CarCompany, DbType = DbType.Int32 });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@CarYear", Value = transaction.CarDetail.CarYear, DbType = DbType.Int32 });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@CarSeries", Value = transaction.CarDetail.CarSeries, DbType = DbType.Int32 });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@CarMake", Value = transaction.CarDetail.CarMake, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@CarTypeOfBody", Value = transaction.CarDetail.CarTypeOfBodyID, DbType = DbType.Int32 });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@TypeOfCover", Value = transaction.CarDetail.TypeOfCover, DbType = DbType.Int32 });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@EngineSeries", Value = transaction.CarDetail.EngineSeries, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@MotorType", Value = transaction.CarDetail.MotorType, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@EngineNo", Value = transaction.CarDetail.EngineNo, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@Color", Value = transaction.CarDetail.Color, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@ConductionNo", Value = transaction.CarDetail.ConductionNo, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@ChassisNo", Value = transaction.CarDetail.ChassisNo, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@PlateNo", Value = transaction.CarDetail.PlateNo, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@Accessories", Value = transaction.CarDetail.Accessories, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@AuthenticationNo", Value = transaction.CarDetail.AuthenticationNo, DbType = DbType.String });
                        carDetailCmd.Parameters.Add(new SqlParameter() { ParameterName = "@COCNo", Value = transaction.CarDetail.COCNo, DbType = DbType.String });

                        using (carDetailCmd)
                        {
                            carDetailResult = carDetailCmd.ExecuteNonQuery();
                        }

                        string perilSql = cls_data_access_sqlgenerator.CreateSQLforPerils(transaction.Perils, transaction.TransactionID);
                        SqlCommand perilsCmd = go_sqlConnection.CreateCommand();

                        perilsCmd.Connection = go_sqlConnection;
                        perilsCmd.Transaction = dataTransaction;

                        perilsCmd.CommandType = CommandType.Text;
                        perilsCmd.CommandText = perilSql;
                        perilsCmd.CommandTimeout = 30;

                        using (perilsCmd)
                        {
                            perilResult = perilsCmd.ExecuteNonQuery();
                        }

                        string computationsSql = cls_data_access_sqlgenerator.CreateSQLforComputations(transaction.Computations.NetComputationDetails, transaction.Computations.GrossComputationDetails, transaction.TransactionID);

                        SqlCommand computationCmd = go_sqlConnection.CreateCommand();

                        computationCmd.Connection = go_sqlConnection;
                        computationCmd.Transaction = dataTransaction;

                        computationCmd.CommandType = CommandType.Text;
                        computationCmd.CommandText = computationsSql;
                        computationCmd.CommandTimeout = 30;

                        using (computationCmd)
                        {
                            computationResult = computationCmd.ExecuteNonQuery();
                        }

                        dataTransaction.Commit();
                    }
                    else
                    {

                    }
                }
                catch
                {
                    dataTransaction.Rollback();
                }
            }

            return result > 0 && perilResult > 0 && computationResult > 0 && carDetailResult > 0 && deleteCarDetailResult > 0 && deleteComputationResult > 0 && deletePerilResult > 0;
        }

        public Transaction GetTransactionById(int transactionId)
        {
            go_dah.uf_set_stored_procedure("sp_get_transactionbyid", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@TransactionID", transactionId);
            IDataReader reader = go_dah.uf_execute_reader();
            Transaction t = new Transaction();
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
                    //t.CarDetail.MotorType = reader.IsDBNull(33) ? string.Empty : reader.GetString(33);
                    //t.CarDetail.TypeOfCoverValue = reader.IsDBNull(34) ? 0 : reader.GetInt32(34);
                    //t.CarDetail.EngineSeriesText = reader.IsDBNull(35) ? string.Empty : reader.GetString(35);
                    //t.CarDetail.EngineNo = reader.IsDBNull(36) ? string.Empty : reader.GetString(36);
                    //t.CarDetail.Color = reader.IsDBNull(37) ? string.Empty : reader.GetString(37);
                    //t.CarDetail.ConductionNo = reader.IsDBNull(38) ? string.Empty : reader.GetString(38);
                    //t.CarDetail.ChassisNo = reader.IsDBNull(39) ? string.Empty : reader.GetString(39);
                    //t.CarDetail.PlateNo = reader.IsDBNull(40) ? string.Empty : reader.GetString(40);
                    //t.CarDetail.Accessories = reader.IsDBNull(41) ? string.Empty : reader.GetString(41);
                }
            }
            t.CarDetail = GetCarDetailByTransactionID(t.TransactionID);
            t.Computations = GetTransactionComputationsByTransactionId(t.TransactionID);
            t.Perils = GetTransactionPerilsByTransactionId(t.TransactionID);
            return t;
        }

        private CarDetail GetCarDetailByTransactionID(int transactionId) 
        {
            go_dah.uf_set_stored_procedure("sp_get_cardetailbytransactionid", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@TransactionID", transactionId);
            IDataReader reader = go_dah.uf_execute_reader();
            CarDetail detail = new CarDetail();
            using (reader)
            {
                int transactionIdIdx = reader.GetOrdinal("TransactionID");
                int carCompanyIdx = reader.GetOrdinal("CarCompany");
                int carYearIdx = reader.GetOrdinal("CarYear");
                int carSeriesIdx = reader.GetOrdinal("CarSeries"); 
                int carMakeIdx = reader.GetOrdinal("CarMake"); 
                int carTypeOfBodyIdx = reader.GetOrdinal("CarTypeOfBody"); 
                int typeOfCoverIdx = reader.GetOrdinal("TypeOfCover"); 
                int engineSeriesIdx = reader.GetOrdinal("EngineSeries"); 
                int motorTypeIdx = reader.GetOrdinal("MotorType"); 
                int engineNoIdx = reader.GetOrdinal("EngineNo"); 
                int colorIdx = reader.GetOrdinal("Color"); 
                int conductionNoIdx = reader.GetOrdinal("ConductionNo"); 
                int chassisNoIdx = reader.GetOrdinal("ChassisNo"); 
                int plateNoIdx = reader.GetOrdinal("PlateNo");
                int accessoriesIdx = reader.GetOrdinal("Accessories");
                int carCompanyNameIdx = reader.GetOrdinal("CarCompanyName");
                int carMakeNameIdx = reader.GetOrdinal("CarMakeName");
                int authenticationNoIdx = reader.GetOrdinal("AuthenticationNo");
                int cocNoIdx = reader.GetOrdinal("COCNo");
                int yearModelIdx = reader.GetOrdinal("yearModel");
                int coverNameIdx = reader.GetOrdinal("coverName");
                while (reader.Read())
                {
                    detail.TransactionID = reader.IsDBNull(transactionIdIdx) ? 0 : reader.GetInt32(transactionIdIdx);
                    detail.TypeOfCover = reader.IsDBNull(typeOfCoverIdx) ? 0 : reader.GetInt32(typeOfCoverIdx);
                    detail.CarCompany = reader.IsDBNull(carCompanyIdx) ? 0 : reader.GetInt32(carCompanyIdx);
                    detail.CarYear = reader.IsDBNull(carYearIdx) ? 0 : reader.GetInt32(carYearIdx);
                    detail.CarSeries = reader.IsDBNull(carSeriesIdx) ? 0 : reader.GetInt32(carSeriesIdx);
                    detail.CarMake = reader.IsDBNull(carMakeIdx) ? string.Empty : reader.GetString(carMakeIdx);
                    detail.CarTypeOfBodyID = reader.IsDBNull(carTypeOfBodyIdx) ? 0 : reader.GetInt32(carTypeOfBodyIdx);
                    detail.EngineSeries = reader.IsDBNull(engineSeriesIdx) ? string.Empty : reader.GetString(engineSeriesIdx);
                    detail.MotorType = reader.IsDBNull(motorTypeIdx) ? string.Empty : reader.GetString(motorTypeIdx);
                    detail.EngineNo = reader.IsDBNull(engineNoIdx) ? string.Empty : reader.GetString(engineNoIdx);
                    detail.Color = reader.IsDBNull(colorIdx) ? string.Empty : reader.GetString(colorIdx);
                    detail.ConductionNo = reader.IsDBNull(conductionNoIdx) ? string.Empty : reader.GetString(conductionNoIdx);
                    detail.ChassisNo = reader.IsDBNull(chassisNoIdx) ? string.Empty : reader.GetString(chassisNoIdx);
                    detail.PlateNo = reader.IsDBNull(plateNoIdx) ? string.Empty : reader.GetString(plateNoIdx);
                    detail.Accessories = reader.IsDBNull(accessoriesIdx) ? string.Empty : reader.GetString(accessoriesIdx);
                    detail.CarCompanyText = reader.IsDBNull(carCompanyNameIdx) ? string.Empty : reader.GetString(carCompanyNameIdx);
                    detail.CarMakeText = reader.IsDBNull(carMakeNameIdx) ? string.Empty : reader.GetString(carMakeNameIdx);
                    detail.AuthenticationNo = reader.IsDBNull(authenticationNoIdx) ? string.Empty : reader.GetString(authenticationNoIdx);
                    detail.COCNo = reader.IsDBNull(cocNoIdx) ? string.Empty : reader.GetString(cocNoIdx);
                    detail.CarYearText = reader.IsDBNull(yearModelIdx) ? "0" : reader.GetInt32(yearModelIdx).ToString();
                    detail.TypeOfCoverText = reader.IsDBNull(coverNameIdx) ? string.Empty : reader.GetString(coverNameIdx);
                }
            }
            return detail;
        }

        public List<TransactionPeril> GetTransactionPerilsByTransactionId(int transactionId)
        {
            go_dah.uf_set_stored_procedure("sp_get_perilsbytransactionid", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@TransactionID", transactionId);
            IDataReader reader = go_dah.uf_execute_reader();
            List<TransactionPeril> perils = new List<TransactionPeril>();
            using (reader)
            {
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

                    perils.Add(tp);
                }
            }
            return perils;
        }

        public TransactionComputation GetTransactionComputationsByTransactionId(int transactionId)
        {
            go_dah.uf_set_stored_procedure("sp_get_transactioncomputationsbytransactionid", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@TransactionID", transactionId);
            IDataReader reader = go_dah.uf_execute_reader();
            TransactionComputation c = new TransactionComputation();
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
            return c;
        }

        public List<CustomerInfo> GetNamesAutocomplete() 
        {
            go_dah.uf_set_stored_procedure("sp_getnamesautocomplete", ref go_sqlConnection);
            IDataReader reader = go_dah.uf_execute_reader();
            List<CustomerInfo> list = new List<CustomerInfo>();
            using (reader)
            {
                int recordIdIdx = reader.GetOrdinal("RecordID");
                int firstNameIdx = reader.GetOrdinal("FirstName");
                int lastNameIdx = reader.GetOrdinal("LastName");
                int middleNameIdx = reader.GetOrdinal("MiddleName");
                int addressIdx = reader.GetOrdinal("Address");
                int telephoneIdx = reader.GetOrdinal("Telephone");
                int mobileNoIdx = reader.GetOrdinal("MobileNo");
                int emailIdx = reader.GetOrdinal("Email");
                int designationIdx = reader.GetOrdinal("Designation");
                while (reader.Read())
                {
                    CustomerInfo ap = new CustomerInfo();
                    ap.CustomerID = reader.IsDBNull(recordIdIdx) ? 0 : reader.GetInt32(recordIdIdx);
                    ap.FirstName = reader.IsDBNull(firstNameIdx) ? string.Empty : reader.GetString(firstNameIdx);
                    ap.LastName = reader.IsDBNull(lastNameIdx) ? string.Empty : reader.GetString(lastNameIdx);
                    ap.MiddleName = reader.IsDBNull(middleNameIdx) ? string.Empty : reader.GetString(middleNameIdx);
                    ap.Address = reader.IsDBNull(addressIdx) ? string.Empty : reader.GetString(addressIdx);
                    ap.Telephone = reader.IsDBNull(telephoneIdx) ? string.Empty : reader.GetString(telephoneIdx);
                    ap.MobileNo = reader.IsDBNull(mobileNoIdx) ? string.Empty : reader.GetString(mobileNoIdx);
                    ap.Email = reader.IsDBNull(emailIdx) ? string.Empty : reader.GetString(emailIdx);
                    ap.Designation = reader.IsDBNull(designationIdx) ? string.Empty : reader.GetString(designationIdx);
                    list.Add(ap);
                }
            }
            return list;
        }

        public Dictionary<string, List<DropDownListItem>> LoadAllSearchFilters() {
            Dictionary<string, List<DropDownListItem>> filters = new Dictionary<string, List<DropDownListItem>>();

            go_dah.uf_set_stored_procedure("sp_get_all_searchfilters", ref go_sqlConnection);
            IDataReader reader = go_dah.uf_execute_reader();

            using (reader)
            {
                //crediting branches
                List<DropDownListItem> creditingBranches = new List<DropDownListItem>();
                int branchIdIdx = reader.GetOrdinal("creditingBrID");
                int branchNameIdx = reader.GetOrdinal("creditingBranchName");
                while (reader.Read()) {
                    DropDownListItem item = new DropDownListItem();
                    item.Value = reader.IsDBNull(branchIdIdx) ? "0" : reader.GetInt32(branchIdIdx).ToString();
                    item.Text = reader.IsDBNull(branchNameIdx) ? string.Empty : reader.GetString(branchNameIdx);
                    creditingBranches.Add(item);
                }

                filters.Add("creditingbranches", creditingBranches);

                if (reader.NextResult()) {
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

            return filters;
        }

        public IEnumerable<TransactionSearchResultDTO> SearchTransaction(string whereClause)
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

            if(!string.IsNullOrEmpty(whereClause))
            {
                sql.AppendFormat(" WHERE {0}", whereClause);
            }

            sql.Append(" ORDER BY t.dateCreated DESC ");

            go_dah.uf_set_sql_statement(sql.ToString(), ref go_sqlConnection);
            IDataReader reader = go_dah.uf_execute_reader();

            List<TransactionSearchResultDTO> ts = new List<TransactionSearchResultDTO>();
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
                    TransactionSearchResultDTO t = new TransactionSearchResultDTO();
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
            return ts;
        }

        public List<Endorsement> GetAllEndorsement() {
            go_dah.uf_set_stored_procedure("sp_getallendorsement", ref go_sqlConnection);
            IDataReader reader = go_dah.uf_execute_reader();
            List<Endorsement> list = new List<Endorsement>();

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

            return list;
        }

        public Endorsement GetOneEndorsement(int endorsementCode)
        {
            go_dah.uf_set_stored_procedure("sp_getendorsementbycode", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@endtCode", endorsementCode);
            IDataReader reader = go_dah.uf_execute_reader();
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

            return e;
        }

        public int UpdateCOCNo(string newCocNo, int transactionId)
        {
            string sql = string.Format(
                "UPDATE mTransactionCarDetail SET [COCNo] = '{0}' WHERE TransactionID={1}",
                newCocNo, transactionId);
            go_dah.uf_set_sql_statement(sql, ref go_sqlConnection);
            return go_dah.uf_execute_non_query();
        }

        public int UpdateInsuredName(int recordId, string newFirstName, string newLastName,
                                    string newMi) 
        {
            string sql = string.Format(
                "UPDATE mTransactionCustomerInfo SET [FirstName] = '{0}',[LastName]='{1}',[MiddleName]='{2}' WHERE RecordID={3}",
                newFirstName,newLastName, newMi, recordId);
                        go_dah.uf_set_sql_statement(sql, ref go_sqlConnection);
                        return go_dah.uf_execute_non_query();
        }

        public int UpdateAddress(int recordId, string newAddress)
        {
            string sql = string.Format(
                "UPDATE mTransactionCustomerInfo SET [Address] = '{0}' WHERE RecordID={1}",
                 newAddress, recordId);
            go_dah.uf_set_sql_statement(sql, ref go_sqlConnection);
            return go_dah.uf_execute_non_query();
        }

        public int UpdateMortgagee(int transactionId, string mortgagee)
        {
            string sql = string.Format(
                "UPDATE mTransactions SET [mortgage] = {0} WHERE TransactionID={1}",
                 mortgagee, transactionId);
            go_dah.uf_set_sql_statement(sql, ref go_sqlConnection);
            return go_dah.uf_execute_non_query();
        }

        public int DeleteMortgagee(int transactionId)
        {
            string sql = string.Format(
                "UPDATE mTransactions SET [mortgage] = 0 WHERE TransactionID={0}", transactionId);
            go_dah.uf_set_sql_statement(sql, ref go_sqlConnection);
            return go_dah.uf_execute_non_query();
        }

        public int UpdatePolicyPeriod(int transactionId, DateTime from, DateTime to)
        {
            string sql = string.Format(
                "UPDATE mTransactions SET [policyPeriodFrom] = @PolicyPeriodFrom,[policyPeriodTo] = @PolicyPeriodTo WHERE TransactionID={0}", transactionId);
            go_dah.uf_set_sql_statement(sql, ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@PolicyPeriodFrom", from);
            go_dah.uf_set_stored_procedure_param("@PolicyPeriodTo", to);
            return go_dah.uf_execute_non_query();
        }

        public int UpdateVehicleDescription(int transactionId, int carcompany,
            string carmake, int carseries, string engineseries)
        {
            string sql = string.Format(
                "UPDATE mTransactionCarDetail SET [CarCompany] = {0},[CarMake] = '{1}',[CarSeries]={2},[EngineSeries]='{3}' WHERE TransactionID={4}",
                        carcompany, carmake, carseries, engineseries, transactionId);
            go_dah.uf_set_sql_statement(sql, ref go_sqlConnection);
            return go_dah.uf_execute_non_query();
        }

        public int UpdateTransferOwnership(int transactionId, int customerId, int typeofinsurance)
        {
            string sql = string.Format(
                "UPDATE mTransactions SET [CustomerID] = @CustomerID,[typeOfInsured]=@typeOfInsured WHERE TransactionID={0}", transactionId);
            go_dah.uf_set_sql_statement(sql, ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@CustomerID", customerId);
            go_dah.uf_set_stored_procedure_param("@typeOfInsured", typeofinsurance);
            return go_dah.uf_execute_non_query();
        }

        public bool SaveEndorsementDetails(int transactionId, int newTransactionId,
                            string endorsementText, DateTime dateEndorsed,
                            DateTime effectivityDate, DateTime expiryDate)
        {
            go_dah.uf_set_stored_procedure("sp_saveendorsementdetails", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@ParentTransactionID", transactionId);
            go_dah.uf_set_stored_procedure_param("@NewTransactionID", newTransactionId);
            go_dah.uf_set_stored_procedure_param("@EndorsementText", endorsementText);
            go_dah.uf_set_stored_procedure_param("@DateEndorsed", dateEndorsed);
            go_dah.uf_set_stored_procedure_param("@EffectivityDate", effectivityDate);
            go_dah.uf_set_stored_procedure_param("@ExpiryDate", expiryDate);

            return go_dah.uf_execute_non_query() > 0;

        }

        public EndorsementDetail GetEndorsementDetail(int id)
        {
            go_dah.uf_set_stored_procedure("sp_getendorsementdetailbyid", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@NewTransactionID", id);

            IDataReader reader = go_dah.uf_execute_reader();
            EndorsementDetail endorsementDetail = null;
            using (reader)
            {
                int parentIdIdx = reader.GetOrdinal("ParentTransactionID");
                int newIdIdx = reader.GetOrdinal("NewTransactionID");
                int eTextIdx = reader.GetOrdinal("EndorsementText");
                int dateEndorsedId = reader.GetOrdinal("DateEndorsed");
                int effectivityDateIdx = reader.GetOrdinal("EffectivityDate");
                int expDateIdx = reader.GetOrdinal("ExpiryDate");

                while (reader.Read())
                {
                    endorsementDetail = new EndorsementDetail();
                    endorsementDetail.ParentTransactionID = reader.GetInt32(parentIdIdx);
                    endorsementDetail.NewTransactionID = reader.GetInt32(newIdIdx);
                    endorsementDetail.EndorsementText = reader.GetString(eTextIdx);
                    endorsementDetail.DateEndorsed = reader.GetDateTime(dateEndorsedId);
                    endorsementDetail.EffectivityDate = reader.GetDateTime(effectivityDateIdx);
                    endorsementDetail.ExpiryDate = reader.GetDateTime(expDateIdx);
                }
            }
            return endorsementDetail;

        }

        public List<PerilsDefault> GetAllPerilsDefaults()
        {
            go_dah.uf_set_stored_procedure("sp_getallperilsdefaults", ref go_sqlConnection);
            IDataReader reader = go_dah.uf_execute_reader();
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
            return defaults;
        }

        public int PostTransaction(int transactionId)
        {
            go_dah.uf_set_stored_procedure("sp_posttransaction", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@TransactionID", transactionId);
            return go_dah.uf_execute_non_query();
        }
    }
}

