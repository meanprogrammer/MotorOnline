using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MotorOnline.Library.Entity;
using MotorOnline.Business;

namespace MotorOnline.Web
{
    public class TransactionHelper
    {
        public static DateTime CreateValidPolicyPeriodTo(DateTime selectedDate) {
            return selectedDate.AddYears(1);
        }

        public static bool IsCurrentDateSelectionValid(DateTime selectedDate) {
            DateTime currentDate = DateTime.Now;
            DateTime lastValidDate = DateTime.Now.AddDays(6);

            return (selectedDate.Date >= currentDate.Date && selectedDate.Date <= lastValidDate.Date);
        }
    }

    public class ParNoHelper
    {
        public static string GenerateValidParNo() {
            string generatedParNo = string.Empty;
            cls_data_access_layer dal = new cls_data_access_layer();
            var lastparno = BusinessFacade.Business.MiscBusiness.GetLastParNo();
            return GenerateParNoWithFormat(lastparno);
        }

        private static string GenerateParNoWithFormat(string lastParNo)
        {
            string yearPart = DateTime.Now.Year.ToString().Remove(0, 2);
            if (!string.IsNullOrEmpty(lastParNo))
            {
                string[] parts = lastParNo.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                int idPart = int.Parse(parts[2]) + 1;
                return string.Format("MC-{0}-{1}", yearPart, idPart.ToString("D7"));
            }
            else
            {
                return string.Format("MC-{0}-0000001", yearPart);
            }
        }
    }

    public class PolicyNoHelper
    {
        public static string GenerateValidPolicyNo() {
            string generatedPolicyNo = string.Empty;
            cls_data_access_layer dal = new cls_data_access_layer();
            var lastpolicyno = BusinessFacade.Business.MiscBusiness.GetLastPolicyNo();
            return GeneratePolicyNoWithFormat(lastpolicyno);
        }

        private static string GeneratePolicyNoWithFormat(string lastPolicyNo) {
            //string yearPart = DateTime.Now.Year.ToString().Remove(0, 2);
            string yearPart = DateTime.Now.Year.ToString().Remove(0, 2);
            if (!string.IsNullOrEmpty(lastPolicyNo))
            {
                string[] parts = lastPolicyNo.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                //int idPart = int.Parse(parts[1]);
                int lastDigitPart = int.Parse(parts[1]);
                
                if (lastDigitPart == 99)
                {
                    //idPart++;
                    lastDigitPart = 0;
                }
                else
                {
                    lastDigitPart++;
                }

                return string.Format("MC-{0}-{1}-{2}-{3}", yearPart, lastDigitPart.ToString("D7"), "00", "00");
            }
            else
            {
                return string.Format("MC-{0}-0000000-00-00", yearPart);
            }
        }

        public static string GetEndorsementPolicyNo(string currentPolicyNo) {
            //MC-14-0000015-00-00
            //if (!string.IsNullOrEmpty(currentPolicyNo))
            //{
                string[] parts = currentPolicyNo.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                //TODO: Is this safe
                int last = Convert.ToInt32(parts.Last());
                last++;
                currentPolicyNo = currentPolicyNo.Remove(currentPolicyNo.Length - 3, 3);
                return string.Format("{0}-{1}", currentPolicyNo, last.ToString("D2"));
            //}
        }
    }

    public static class ComputationDetailsHelper
    {
        public static ComputationDetails ComputeTransactionDetails(ComputationFactor factors, double premium, int covertype)
        {
            ComputationDetails details = new ComputationDetails();

            details.BasicPremium = premium;

            details.DocumentaryStamps = Math.Round((premium * factors.DocumentaryStamps) / 100);
            details.ValueAddedTax = (premium * factors.ValueAddedTax) / 100;
            details.LocalGovernmentTax = ((premium * factors.LocalGovtTax) / 100) / 100;
            if (covertype == 1 || covertype == 2)
            {
                details.DSTonCOC = factors.DSTonCOC;
                details.LTOInterconnectivity = factors.LTOConnectivity;
            }

            details.GrandTotal = details.BasicPremium + details.DocumentaryStamps + details.ValueAddedTax + details.LocalGovernmentTax + details.DSTonCOC + details.LTOInterconnectivity;

            return details;
        }
    }
}