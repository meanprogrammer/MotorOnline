using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Serialization;
using MotorOnline.Helpers;
using System.Text;
using System.Globalization;
using MotorOnline.Library.Entity;

namespace MotorOnline.Web.ajax
{
    public partial class TransactionAjax : System.Web.UI.Page
    {
        private cls_data_access_layer data = null;
        public TransactionAjax() {
            data = new cls_data_access_layer();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var action = Request.Form["action"];
            switch (action)
            {
                case "getcarcompanies":
                    HandleGetCarCompanies();
                    break;
                case "filtercarmake":
                    HandleFilterCarMake();
                    break;
                case "getperils":
                    HandleGetPerils();
                    break;
                case "getperilsedit":
                    HandleGetPerilsEdit();
                    break;
                case "updatetransaction":
                    HandleUpdateTransaction();
                    break;
                case "filterengine":
                    HandleFilterEngine();
                    break;
                case "getexistingpolicy":
                    HandleFilterExistingPolicy();
                    break;
                case "gettariffrates":
                    HandleGetTrariffRates();
                    break;
                case "computedetails":
                    HandleComputeDetails();
                    break;
                case "savetransaction":
                    HandleSaveTransaction();
                    break;
                case "getbodytypes":
                    HandleGetBodyTypes();
                    break;
                case "getcaryears":
                    HandleGetCarYears();
                    break;
                case "getcardetailsoptions":
                    HandleGetCarDetailsOptions();
                    break;
                case "gettransactionbyid":
                    HandleGetTransaction();
                    break;
                case "loadsearchfilters":
                    HandleLoadSearchFilters();
                    break;
                case "searchtransactions":
                    HandleSearchTransactions();
                    break;
                //NOT USED
                case "loadallendorement":
                    HandleLoadAllEndorsement();
                    break;
                case "getendorsementbycode":
                    HandleGetEndorsementByCode();
                    break;
                case "updateendorsement":
                    HandleUpdateEndorsement();
                    break;
                default:
                    break;
            }
        }


        private void HandleUpdateEndorsement()
        {
            var type = Request.Form["type"];
            var transactionId = Request.Form["transactionid"];
            var customerId = Request.Form["customerid"];
            int result = 0;
            int newId = 0;
            switch (type)
            {
                case "3":
                    var newCocNo = Request.Form["newcocno"];
                    var currentPolicyNo = Request.Form["policyno"];
                    
                    result = data.SaveTransactionWithUpdatedCOCNo(
                        int.Parse(transactionId), 
                        PolicyNoHelper.GetEndorsementPolicyNo(currentPolicyNo), 
                        newCocNo,
                        out newId);
                    break;
                case "15":
                    var newLastName = Request.Form["newlastname"];
                    var newFirstName = Request.Form["newfirstname"];
                    var newMI = Request.Form["newmi"];
                    result = data.UpdateInsuredName(int.Parse(customerId), newFirstName, newLastName, newMI);
                    break;
                case "17":
                case "19":
                    var newAddress = Request.Form["newaddress"];
                    result = data.UpdateAddress(int.Parse(customerId), newAddress);
                    break;
                case "20":
                case "22":
                    var newMortgagee = Request.Form["newmortgagee"];
                    result = data.UpdateMortgagee(int.Parse(transactionId), newMortgagee);
                    break;
                case "21":
                    result = data.DeleteMortgagee(int.Parse(transactionId));
                    break;
                case "25":
                    var periodfrom = Request.Form["periodfrom"];
                    var periodto = Request.Form["periodto"];
                    result = data.UpdatePolicyPeriod(
                                int.Parse(transactionId),
                                ParseDate(periodfrom),
                                ParseDate(periodto));
                    break;
                case "33":
                    var carcompany = Request.Form["carcompany"];
                    var carmake = Request.Form["carmake"];
                    var engineseries = Request.Form["engineseries"];
                    result = data.UpdateVehicleDescription(
                                int.Parse(transactionId),
                                int.Parse(carcompany),
                                GetCarMake(carmake),
                                GetCarSeries(carmake),
                                engineseries);

                    break;
                default:
                    break;
            }

            Dictionary<string, string> res = new Dictionary<string, string>();
            res.Add("NewID", newId.ToString());
           

            if (result > 0)
            {
                res.Add("Status", "true");
                Render<Dictionary<string, string>>(res);
            }
            else
            {
                res.Add("Status", "false");
                Render<Dictionary<string, string>>(res);
            }
        }

