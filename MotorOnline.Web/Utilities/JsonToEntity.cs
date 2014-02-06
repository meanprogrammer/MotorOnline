using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Globalization;
using MotorOnline.Library.Entity;

namespace MotorOnline.Web
{
    public class JsonToEntity
    {
        public static Transaction ConvertJsonToTransaction(string json, string carDetailsJson)
        {
            Transaction t = new Transaction();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> result = serializer.Deserialize<Dictionary<string, string>>(json);

            t.CreditingBranch = result.GetInt32FromJson("CreditingBranch");
            t.ParNo = result.GetStringFromJson("ParNo");
            t.PolicyNo = result.GetStringFromJson("PolicyNo");
            //"GeniisysNo": $('#lblGeniisysNo').html(),
            t.GeniisysNo = result.GetStringFromJson("GeniisysNo");
            //"DateCreated": $('#lblCurrentDate').html(),
            t.DateCreated = result.GetDateFromJson("DateCreated");
            //"PolicyPeriodFrom":$('#PeriodFromTextbox').val(),
            t.PolicyPeriodFrom = result.GetDateFromJson("PolicyPeriodFrom");
            //"PolicyPeriodTo":$('#PeriodToTextbox').val(),
            t.PolicyPeriodTo = result.GetDateFromJson("PolicyPeriodTo");
            //"BussinessType":$('#ddBusinessType').val(),
            t.BussinessType = result.GetStringFromJson("BussinessType");
            //"PolicyStatus":$('#lblPolicyStatus').html(),
            t.PolicyStatus = result.GetStringFromJson("PolicyStatus");
            //"SubLineCode":$('#SublineDropdown').val(),
            t.SubLineCode = result.GetStringFromJson("SubLineCode");
            //"MortgageCode":$('#ddlMortgagee').val(),
            t.MortgageCode = result.GetInt32FromJson("MortgageCode");
            //"IntermediaryCode":$('#ddInterMediary').val(),
            t.IntermediaryCode = result.GetInt32FromJson("IntermediaryCode");
            //"TypeOfInsured":$('#TypeOfInsuranceDropdown').val(),
            t.TypeOfInsured = result.GetStringFromJson("TypeOfInsured");
            //"IsPosted":false,
            t.IsPosted = result.GetBooleanFromJson("IsPosted");
            //"IsPrinted":false,
            t.IsPrinted = result.GetBooleanFromJson("IsPrinted");
            //"IsEndoresed":false,
            t.IsEndorsed = result.GetBooleanFromJson("IsEndoresed");
            //"InternalRemarks": '',
            t.Remarks = result.GetStringFromJson("Remarks");
            //"CarCompany":$('#CarCompaniesDropdown').val(),
            //t.CarCompany = result.GetInt32FromJson("CarCompany");
            ////"CarYears":$('#YearDropdown').val(),
            //t.CarYear = result.GetInt32FromJson("CarYears");
            ////"CarSeries":$('#CarMakeDropdown').val(),
            //string carSeriesRaw = result.GetStringFromJson("CarSeries");
            //t.CarSeries = GetCarSeries(carSeriesRaw);
            //////public int CarCompanies { get; set; }
            //t.CarCompanies = 0;
            //"CarMakes": $('#CarMakeDropdown').val(),
            //string carMakeRaw = result.GetStringFromJson("CarMakes");
            //t.CarMake = GetCarMake(carMakeRaw);
            ////"CarTypeOfBodyID":$('#TypeOfBodyDropdown').val()
            //t.CarTypeOfBodyID = result.GetInt32FromJson("CarTypeOfBodyID");

            //public string Designation { get; set; }
            t.Customer.Designation = result.GetStringFromJson("Designation");
            //public string LastName { get; set; }
            t.Customer.LastName = result.GetStringFromJson("LastName");
            //public string FirstName { get; set; }
            t.Customer.FirstName = result.GetStringFromJson("FirstName");
            //public string MiddleName { get; set; }
            t.Customer.MiddleName = result.GetStringFromJson("MiddleName");
            //public string Address { get; set; }
            t.Customer.Address = result.GetStringFromJson("Address");
            //public string Telephone { get; set; }
            t.Customer.Telephone = result.GetStringFromJson("Telephone");
            //public string MobileNo { get; set; }
            t.Customer.MobileNo = result.GetStringFromJson("MobileNo");
            //public string Email { get; set; }
            t.Customer.Email = result.GetStringFromJson("Email");
            //public string MultipleCorporateName  { get; set; }
            t.Customer.MultipleCorporateName = result.GetStringFromJson("MultipleCorporateName");
            t.CustomerID = result.GetInt32FromJson("CustomerID");
            Dictionary<string, string> carDetailsDictionary = serializer.Deserialize<Dictionary<string, string>>(carDetailsJson);

            t.CarDetail = ConvertJsonToCarDetails(carDetailsDictionary);

            return t;
        }

