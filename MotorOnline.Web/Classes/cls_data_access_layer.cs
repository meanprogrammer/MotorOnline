using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using MotorOnline.Library.Entity;

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

        public List<Mortgagee> GetAllMortgagee() {
            go_dah.uf_set_stored_procedure("sp_pop_mMortgagee", ref go_sqlConnection);
            IDataReader reader = go_dah.uf_execute_reader();
            List<Mortgagee> mortgagees = new List<Mortgagee>();
            using (reader)
            {
                int valueIdx = reader.GetOrdinal("VALUE");
                int textIdx = reader.GetOrdinal("TEXT");
                while (reader.Read())
                {
                    Mortgagee mg = new Mortgagee();
                    mg.MortgageeID = reader.GetInt32(valueIdx);
                    mg.MortgageeName = reader.GetString(textIdx);

                    mortgagees.Add(mg);
                }
            }
            return mortgagees;
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



        public double GetCtplDefault(string subline) {
            go_dah.uf_set_stored_procedure("sp_getctpldefaultbysubline", ref go_sqlConnection);
            go_dah.uf_set_stored_procedure_param("@SublineCode", subline.Trim().ToUpper());
            object result = go_dah.uf_execute_scalar();
            return ChangeTypeHelper.SafeParseToDouble(result.ToString());
        }

        #endregion

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
        
        public List<TypeOfInsurance> GetTypeOfInsurance()
        {
            go_dah.uf_set_stored_procedure("sp_pop_typeOfInsurance", ref go_sqlConnection);
            IDataReader reader = go_dah.uf_execute_reader();
            List<TypeOfInsurance> insurances = new List<TypeOfInsurance>();
            using (reader)
            {
                int valueIdx = reader.GetOrdinal("VALUE");
                int textIdx = reader.GetOrdinal("TEXT");
                while (reader.Read())
                {
                    TypeOfInsurance toi = new TypeOfInsurance();
                    toi.TypeOfInsuranceID = reader.GetInt32(valueIdx);
                    toi.TypeOfInsuranceName = reader.GetString(textIdx);

                    insurances.Add(toi);
                }
            }
            return insurances;
        }
    }
}


