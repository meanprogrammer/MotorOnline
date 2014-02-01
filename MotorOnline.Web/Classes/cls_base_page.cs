using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;


namespace MotorOnline.Web
{
    public class cls_base_page : System.Web.UI.Page
    {
        public cls_data_access_layer go_dal;

        public cls_base_page()
        {
            go_dal = new cls_data_access_layer();
        }

        protected bool uf_is_date(object ao_date)
        {
            try
            {
                if (uf_is_empty(ao_date.ToString()))
                    return false;

                Convert.ToDateTime(ao_date);
                return true;
            }
            catch
            {
                return false;
            }
        }


        protected bool uf_date_in_correct_format(object ao_date)
        {
            string ls_correct_format;    
                               
            try
            {
                ls_correct_format = Convert.ToDateTime(ao_date).ToString("d", new System.Globalization.CultureInfo("ja-JP"));  
                if (ls_correct_format == ao_date.ToString())
                    return true;
            }
            catch
            {
                return false;
            }

            return false;
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

        public int uf_cint(object ao_value, int ai_default = 0)
        {
            if (ao_value == null | Convert.IsDBNull(ao_value))
            {
                return ai_default;
            }
            else if (string.IsNullOrEmpty(Convert.ToString(ao_value)) | !uf_is_integer(ao_value))
            {
                return ai_default;
            }
            else
            {
                return Convert.ToInt32(ao_value);
            }
        }

        /// <summary>
        /// Checks if a value can be converted into Integer.
        /// </summary>
        /// <param name="ao_integer">Value to be checked whether convertable to Integer data type.</param>
        /// <returns>True if the value is convertible to Integer; False if not.</returns>
        /// <remarks></remarks>
        public static bool uf_is_integer(object ao_integer)
        {
            int li_integer = 0;
            try
            {
                li_integer = Convert.ToInt32(ao_integer);

                return true;

            }
            catch
            {
                return false;
            }
        }

        public static decimal uf_cdec(object ao_value, decimal ai_default = 0)
        {
            if (ao_value == null | Convert.IsDBNull(ao_value))
            {
                return ai_default;
            }
            else if (string.IsNullOrEmpty(Convert.ToString(ao_value)) | !uf_is_decimal(ao_value))
            {
                return ai_default;
            }
            else
            {
                return Convert.ToDecimal(ao_value);
            }
        }

        /// <summary>
        /// Checks if a value can be converted into Decimal.
        /// </summary>
        /// <param name="ao_decimal">Value to be checked whether convertable to Decimal data type.</param>
        /// <returns>True if the value is convertible to Decimal; False if not.</returns>
        /// <remarks></remarks>
        public static bool uf_is_decimal(object ao_decimal)
        {
            decimal ld_decimal = default(decimal);
            try
            {
                ld_decimal = Convert.ToDecimal(ao_decimal);

                return true;

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Populates a DDLB
        /// </summary>
        /// <param name="ao_ddlb"></param>
        /// <param name="adt_source"></param>
        /// <param name="as_valuefield"></param>
        /// <param name="as_textfield"></param>
        public void uf_populate_ddlb(DropDownList ao_ddlb, DataTable adt_source, bool ab_empty_option = true, string as_empty_option_text ="Select", string as_valuefield = "VALUE", string as_textfield = "TEXT")
        {
            DataRow ldr_empty_row = adt_source.NewRow();

            ldr_empty_row[as_valuefield] = 0;
            ldr_empty_row[as_textfield] = as_empty_option_text.ToString();

            adt_source.Rows.InsertAt(ldr_empty_row, 0);

            ao_ddlb.DataSource = adt_source;
            ao_ddlb.DataValueField = as_valuefield;
            ao_ddlb.DataTextField = as_textfield;
            ao_ddlb.DataBind();
        }

        public void uf_populate_ddlb(DropDownList ao_ddlb, IDataReader adt_source, bool ab_empty_option = true, string as_empty_option_text = "Select", string as_valuefield = "VALUE", string as_textfield = "TEXT")
        {

            List<ListItem> c = new List<ListItem>();
            c.Add(new ListItem(as_empty_option_text, "0"));
            using (adt_source)
            {
                while (adt_source.Read())
                {
                    c.Add(
                        new ListItem() { 
                            Value = adt_source.GetInt32(0).ToString(),
                            Text = adt_source.GetInt32(1).ToString()
                        }
                        );
                }
            }

            ao_ddlb.Items.AddRange(c.ToArray());
            ao_ddlb.DataValueField = as_valuefield;
            ao_ddlb.DataTextField = as_textfield;
            ao_ddlb.DataBind();

        }

        #region " Footer Messages "

        public DataTable uf_create_message_table()
        {
            using (DataTable ldt_msg = new DataTable())
            {
                ldt_msg.Columns.Add("MSG");
                return ldt_msg;
            }
        }

        public void uf_add_message_table_row(ref DataTable adt_msg, string as_msg)
        {
            DataRow ldr = adt_msg.NewRow();
            ldr["MSG"] = as_msg;
            adt_msg.Rows.Add(ldr);
        }

        #endregion

        #region " Session Variables "

        public string p_ses_account_type
        {
            get {
                if (Session["acct_type"] == null)
                    return "";
                else
                    return Session["acct_type"].ToString();
            }
            set {
                if (Session["acct_type"] == null)
                    Session.Add("acct_type", value);
                else
                    Session["acct_type"] = value;
            }
        }


        #endregion

    }


}