        private static CarDetail ConvertJsonToCarDetails(Dictionary<string, string> carDetailsDictionary)
        {
            CarDetail detail = new CarDetail();

            detail.CarSeries = GetCarSeries(carDetailsDictionary.GetStringFromJson("CarMake"));
            detail.CarMake = GetCarMake(carDetailsDictionary.GetStringFromJson("CarMake"));
            detail.CarTypeOfBodyID = carDetailsDictionary.GetInt32FromJson("TypeOfBody");
            detail.TypeOfCoverText = carDetailsDictionary.GetStringFromJson("TypeOfCoverText");
            detail.TypeOfCoverValue = carDetailsDictionary.GetInt32FromJson("TypeOfCoverValue");
            detail.TypeOfCover = carDetailsDictionary.GetInt32FromJson("TypeOfCover");
            detail.CarYear = carDetailsDictionary.GetInt32FromJson("CarYear");
            detail.CarCompanyText = carDetailsDictionary.GetStringFromJson("CarCompanyText");
            detail.CarCompanyValue = carDetailsDictionary.GetInt32FromJson("CarCompanyValue");
            detail.CarCompany = carDetailsDictionary.GetInt32FromJson("CarCompany");
            detail.CarMakeText = carDetailsDictionary.GetStringFromJson("CarMakeText");
            detail.CarMakeValue = carDetailsDictionary.GetStringFromJson("CarMakeValue");
            detail.MotorType = carDetailsDictionary.GetStringFromJson("MotorType");
            detail.EngineSeriesText = carDetailsDictionary.GetStringFromJson("EngineSeriesText");
            detail.EngineSeries = carDetailsDictionary.GetStringFromJson("EngineSeries");
            detail.EngineNo = carDetailsDictionary.GetStringFromJson("Engine");
            detail.ChassisNo = carDetailsDictionary.GetStringFromJson("ChassisNo");
            detail.Color = carDetailsDictionary.GetStringFromJson("Color");
            detail.PlateNo = carDetailsDictionary.GetStringFromJson("PlateNo");
            detail.ConductionNo = carDetailsDictionary.GetStringFromJson("ConductionNo");
            detail.Accessories = carDetailsDictionary.GetStringFromJson("Accessories");
            detail.AuthenticationNo = carDetailsDictionary.GetStringFromJson("AuthenticationNo");
            detail.COCNo = carDetailsDictionary.GetStringFromJson("COCNo");
            return detail;
        }

        private static int GetCarSeries(string text)
        {
            string carSeriesCode = text.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Last();
            int result = 0;
            int.TryParse(carSeriesCode, out result);
            return result;
        }

        private static string GetCarMake(string text)
        {
            return text.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).First();
        }

        public static List<TransactionPeril> ConvertJsonToTransactionPerils(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<string, string>> rawPerils = new List<Dictionary<string, string>>();
            rawPerils = serializer.Deserialize<List<Dictionary<string, string>>>(json);
            List<TransactionPeril> transactionPerils = new List<TransactionPeril>();
            foreach (Dictionary<string, string> perilItem in rawPerils)
            {
                transactionPerils.Add(ConvertRawPerilToTransactionPeril(perilItem));
            }
            return transactionPerils;
        }

