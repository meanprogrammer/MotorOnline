using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorOnline.Library.Entity
{
    public class Transaction
    {
        public Transaction() 
        {
            this.Computations = new TransactionComputation();
            this.CarDetail = new CarDetail();
            this.Customer = new CustomerInfo();
        }

        public int TransactionID { get; set; }
        public int UserID { get; set; }
        public int CreditingBranch { get; set; }
        public string ParNo { get; set; }
        public string PolicyNo { get; set; }
        public string GeniisysNo { get; set; }
        public DateTime DateCreated { get; set; }

        public DateTime PolicyPeriodFrom { get; set; }
        public DateTime PolicyPeriodTo { get; set; }

        public string BussinessType { get; set; }

        public string PolicyStatus { get; set; }
        public string SubLineCode { get; set; }
        public string SublineText { get; set; }
        public int MortgageCode { get; set; }
        public int IntermediaryCode { get; set; }
        public int TypeOfInsurance { get; set; }

        public bool IsPosted { get; set; }
        public bool IsPrinted { get; set; }
        public bool IsEndorsed { get; set; }

        public string Remarks { get; set; }

        public CarDetail CarDetail { get; set; }
        public List<TransactionPeril> Perils { get; set; }

        public TransactionComputation Computations { get; set; }
        public CustomerInfo Customer { get; set; }


        public int CustomerID { get; set; }

        //Additional properties for display only
        public string CreditingBranchName { get; set; }
        public string MortgageeName { get; set; }
        public string IntermediaryName { get; set; }
        public string TypeOfInsuranceName { get; set; }

        //For display purposes only
        public string DateCreatedText
        {
            get {
                return DateCreated.ToString("MMM dd, yyyy");
            }
        }

        public string PolicyPeriodFromText
        {
            get
            {
                return PolicyPeriodFrom.ToString("MM/dd/yyyy");
            }
        }

        public string PolicyPeriodToText
        {
            get
            {
                return PolicyPeriodTo.ToString("MM/dd/yyyy");
            }
        }

        public string CarMakeAndSeriesText {
            get {
                return string.Format("{0}|{1}", this.CarDetail.CarMake, this.CarDetail.CarSeries);
            }
        }

        public string CarEngineText
        {
            get {
                return this.CarDetail.EngineSeries.Trim().Replace(" ", "_");
            }
        }

        public bool IsCTPLOnly { 
            get {
                return (this.CarDetail.TypeOfCover == 1);
            } 
        }

        public bool IsComprehensiveWithCTPL
        {
            get
            {
                return (this.CarDetail.TypeOfCover == 2);
            }
        }

        public bool IsComprehensiveWithOutCTPL
        {
            get
            {
                return (this.CarDetail.TypeOfCover == 3);
            }
        }

    }
}