        private void HandleGetEndorsementByCode()
        {
            Endorsement e = data.GetOneEndorsement(int.Parse(Request.Form["ecode"]));
            Render<Endorsement>(e);
        }

        private void HandleLoadAllEndorsement()
        {
            List<Endorsement> endorsements = data.GetAllEndorsement();
            Render<List<Endorsement>>(endorsements);
        }

        private void HandleSearchTransactions()
        {
            var creditingbranch = Request.Form["creditingbranch"];
            var parno = Request.Form["parno"];
            var policyno = Request.Form["policyno"];
            var subline = Request.Form["subline"];
            var datecreated = Request.Form["datecreated"];
            var policyperiodfrom = Request.Form["policyperiodfrom"];
            var policyperiodto = Request.Form["policyperiodto"];
            var typeofcover = Request.Form["typeofcover"];
            var mortgagee = Request.Form["mortgagee"];
            var intermediary = Request.Form["intermediary"];
            var carcompany = Request.Form["carcompany"];
            var motortype = Request.Form["motortype"];
            var chassisno = Request.Form["chassisno"];
            var engineno = Request.Form["engineno"];
            var lastname = Request.Form["lastname"];
            var firstname = Request.Form["firstname"];

            string whereClause = BuildSQLWhereClause(creditingbranch, parno, policyno, subline, datecreated, policyperiodfrom,
                policyperiodto, typeofcover, mortgagee, intermediary, carcompany, motortype, chassisno, engineno, firstname, lastname);

            IEnumerable<TransactionSearchResultDTO> ts = data.SearchTransaction(whereClause);
            Render<IEnumerable<TransactionSearchResultDTO>>(ts);
        }

        private string BuildSQLWhereClause(string creditingbranch,
            string parno, string policyno, string subline, string datecreated, string policyperiodfrom,
            string policyperiodto, string typeofcover, string mortgagee, string intermediary,
            string carcompany, string motortype, string chassisno, string engineno,
            string firstname, string lastname) {

                StringBuilder sql = new StringBuilder();
                if (creditingbranch != "0")
                {
                    sql.AppendFormat(" t.creditingBranch = {0} ", creditingbranch);
                }

                if (!string.IsNullOrEmpty(parno.Trim()))
                {
                    AddAnd(sql);
                    sql.AppendFormat(" t.parNo = '{0}' ", parno.Trim());
                }

                if (!string.IsNullOrEmpty(policyno.Trim()))
                {
                    AddAnd(sql);
                    sql.AppendFormat(" t.policyNo = '{0}' ", policyno.Trim());
                }

                if (subline != "0")
                {
                    AddAnd(sql);
                    sql.AppendFormat(" t.sublineCode = '{0}' ", subline);
                }

                if (!string.IsNullOrEmpty(datecreated))
                {
                    AddAnd(sql);
                    sql.AppendFormat(" t.dateCreated = '{0}' ", ChangeDateFormat(datecreated));
                }
				
				if (!string.IsNullOrEmpty(policyperiodfrom))
                {
                    AddAnd(sql);
                    sql.AppendFormat(" t.policyPeriodFrom = '{0}' ", policyperiodfrom);
                }

                if (!string.IsNullOrEmpty(policyperiodto))
                {
                    AddAnd(sql);
                    sql.AppendFormat(" t.policyPeriodTo = '{0}' ", policyperiodto);
                }

                if (typeofcover != "0")
                {
                    AddAnd(sql);
                    sql.AppendFormat(" tc.TypeOfCover = {0} ", typeofcover);
                }

                if (mortgagee != "0")
                {
                    AddAnd(sql);
                    sql.AppendFormat(" t.mortgage = {0} ", mortgagee);
                }

                if (intermediary != "0")
                {
                    AddAnd(sql);
                    sql.AppendFormat(" t.intermediaryCode = {0} ", intermediary);
                }

                if (carcompany != "0")
                {
                    AddAnd(sql);
                    sql.AppendFormat(" tc.CarCompany = {0} ", carcompany);
                }

                if (motortype != "0")
                {
                    AddAnd(sql);
                    sql.AppendFormat(" tc.MotorType = '{0}' ", motortype);
                }

                if (!string.IsNullOrEmpty(chassisno.Trim()))
                {
                    AddAnd(sql);
                    sql.AppendFormat(" tc.ChassisNo = '{0}' ", chassisno.Trim());
                }

                if (!string.IsNullOrEmpty(engineno.Trim()))
                {
                    AddAnd(sql);
                    sql.AppendFormat(" tc.EngineNo = '{0}' ", engineno.Trim());
                }

                if (!string.IsNullOrEmpty(firstname.Trim()))
                {
                    AddAnd(sql);
                    sql.AppendFormat(" mtc.FirstName = '{0}' ", firstname.Trim());
                }

                if (!string.IsNullOrEmpty(lastname.Trim()))
                {
                    AddAnd(sql);
                    sql.AppendFormat(" mtc.LastName = '{0}' ", lastname.Trim());
                }

                return sql.ToString();
        }

