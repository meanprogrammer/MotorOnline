
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorOnline
{
    public class TransactionSearchResultDTO
    {
        public int TransactionID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string ParNo { get; set; }
        public string PolicyNo { get; set; }
        public string CreditingBranch { get; set; }
        public string Subline { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime PolicyPeriodFrom { get; set; }
        public DateTime PolicyPeriodTo { get; set; }
        public string TypeOfCover { get; set; }
        public string CarCompany { get; set; }
        public string MotorType { get; set; }
        public string ChassisNo { get; set; }
        public string EngineNo { get; set; }


        public string DateCreatedText { get { return this.DateCreated.ToString("MM/dd/yyyy"); } }
        public string PolicyPeriodFromText { get { return this.PolicyPeriodFrom.ToString("MM/dd/yyyy"); } }
        public string PolicyPeriodToText { get { return this.PolicyPeriodTo.ToString("MM/dd/yyyy"); } }
    }
}