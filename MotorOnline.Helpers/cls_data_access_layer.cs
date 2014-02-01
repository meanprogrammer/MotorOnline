using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace MotorOnline
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

        public string get_lastparno() {
            go_dah.uf_set_stored_procedure("sp_getlastparno", ref go_sqlConnection);
            object result = go_dah.uf_execute_scalar();
            return result.ToString();
        }

        public string get_lastpolicyno()
        {
            go_dah.uf_set_stored_procedure("sp_getlastpolicyno", ref go_sqlConnection);
            object result = go_dah.uf_execute_scalar();
            return result.ToString();
        }

        public DataTable uf_pop_mSublines()
        {
            go_dah.uf_set_stored_procedure("sp_pop_mSublines", ref go_sqlConnection);
            return go_dah.uf_execute_data_table();
        }

        //public DataTable uf_pop_mBusinessTypes()
        //{
        //    go_dah.uf_set_stored_procedure("sp_pop_mBusinessType", ref go_sqlConnection);
        //    return go_dah.uf_execute_data_table();
        //}

        public DataTable sp_pop_coverTypes()
        {
            go_dah.uf_set_stored_procedure("sp_pop_coverTypes", ref go_sqlConnection);
            return go_dah.uf_execute_data_table();
        }

        public DataTable sp_pop_mMortgagee()
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

        #endregion
    }
}