        private string ChangeDateFormat(string date)
        {
            string fromFormat = "MM/dd/yyyy";
            string toFormat = "yyyy-MM-dd";

            DateTime newDate = DateTime.ParseExact(date, fromFormat, null);

            return newDate.ToString(toFormat);
        }

        public void AddAnd(StringBuilder filter)
        {
            if (!string.IsNullOrEmpty(filter.ToString()))
            {
                filter.Append(" AND ");
            }
        }

        private void HandleLoadSearchFilters()
        {
            Dictionary<string, List<DropDownListItem>> filters = data.LoadAllSearchFilters();
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            Response.Write(serializer.Serialize(filters));
            Response.End();
        }

        private void HandleUpdateTransaction()
        {
            var transactionJson = Request.Form["transaction"];
            var carDetailsJson = Request.Form["cardetails"];
            var perilsJson = Request.Form["perils"];
            var transactionId = Request.Form["transactionid"];

            double netpremium = ChangeTypeHelper.SafeParseToDouble(Request.Form["basicpremiumnet"]);
            double grosspremium = ChangeTypeHelper.SafeParseToDouble(Request.Form["basicpremiumgross"]);
            var covertype = Request.Form["covertype"];

            //Parse the transaction from json
            Transaction transaction = JsonToEntity.ConvertJsonToTransaction(transactionJson, carDetailsJson);
            transaction.TransactionID = ChangeTypeHelper.SafeParseToInt32(transactionId);
            //Parse the perils from json
            List<TransactionPeril> perils = JsonToEntity.ConvertJsonToTransactionPerils(perilsJson);

            transaction.Perils = perils;

            cls_data_access_layer dl = new cls_data_access_layer();
            IDataReader reader = dl.GetComputationFactors();

            ComputationFactor factor = ReaderToEntity.ConvertToComputationFactor(reader);
            ComputationDetails netDetails = ComputationDetailsHelper.ComputeTransactionDetails(factor, netpremium, transaction.CarDetail.TypeOfCover);

            ComputationDetails grossDetails = ComputationDetailsHelper.ComputeTransactionDetails(factor, grosspremium, transaction.CarDetail.TypeOfCover);

            transaction.Computations.NetComputationDetails = netDetails;
            transaction.Computations.GrossComputationDetails = grossDetails;

            bool saveSuccess = dl.UpdateTransaction(transaction);

            Response.Write(saveSuccess ? "1" : "0");
            Response.End();
        }

        private void HandleGetPerilsEdit()
        {
            var tId = Request.Form["transactionid"];

        }

