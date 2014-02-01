using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Caching;


namespace MotorOnline.Web
{
    public class cls_data_access_helper : IDisposable
    {

        #region " Object Declarations "

        private bool gb_disposed;
        private char gs_list_delimiter;
        private int gi_sql_command_timeout;
        private SqlCommand go_sql_command;

        #endregion

        #region " Base Functions "

        public cls_data_access_helper()
        {
            gb_disposed = false;
            gs_list_delimiter = Convert.ToChar(",");
            gi_sql_command_timeout = 30;
        }

        /// Destructor method
        /// </summary>
        /// <remarks>This will run only if the Dispose method does not get called.
        /// </remarks>
        ~cls_data_access_helper()
        {
            Dispose(false);
        }

        /// <summary>
        /// This method is called when all resources (managed, unmanaged) should disposed explicitly
        /// </summary>
        /// <remarks>Cleans up the objects instantiated for use within this class</remarks>
        public void Dispose()
        {
            Dispose(true);
            //Take this class off the Finalization queue to prevent finalization code for this object from executing twice
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Where all resources should be disposed
        /// </summary>
        /// <param name="ab_disposing">Set to true if all managed and unmanage resources should be disposed.</param>
        protected virtual void Dispose(bool ab_disposing)
        {
            if (!gb_disposed)
            {
                if (ab_disposing)
                {
                    if (go_sql_command != null)
                    {
                        go_sql_command.Dispose();
                        go_sql_command = null;
                    }
                }
                //Release unmanaged resources here
            }
            gb_disposed = true;
        }


        public void uf_set_stored_procedure(string as_stored_procedure_name, ref SqlConnection ao_database_connection)
        {
            go_sql_command = new SqlCommand(as_stored_procedure_name, ao_database_connection);
            go_sql_command.CommandType = CommandType.StoredProcedure;
            go_sql_command.CommandTimeout = gi_sql_command_timeout;
        }

        public void uf_set_stored_procedure(string as_stored_procedure_name, ref SqlTransaction at_database_transaction)
        {
            go_sql_command = new SqlCommand(as_stored_procedure_name, at_database_transaction.Connection);
            go_sql_command.CommandType = CommandType.StoredProcedure;
            go_sql_command.CommandTimeout = gi_sql_command_timeout;
            go_sql_command.Transaction = at_database_transaction;
        }

        public void uf_set_stored_procedure_param(string as_command_parameter, object as_parameter_value, bool ab_check_is_empty = false, bool ab_parse_list = false)
        {
            if (ab_check_is_empty && (as_parameter_value == null || as_parameter_value == ""))
            {
                return;
            }
            if (ab_parse_list)
            {
                as_parameter_value = uf_parse_list(as_parameter_value.ToString());
            }
            go_sql_command.Parameters.AddWithValue(as_command_parameter, as_parameter_value);
        }

        public void uf_set_sql_statement(string as_sql_statement, ref SqlConnection ao_database_connection)
        {
            go_sql_command = new SqlCommand(as_sql_statement, ao_database_connection);
            go_sql_command.CommandType = CommandType.Text;
            go_sql_command.CommandTimeout = gi_sql_command_timeout;
        }

        public void uf_set_sql_statement(string as_sql_statement, ref SqlTransaction at_database_transaction)
        {
            go_sql_command = new SqlCommand(as_sql_statement, at_database_transaction.Connection);
            go_sql_command.CommandType = CommandType.Text;
            go_sql_command.CommandTimeout = gi_sql_command_timeout;
            go_sql_command.Transaction = at_database_transaction;
        }

        public void uf_set_timeout(int ai_timeout)
        {
            gi_sql_command_timeout = ai_timeout;
        }


        /// <summary>
        /// Use to execute a database modification process
        /// </summary>
        /// <returns>Row(s) affected</returns>
        /// <remarks></remarks>
        public int uf_execute_non_query()
        {
            int li_rows_affected = 0;
            try
            {
                if (!(go_sql_command.Connection.State == ConnectionState.Open))
                    go_sql_command.Connection.Open();

                li_rows_affected = go_sql_command.ExecuteNonQuery();

                return li_rows_affected;

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Use to execute a single unit data request
        /// </summary>
        /// <returns>Data requested (single unit)</returns>
        /// <remarks></remarks>
        public object uf_execute_scalar()
        {
            object lo_result = null;
            try
            {
                if (!(go_sql_command.Connection.State == ConnectionState.Open))
                    go_sql_command.Connection.Open();

                lo_result = go_sql_command.ExecuteScalar();

                return lo_result;

            }
            catch
            {
                throw;
            }
        }
                
        /// <summary>
        /// Deprecated: Use to execute a multiple resultset data request
        /// </summary>
        /// <returns>Data requested (set of resultsets)</returns>
        /// <remarks></remarks>
        public DataSet uf_execute_dataset()
        {
            SqlDataAdapter lda_data_adapter = new SqlDataAdapter();
            DataSet lds_return_data = new DataSet();
            try
            {
                lda_data_adapter.SelectCommand = go_sql_command;
                lda_data_adapter.Fill(lds_return_data);

                return lds_return_data;

            }
            catch
            {
                throw;
            }
            finally
            {
                if ((lda_data_adapter != null))
                    lda_data_adapter.Dispose();
                lda_data_adapter = null;
                if ((lds_return_data != null))
                    lds_return_data.Dispose();
                lds_return_data = null;
            }
        }

        /// <summary>
        /// Use to execute a multiple resultset data request
        /// </summary>
        /// <returns>Data requested (set of resultsets)</returns>
        /// <remarks></remarks>
        public DataSet uf_execute_data_set()
        {
            SqlDataAdapter lda_data_adapter = new SqlDataAdapter();
            DataSet lds_return_data = new DataSet();
            try
            {
                lda_data_adapter.SelectCommand = go_sql_command;
                lda_data_adapter.Fill(lds_return_data);

                return lds_return_data;

            }
            catch
            {
                throw;
            }
            finally
            {
                if ((lda_data_adapter != null))
                    lda_data_adapter.Dispose();
                lda_data_adapter = null;
                if ((lds_return_data != null))
                    lds_return_data.Dispose();
                lds_return_data = null;
            }
        }

        /// <summary>
        /// Use to execute a single resultset data request
        /// </summary>
        /// <returns>Data requested (resultset)</returns>
        /// <remarks></remarks>
        public DataTable uf_execute_data_table()
        {
            SqlDataAdapter lda_data_adapter = new SqlDataAdapter();
            DataTable ldt_return_data = new DataTable();
            try
            {
                lda_data_adapter.SelectCommand = go_sql_command;
                lda_data_adapter.Fill(ldt_return_data);

                return ldt_return_data;

            }
            catch
            {
                throw;
            }
            finally
            {
                if ((lda_data_adapter != null))
                    lda_data_adapter.Dispose();
                lda_data_adapter = null;
                if ((ldt_return_data != null))
                    ldt_return_data.Dispose();
                ldt_return_data = null;
            }
        }

        public IDataReader uf_execute_reader() 
        {
            IDataReader reader = null;
            try
            {
                if (!(go_sql_command.Connection.State == ConnectionState.Open))
                    go_sql_command.Connection.Open();

                reader = go_sql_command.ExecuteReader();

                return reader;

            }
            catch
            {
                throw;
            }
        }

        public string uf_parse_list(string as_list)
        {
            string[] ls_array = as_list.Split(Convert.ToChar(gs_list_delimiter));
            List<string> ls_list = new List<string>();
            foreach (string ls in ls_array)
            {
                ls_list.Add("'" + ls.Replace("'", "''") + "'");
            }
            return string.Join(gs_list_delimiter.ToString(), ls_list.ToArray());
        }

        public DataTable uf_filter_data(DataTable adt, string as_filter)
        {
            if (adt == null)
            {
                return new DataTable();
            }
            else
            {
                using (DataTable ldt = adt.Copy())
                {
                    if (ldt.Rows.Count > 0)
                    {
                        ldt.DefaultView.RowFilter = as_filter;
                        return ldt.DefaultView.ToTable().Copy();
                    }
                    else
                    {
                        return ldt;
                    }
                }
            }
        }

        protected bool uf_is_empty(string ao_string)
        {
            if (ao_string == null || string.IsNullOrEmpty(ao_string.Trim()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}