        private static TransactionPeril 
            ConvertRawPerilToTransactionPeril(Dictionary<string, string> perils)
        {
            TransactionPeril p = new TransactionPeril();
            p.NewLimitSI = perils.GetDoubleFromJson("NewLimitSI");
            p.NewRate = perils.GetDoubleFromJson("NewRate");
            p.NewPremium = perils.GetDoubleFromJson("NewPremium");
            p.NewPolicyRate = perils.GetDoubleFromJson("NewPolicyRate");
            p.NewPolicyPremium = perils.GetDoubleFromJson("NewPolicyPremium");

            string parentPerilID = perils.GetStringFromJson("Data");

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //Dictionary<string, string> parentPeril = serializer.Deserialize<Dictionary<string, string>>();

            
            //"PerilID": 274,
            p.PerilID = ChangeTypeHelper.SafeParseToInt32(parentPerilID);
                //parentPeril.GetInt32FromJson("PerilID");
            ////"LineID": 2,
            //p.LineID = parentPeril.GetInt32FromJson("LineID");
            ////"SubLineID": 0,
            //p.SubLineID = parentPeril.GetInt32FromJson("SubLineID");
            ////"PerilSName": "ODTH",
            //p.PerilSName = parentPeril.GetStringFromJson("PerilSName");
            ////"PerilName": "OWNDAMAGE/THEFT",
            //p.PerilName = parentPeril.GetStringFromJson("PerilName");
            ////"PerilLName": "Section III OWN DAMAGE/THEFT",
            //p.PerilLName = parentPeril.GetStringFromJson("PerilLName");
            ////"PerilType": "B",
            
            //p.PerilType = parentPeril.GetStringFromJson("PerilType");
            ////"IsActive": true,
            //p.IsActive = parentPeril.GetBooleanFromJson("IsActive");
            ////"PerilCode": 13,
            //p.PerilCode = parentPeril.GetInt32FromJson("PerilCode");
            ////"RI_COMM_RT": 22.5,
            //p.RI_COMM_RT = parentPeril.GetDecimalFromJson("RI_COMM_RT");
            ////"IsLimitFixed": false,
            //p.IsLimitFixed = parentPeril.GetBooleanFromJson("IsLimitFixed");
            ////"DefaultLimit": "",
            //p.DefaultLimit = parentPeril.GetStringFromJson("DefaultLimit");
            ////"RequiresLTOInterconn": false,
            //p.RequiresLTOInterconn = parentPeril.GetBooleanFromJson("RequiresLTOInterconn");
            ////"RequiresDSTonCOC": false,
            //p.RequiresDSTonCOC = parentPeril.GetBooleanFromJson("RequiresDSTonCOC");
            ////"LimitSI": 0,
            //p.LimitSI = parentPeril.GetInt32FromJson("LimitSI");
            ////"Rate": 0,
            //p.Rate = parentPeril.GetDoubleFromJson("Rate");
            ////"Premium": 0,
            //p.Premium = parentPeril.GetInt32FromJson("Premium");
            ////"PolicyRate": 0,
            //p.PolicyRate = parentPeril.GetDoubleFromJson("PolicyRate");
            ////"PolicyPremium": 0,
            //p.PolicyPremium = parentPeril.GetDecimalFromJson("PolicyPremium");
            ////"Limit": 0,
            //p.Limit = parentPeril.GetInt32FromJson("Limit");
            ////"PC": 0,
            //p.PC = parentPeril.GetInt32FromJson("PC");
            ////"CVLightMedium": 0,
            //p.CVLightMedium = parentPeril.GetInt32FromJson("CVLightMedium");
            ////"CVHeavy": 0
            //p.CVHeavy = parentPeril.GetInt32FromJson("CVHeavy");


            return p;
        }
    }

    //class TypeHelper
    //{
    //    public static int SafeParseToInt32(string text)
    //    {
    //        int result = 0;
    //        int.TryParse(text, out result);
    //        return result;
    //    }
    //}

    static class JsonHelper
    {
        public static double GetDoubleFromJson(this Dictionary<string, string> dictionary, string key)
        {
            string tempString = string.Empty;
            double result = 0;
            if (dictionary.TryGetValue(key, out tempString))
            {
                //HACK
                if (tempString.ToLower().Trim() == "tariff")
                {
                    return result;
                }
                double.TryParse(tempString.Trim(), out result);
            }

            return result;
        }

        public static int GetInt32FromJson(this Dictionary<string, string> dictionary, string key)
        {
            string tempString = string.Empty;
            int result = 0;
            if (dictionary.TryGetValue(key, out tempString))
            {
                int.TryParse(tempString.Trim(), out result);
            }

            return result;
        }

        public static decimal GetDecimalFromJson(this Dictionary<string, string> dictionary, string key)
        {
            string tempString = string.Empty;
            decimal result = 0;
            if (dictionary.TryGetValue(key, out tempString))
            {
                decimal.TryParse(tempString.Trim(), out result);
            }

            return result;
        }

        public static DateTime GetDateFromJson(this Dictionary<string, string> dictionary, string key)
        {
            string tempString = string.Empty;
            DateTime result = DateTime.Now;
            if (dictionary.TryGetValue(key, out tempString))
            {
                //NOTE: This become a bug for no apparent reason
                DateTime.TryParse(tempString, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            }

            return result;
        }

        public static string GetStringFromJson(this Dictionary<string, string> dictionary, string key)
        {
            string tempString = string.Empty;
            DateTime result = DateTime.Now;
            dictionary.TryGetValue(key, out tempString);
            return tempString.Trim();
        }

        public static bool GetBooleanFromJson(this Dictionary<string, string> dictionary, string key)
        {
            string tempString = string.Empty;
            bool result = false;
            if (dictionary.TryGetValue(key, out tempString))
            {
                bool.TryParse(tempString, out result);
            }

            return result;
        }
    }
}