        private void HandleGetTransaction()
        {
            var tId = Request.Form["transactionid"];
            
            cls_data_access_layer dl = new cls_data_access_layer();
            Transaction t = dl.GetTransactionById(ChangeTypeHelper.SafeParseToInt32(tId));

            List<TransactionPeril> arrangedPerils = t.Perils;
            switch (t.CarDetail.TypeOfCover)
            {
                case 3:
                    arrangedPerils = PerilsArranger.SortWithoutCTPL(t.Perils);
                    break;
                case 2:
                    arrangedPerils = PerilsArranger.SortWithCTPL(t.Perils);
                    break;
                default:
                    break;
            }
            t.Perils = arrangedPerils;
            Render<Transaction>(t);
        }

        private void HandleGetCarDetailsOptions()
        {
            cls_data_access_layer dl = new cls_data_access_layer();
            Dictionary<string, List<DropDownListItem>> options = new Dictionary<string, List<DropDownListItem>>();

            DataTable yearsDt = dl.sp_pop_carYears();
            List<DropDownListItem> caryears = new List<DropDownListItem>();
            foreach (DataRow row in yearsDt.Rows)
            {
                DropDownListItem li = new DropDownListItem();
                li.Value = row["VALUE"].ToString();
                li.Text = row["TEXT"].ToString();
                caryears.Add(li);
            }

            options.Add("caryears", caryears);

            DataTable ccDt = dl.PopulateCarCompanies();
            List<DropDownListItem> carcompanies = new List<DropDownListItem>();
            foreach (DataRow row in ccDt.Rows)
            {
                DropDownListItem li = new DropDownListItem();
                li.Value = row["VALUE"].ToString();
                li.Text = row["TEXT"].ToString();
                carcompanies.Add(li);
            }

            options.Add("carcompanies", carcompanies);

            DataTable typesOfBodyDt = dl.sp_pop_carTypesOfBody();
            List<DropDownListItem> typesofbody = new List<DropDownListItem>();
            foreach (DataRow row in typesOfBodyDt.Rows)
            {
                DropDownListItem li = new DropDownListItem();
                li.Value = row["VALUE"].ToString();
                li.Text = row["TEXT"].ToString();
                typesofbody.Add(li);
            }

            options.Add("typesofbody", typesofbody);

            DataTable typesOfCoverDt = dl.PopulateCoverTypes();
            List<DropDownListItem> typesOfCover = new List<DropDownListItem>();
            foreach (DataRow row in typesOfCoverDt.Rows)
            {
                DropDownListItem li = new DropDownListItem();
                li.Value = row["VALUE"].ToString();
                li.Text = row["TEXT"].ToString();
                typesOfCover.Add(li);
            }

            options.Add("typesofcover", typesOfCover);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            Response.Write(serializer.Serialize(options));
            Response.End();

        }

        private void HandleGetCarYears()
        {
            cls_data_access_layer dl = new cls_data_access_layer();
            DataTable dt = dl.sp_pop_carYears();
            List<DropDownListItem> items = new List<DropDownListItem>();
            foreach (DataRow row in dt.Rows)
            {
                DropDownListItem li = new DropDownListItem();
                li.Value = row["VALUE"].ToString();
                li.Text = row["TEXT"].ToString();
                items.Add(li);
            }
            RenderDropDownListItem(items);
        }

        private void HandleGetBodyTypes()
        {
            cls_data_access_layer dl = new cls_data_access_layer();
            DataTable dt = dl.sp_pop_carTypesOfBody();
            List<DropDownListItem> items = new List<DropDownListItem>();
            foreach (DataRow row in dt.Rows)
            {
                DropDownListItem li = new DropDownListItem();
                li.Value = row["VALUE"].ToString();
                li.Text = row["TEXT"].ToString();
                items.Add(li);
            }
            RenderDropDownListItem(items);
        }

