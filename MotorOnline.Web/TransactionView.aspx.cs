using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using MotorOnline.Library.Entity;

namespace MotorOnline.Web
{
    public partial class TransactionView : cls_base_page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Created date is set to Date now
                lblCurrentDate.Text = DateTime.Now.ToString("MMM dd, yyyy");

                //Dropdowns on the main page
                PopulateCreditingBranch();
                PopulateSublines();
                PopulateMortgagee();
                PopulateIntermediaries();
                PopulateTypeOfInsurance();

                //Dropdowns on the popup
                PopulateCoverTypes();
                PopulateTypesOfBody();
                PopulateCarYears();
                PopulateCarCompany();

                this.lblParNo.Text = ParNoHelper.GenerateValidParNo();
                this.lblPolicyNo.Text = PolicyNoHelper.GenerateValidPolicyNo();

                //Load data early for autocomplete later
                GetNamesAutocomplete();

                //Load transaction if there is an id
                string transactionId = Request.QueryString.Get("id");
                if (!string.IsNullOrEmpty(transactionId))
                {
                    int id = ChangeTypeHelper.SafeParseToInt32(transactionId);

                    this.IdHiddenField.Value = id.ToString();



                    //JavaScriptSerializer serializer = new JavaScriptSerializer();
                    //string json = serializer.Serialize(t); 

                    //ClientScript.RegisterStartupScript(this.GetType(), "on_load", string.Format("loadtransaction('{0}', '{1}');", json, id), true);
                }
            }
        }

        public void PopulateCreditingBranch()
        {
            uf_populate_ddlb(ao_ddlb: ddlCreditingBranch, adt_source: go_dal.uf_pop_mCreditingBranches(), as_empty_option_text: "Select Crediting Branch");
            ddlCreditingBranch.SelectedIndex = 0;
        }

        public void PopulateCoverTypes()
        {
            uf_populate_ddlb(ao_ddlb: TypeOfCoverDropdown, adt_source: go_dal.sp_pop_coverTypes(), as_empty_option_text: "Select Cover Type ");
            TypeOfCoverDropdown.SelectedIndex = 0;
        }

        public void PopulateMortgagee()
        {
            uf_populate_ddlb(ao_ddlb: ddlMortgagee, adt_source: go_dal.sp_pop_mMortgagee(), as_empty_option_text: "Select Mortgagee");
            ddlMortgagee.SelectedIndex = 0;
        }

        public void PopulateIntermediaries()
        {
            uf_populate_ddlb(ao_ddlb: ddInterMediary, adt_source: go_dal.sp_pop_mIntermediaries(), as_empty_option_text: "Select Intermediary");
            ddInterMediary.SelectedIndex = 0;
        }

        public void PopulateTypeOfInsurance()
        {
            uf_populate_ddlb(ao_ddlb: TypeOfInsuranceDropdown, adt_source: go_dal.sp_pop_typeOfInsurance(), as_empty_option_text: "Select Type of Cover");
            TypeOfInsuranceDropdown.SelectedIndex = 0;
        }

        public void PopulateSublines()
        {
            uf_populate_ddlb(ao_ddlb: SublineDropdown, adt_source: go_dal.uf_pop_mSublines(), as_empty_option_text: "Select Subline");
            SublineDropdown.SelectedIndex = 0;
        }

        public void PopulateTypesOfBody()
        {
            uf_populate_ddlb(ao_ddlb: TypeOfBodyDropdown, adt_source: go_dal.sp_pop_carTypesOfBody(), as_empty_option_text: "Select Body Type");
            TypeOfBodyDropdown.SelectedIndex = 0;
        }

        public void PopulateCarYears() {
            uf_populate_ddlb(ao_ddlb: YearDropdown, 
            adt_source: go_dal.sp_pop_carYears_reader(), 
            as_empty_option_text: "Select Car Year");
            YearDropdown.SelectedIndex = 0;
        }

        public void PopulateCarCompany()
        {
            uf_populate_ddlb(ao_ddlb: CarCompaniesDropdown, adt_source: go_dal.sp_pop_CarCompanies(), as_empty_option_text: "Select Car Company");
            CarCompaniesDropdown.SelectedIndex = 0;
        }

        public void GetNamesAutocomplete() {
            List<CustomerInfo> aps =  go_dal.GetNamesAutocomplete();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            this.NamesAutocompleteHiddenField.Value = serializer.Serialize(aps);
        }
    }
}