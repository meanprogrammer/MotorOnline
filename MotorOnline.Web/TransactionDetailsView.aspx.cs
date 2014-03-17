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
    public partial class TransactionDetailsView : cls_base_page
    {
        private User _currentUser;
        public User CurrentUser {
            get {
                return Session[string.Format("user_{0}", Session.SessionID)] as User;
            }
            set {
                this._currentUser = value;
            } 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.CurrentUser.UserRole.CanViewTransaction)
            {
                Response.Redirect("NotAllowed.aspx", true);
            }
            //if (!Page.IsPostBack)
            //{
            //    //Created date is set to Date now
            //    lblCurrentDate.Text = DateTime.Now.ToString("MMM dd, yyyy");

            //    //Dropdowns on the main page
            //    PopulateCreditingBranch();
            //    PopulateSublines();
            //    PopulateMortgagee();
            //    PopulateIntermediaries();
            //    PopulateTypeOfInsurance();

            //    //Dropdowns on the popup
            //    PopulateCoverTypes();
            //    PopulateTypesOfBody();
            //    PopulateCarYears();
            //    PopulateCarCompany();

            //    this.lblParNo.Text = ParNoHelper.GenerateValidParNo();
            //    this.lblPolicyNo.Text = PolicyNoHelper.GenerateValidPolicyNo();

            //    //Load data early for autocomplete later
            //    GetNamesAutocomplete();

            //TODO: Load All endorsements but later load this if the user is admin
            PopuplateEndorsement();

            //    //Load transaction if there is an id
            string transactionId = Request.QueryString.Get("id");
            if (!string.IsNullOrEmpty(transactionId))
            {
                int id = ChangeTypeHelper.SafeParseToInt32(transactionId);

                this.IdHiddenField.Value = id.ToString();



                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //string json = serializer.Serialize(t); 

                //ClientScript.RegisterStartupScript(this.GetType(), "on_load", string.Format("loadtransaction('{0}', '{1}');", json, id), true);
            }
            //}
        }

        public void PopuplateEndorsement()
        {
            cls_data_access_layer dl = new cls_data_access_layer();
            List<Endorsement> endorsements = dl.GetAllEndorsement();
            foreach (Endorsement e in endorsements)
            {
                this.EndorsementDropdown.Items.Add(new ListItem(e.EndorsementTitle, e.EndorsementCode.ToString()));
            }
            this.EndorsementDropdown.SelectedIndex = 0;
        }

        public void GetNamesAutocomplete() {
            List<CustomerInfo> aps =  go_dal.GetNamesAutocomplete();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            this.NamesAutocompleteHiddenField.Value = serializer.Serialize(aps);
        }
    }
}