        private void HandleSaveTransaction()
        {
            var transactionJson = Request.Form["transaction"];
            var carDetailsJson = Request.Form["cardetails"];
            var perilsJson = Request.Form["perils"];

            double netpremium = ChangeTypeHelper.SafeParseToDouble(Request.Form["basicpremiumnet"]);
            double grosspremium = ChangeTypeHelper.SafeParseToDouble(Request.Form["basicpremiumgross"]);


            //Parse the transaction from json
            Transaction transaction = JsonToEntity.ConvertJsonToTransaction(transactionJson, carDetailsJson);
            //Parse the perils from json
            List<TransactionPeril> perils = JsonToEntity.ConvertJsonToTransactionPerils(perilsJson);

            transaction.Perils = perils;

            cls_data_access_layer dl = new cls_data_access_layer();
            IDataReader reader = dl.GetComputationFactors();

            ComputationFactor factor = ReaderToEntity.ConvertToComputationFactor(reader);
            ComputationDetails netDetails = ComputationDetailsHelper.ComputeTransactionDetails(factor, netpremium, transaction.CarDetail.TypeOfCover);

            ComputationDetails grossDetails = ComputationDetailsHelper.ComputeTransactionDetails(factor, grosspremium, transaction.CarDetail.TypeOfCover);

            transaction.Computations.NetComputationDetails = netDetails;
            transaction.Computations.GrossComputationDetails = grossDetails;

            bool saveSuccess = dl.SaveTransaction(transaction);

            Response.Write(saveSuccess ? "1" : "0");
            Response.End();
        }

        private void HandleComputeDetails()
        {
            double netpremium = double.Parse(Request.Form["basicpremiumnet"]);
            double grosspremium = double.Parse(Request.Form["basicpremiumgross"]);
            var covertype = Request.Form["covertype"];
            cls_data_access_layer dl = new cls_data_access_layer();
            IDataReader reader = dl.GetComputationFactors();

            TransactionComputation tc = new TransactionComputation();
            ComputationFactor factor = ReaderToEntity.ConvertToComputationFactor(reader);

            ComputationDetails netDetails = ComputationDetailsHelper.ComputeTransactionDetails(factor, netpremium, int.Parse(covertype));
            tc.NetComputationDetails = netDetails;

            ComputationDetails grossDetails = ComputationDetailsHelper.ComputeTransactionDetails(factor, grosspremium, int.Parse(covertype));
            tc.GrossComputationDetails = grossDetails;

            Render<TransactionComputation>(tc);
        }

        private void HandleGetTrariffRates()
        {
            var motortype = Request.Form["motortype"];
            var subline = Request.Form["subline"];
            var ids = Request.Form["ids"];
            DefaultTariffRateResponse response = new DefaultTariffRateResponse();
            IList<int> parameters = SerializationHelper.Deserialize<IList<int>>(ids);

            #region 194and195

            if (parameters.Count > 0)
            {
                foreach (int parameter in parameters)
                {
                    IDataReader reader = data.sp_getRatesByPerilID(parameter);
                    using (reader)
                    {
                        List<TariffRate> rates = ReaderToEntity.ConvertToListOfTariffRate(reader);
                        List<DropDownListItem> items = new List<DropDownListItem>();
                        foreach (TariffRate r in rates)
                        {
                            DropDownListItem i = new DropDownListItem();
                            i.Text = r.Limit.ToString();
                            string value = string.Empty;
                            if (subline.Trim().ToUpper() == "PC")
                            {
                                value = r.PC.ToString();
                            }
                            else
                            {
                                if (motortype.ToUpper().Trim() == "LIGHT" || motortype.ToUpper().Trim() == "MEDIUM")
                                {
                                    value = r.CVLightMedium.ToString();
                                }
                                else
                                {
                                    value = r.CVHeavy.ToString();
                                }
                            }
                            i.Value = value;
                            items.Add(i);
                        }
                        response.DropdownValues.Add(parameter.ToString(), items);
                    }
                    
                }
            }

            #endregion


            //NOTE: The value of the default tariff rate for the ctpl must be in the tariff table
            //This code is here under that assumption
            #region CTPLdefault
            double ctplDefault = data.GetCtplDefault(subline);
            response.CTPLDefault = ctplDefault.ToString();
            #endregion

            Render<DefaultTariffRateResponse>(response);
        }

        private void HandleGetPerils()
        {
            var type = Request.Form["type"];
            cls_data_access_layer dl = new cls_data_access_layer();
            IDataReader reader = dl.sp_getPerilsByTypeOfCover(int.Parse(type));
            //convert
            List<Perils> perils = ReaderToEntity.ConvertToListOfPerils(reader);
            List<Perils> arrangedPerils = perils;
            switch (type)
            {
                case "3":
                    arrangedPerils = PerilsArranger.SortWithoutCTPL(perils);
                    break;
                case "2":
                    arrangedPerils = PerilsArranger.SortWithCTPL(perils);
                    break;
                default:
                    break;
            }
            //json encode
            Render<Perils>(arrangedPerils);
        }

        private void HandleFilterExistingPolicy()
        {
            var engineNo = Request.Form["engineid"];

            if (!string.IsNullOrEmpty(engineNo))
            {
                engineNo = engineNo.Trim();
            }

            cls_data_access_layer dl = new cls_data_access_layer();
            DataTable dt = dl.GetUploadedPolicyStatusByEngineNo(engineNo);
            List<string> items = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                items.Add(row["policyStatus"].ToString());
            }
            RenderStringList(items);
        }

        private void HandleFilterEngine()
        {
            var makeId = Request.Form["makeid"];
            var companyId = Request.Form["compid"];
            var seriesId = Request.Form["seriesid"];
            cls_data_access_layer dl = new cls_data_access_layer();
            DataTable dt = dl.FilterCarEngineByCarSeries(int.Parse(makeId), int.Parse(companyId), int.Parse(seriesId));
            List<DropDownListItem> items = new List<DropDownListItem>();
            foreach (DataRow row in dt.Rows)
            {
                DropDownListItem li = new DropDownListItem();
                li.Value = row["engineSeries"].ToString().Trim().Replace(" ", "_");
                li.Text = row["engineSeries"].ToString();
                items.Add(li);
            }
            RenderDropDownListItem(items);
        }

        private void HandleFilterCarSeries()
        {
            var makeId = Request.Form["makeid"];
            var companyId = Request.Form["compid"];
            cls_data_access_layer dl = new cls_data_access_layer();
            DataTable dt = dl.FilterCarSeriesByCarMake(int.Parse(makeId), int.Parse(companyId));
            List<DropDownListItem> items = new List<DropDownListItem>();
            foreach (DataRow row in dt.Rows)
            {
                DropDownListItem li = new DropDownListItem();
                li.Value = row["seriesCode"].ToString();
                li.Text = row["seriesCode"].ToString();
                items.Add(li);
            }
            RenderDropDownListItem(items);

        }

        private void HandleFilterCarMake()
        {
            var id = Request.Form["compid"];
            cls_data_access_layer dl = new cls_data_access_layer();
            DataTable dt = dl.FilterCarMakeByCarCompany(int.Parse(id));
            List<DropDownListItem> items = new List<DropDownListItem>();
            foreach (DataRow row in dt.Rows)
            {
                DropDownListItem li = new DropDownListItem();
                li.Value = row["makeAndSeriesId"].ToString();
                li.Text = row["makeAndSeries"].ToString();
                items.Add(li);
            }
            RenderDropDownListItem(items);
        }

        private void HandleGetCarCompanies()
        {
            cls_data_access_layer dl = new cls_data_access_layer();
            DataTable dt = dl.PopulateCarCompanies();
            List<DropDownListItem> items = new List<DropDownListItem>();
            foreach (DataRow row in dt.Rows)
            {
                DropDownListItem li = new DropDownListItem();
                li.Value = row["VALUE"].ToString();
                li.Text = row["TEXT"].ToString();
                items.Add(li);
            }
            RenderDropDownListItem(items);

        }

        private void RenderDropDownListItem(List<DropDownListItem> items)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Response.Write(serializer.Serialize(items));
            Response.End();
        }

        private void RenderStringList(List<string> items)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Response.Write(serializer.Serialize(items));
            Response.End();
        }

        private void Render<T>(List<T> items)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Response.Write(serializer.Serialize(items));
            Response.End();
        }

        private void Render<T>(T item)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Response.Write(serializer.Serialize(item));
            Response.End();
        }

        private static DateTime ParseDate(string inputdate)
        {
            DateTime result;
                //NOTE: This become a bug for no apparent reason
            DateTime.TryParse(inputdate, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            return result;
        }

        private string GetCarMake(string text) {
            return text.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        }

        private int GetCarSeries(string text)
        {
            return Convert.ToInt32(text.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Last());
        }
    }
}