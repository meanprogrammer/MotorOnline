using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using MotorOnline.Library.Entity;

namespace MotorOnline.Web
{
    public partial class GeneratePrintOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"];
            int result;
            int.TryParse(id, out result);
            if (result <= 0) {
                return;
            }

            cls_data_access_layer dl = new cls_data_access_layer();
            Transaction trans = dl.GetTransactionById(result);

            var document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            document.SetMargins(30, 30, 30, 30);

            var output = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, output);

            document.Open();
            document.NewPage();

            string basePath = Server.MapPath("images");

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(string.Format("{0}/{1}", basePath, "pdf_logo.jpg"));
            logo.ScaleToFit(460, 100);


            PdfPTable t = new PdfPTable(new float[] { 25f,25f,25f,25f });
            t.WidthPercentage = 100;
            //Logo row
            t.AddCell(new PdfPCell(logo) { HorizontalAlignment = 0, Colspan = 4, Border = 0 });

            //Title row
            t.AddCell(PdfHelper.CreateCellWithText("POLICY SCHEDULE", 1, 4, fontSize: 8, fonttype: 1));
            //Empty row
            t.AddCell(PdfHelper.CreateCellWithText(" ", 1, 4));

            PdfPTable tDetails = CreateTransactionDetailsTable(trans);
            t.AddCell(new PdfPCell(tDetails) { Colspan = 4, Border = 0 });

            //5th row - blank
            //t.AddCell(PdfHelper.CreateCellWithText(" ", 0, 4));

            //6th row
            t.AddCell(PdfHelper.CreateCellWithText("CTPL", 0, fonttype: 1));
            double ctplvalue = GetNewPolicyPremium(trans, 187);
            t.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(ctplvalue), 0));
            t.AddCell(PdfHelper.CreateCellWithText(" ", 0, 2));

            //7th row
            t.AddCell(PdfHelper.CreateCellWithText("OWNDAMAGE/THEFT", 0, fonttype: 1));
            double owndvalue = GetNewPolicyPremium(trans, 274);
            t.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(owndvalue), 0));
            t.AddCell(PdfHelper.CreateCellWithText(" ", 0, 2));

            //8th row
            t.AddCell(PdfHelper.CreateCellWithText("ACTS OF NATURE ", 0, fonttype: 1));
            double aonvalue = GetNewPolicyPremium(trans, 182);
            t.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency( aonvalue), 0));
            t.AddCell(PdfHelper.CreateCellWithText("GROSS PREMIUM", 0, fonttype: 1));
            t.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency( trans.Computations.GrossComputationDetails.BasicPremium), 0));
        
            //9th row
            t.AddCell(PdfHelper.CreateCellWithText("STRIKERIOT&COMOTN ", 0, fonttype: 1));
            double scvalue = GetNewPolicyPremium(trans, 191); //trans.Perils.Where(x => x.PerilID == 191).FirstOrDefault().NewPolicyPremium;
            t.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency( scvalue), 0));
            t.AddCell(PdfHelper.CreateCellWithText("DOC Stamps", 0, fonttype: 1));
            t.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency( trans.Computations.GrossComputationDetails.DocumentaryStamps), 0));

            //10th row
            t.AddCell(PdfHelper.CreateCellWithText("VTPL-BODILY INJURED", 0, fonttype: 1));
            double vtplvalue = GetNewPolicyPremium(trans, 194); //trans.Perils.Where(x => x.PerilID == 194).FirstOrDefault().NewPolicyPremium;
            t.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency( vtplvalue), 0));
            t.AddCell(PdfHelper.CreateCellWithText("VALUE ADDED TAX", 0, fonttype: 1));
            t.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(trans.Computations.GrossComputationDetails.ValueAddedTax), 0));

            //11th row
            t.AddCell(PdfHelper.CreateCellWithText("VTPL PROPERTY DAMAGE", 0, fonttype: 1));
            double vtpl2value = GetNewPolicyPremium(trans, 195); //trans.Perils.Where(x => x.PerilID == 195).FirstOrDefault().NewPolicyPremium;
            t.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(vtpl2value), 0));
            t.AddCell(PdfHelper.CreateCellWithText("LOCAL TAX", 0, fonttype: 1));
            t.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(trans.Computations.GrossComputationDetails.LocalGovernmentTax), 0));

            //12th row
            t.AddCell(PdfHelper.CreateCellWithText(" ", 0, 2));
            //t.AddCell(PdfHelper.CreateCellWithText("[ACTS OF NATURE]", 0));
            t.AddCell(PdfHelper.CreateCellWithText("TOTAL AMMOUNT DUE", 0, fonttype: 1));
            t.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(trans.Computations.GrossComputationDetails.GrandTotal), 0));

            //13th row - blank
            t.AddCell(PdfHelper.CreateCellWithText(" ", 0, 4));

            //=====

            PdfPTable innerTable = new PdfPTable(new float[] { 13f, 24f, 23f, 20f, 20f });
            //14th row - headers
            innerTable.AddCell(PdfHelper.CreateCellWithText("INSURED VEHICLE", 1, colspan: 2, topborder: 1, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText("COVERAGE", 1, topborder: 1, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText("LIMIT OF LIABILITY", 1, topborder: 1, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText("POLICY PREMIUM", 1, topborder: 1, fonttype: 1));

            

            //15th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Type of Cover:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(GetCoverType(trans.CarDetail.TypeOfCover), 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText("CTPL", 0, indent: 10f));
            double ctpllimitsivalue = GetNewLimitSI(trans, 187); //trans.Perils.Where(x => x.PerilID == 187).FirstOrDefault().NewLimitSI;
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(ctpllimitsivalue), 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(ctplvalue), 1));
            
            //16th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Car Company:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.CarCompanyText, 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText("OWNDAMAGE/THEFT", 0, indent: 10f));
            double ownlimitsivalue = GetNewLimitSI(trans, 274); //trans.Perils.Where(x => x.PerilID == 274).FirstOrDefault().NewLimitSI;
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(ownlimitsivalue), 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(owndvalue), 1));
            
            //17th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Car Make:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(string.Format("{0} Series {1}", trans.CarDetail.CarMakeText, trans.CarDetail.CarSeries), 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText("Deductible", 0, indent: 20f));
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(double.Parse("2000")), 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 1));

            //18th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Engine Model:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.EngineSeries.Replace("_", " "), 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText("Towing", 0, indent: 20f));
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(double.Parse("500")), 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 1));

            //19th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Plate No.:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.PlateNo, 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText("Authorized Repair Limit", 0, indent: 20f));
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(double.Parse("2500")), 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 1));

            

            //20th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Engine No.:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.EngineNo, 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText("ACTS OF NATURE", 0, indent: 10f));
            double aonlimitsivalue = GetNewLimitSI(trans, 182); //trans.Perils.Where(x => x.PerilID == 182).FirstOrDefault().NewLimitSI;
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(aonlimitsivalue), 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(aonvalue), 1));
            
            //21th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Chassis No.:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.ChassisNo, 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText("VTPL-BODILY INJURY", 0, indent: 10f));
            double vtpllimitsivalue = GetNewLimitSI(trans, 194); //trans.Perils.Where(x => x.PerilID == 194).FirstOrDefault().NewLimitSI;
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(vtpllimitsivalue), 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(vtplvalue), 1));

            //22th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Color:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.Color, 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText("VTPL-PROPERTY DAMAGE", 0, indent: 10f));
            double vtpl2limitsivalue = GetNewLimitSI(trans, 195); //trans.Perils.Where(x => x.PerilID == 195).FirstOrDefault().NewLimitSI;
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(vtpl2limitsivalue), 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 1));

            //23th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Accessories:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.Accessories, 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText("AUTO PA (PER SEAT)", 0, indent: 10f));
            double autopalimitsivalue = GetNewLimitSI(trans, 184); //trans.Perils.Where(x => x.PerilID == 184).FirstOrDefault().NewLimitSI;
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(autopalimitsivalue), 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 1));

            //24th row
            innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 0, 3));
            innerTable.AddCell(PdfHelper.CreateCellWithText("Total Gross Premium", 2, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(trans.Computations.GrossComputationDetails.GrandTotal), 1, topborder: 1));

            t.AddCell(new PdfPCell(innerTable) { Colspan = 4, Border = 0 });
           
            //==============

            //25th row
            t.AddCell(PdfHelper.CreateCellWithText("The following Clauses and Endorsements apply to this Policy :", 0, 4, fonttype: 1));
            //26th row
            t.AddCell(PdfHelper.CreateCellWithText("Accessories Clause", 0, 2, fonttype: 1));
            t.AddCell(PdfHelper.CreateCellWithText("Acts of Nature Endorsement", 0, 2, fonttype: 1));
            //27th row
            t.AddCell(PdfHelper.CreateCellWithText("Airbag Clause", 0, 2));
            t.AddCell(PdfHelper.CreateCellWithText("Auto PA Endorsement - PHP50,000.00", 0, 2));
            //28th row
            t.AddCell(PdfHelper.CreateCellWithText("Electronic Data Recognition Exclusion Clause", 0, 2));
            t.AddCell(PdfHelper.CreateCellWithText("Non-Dealer or Non-Casa Repair Shop clause", 0, 2));
            //29th row
            t.AddCell(PdfHelper.CreateCellWithText("Pair And Set Endorsement", 0, 2));
            t.AddCell(PdfHelper.CreateCellWithText("Strikes, Riots, Civil Commotion Endorsement", 0, 2));
            //30th row
            t.AddCell(PdfHelper.CreateCellWithText("Terrorism & Sabotage Exclusion Clause", 0, 2, bottomborder: 1));
            t.AddCell(PdfHelper.CreateCellWithText("Total Asbestos Exclusion Clause", 0, 2, bottomborder: 1));
            //31th row
            t.AddCell(PdfHelper.CreateCellWithText(" ", 0, 4));
            //32nd row
            t.AddCell(PdfHelper.CreateCellWithText("Compulsory TPL cover under Section I/II of this Policy is deemed deleted.", 0, 4));
            //33rd row
            t.AddCell(PdfHelper.CreateCellWithText(" ", 0, 4, bottomborder: 1));
            //34th row
            t.AddCell(PdfHelper.CreateCellWithText("(Subject to the terms, conditions, warranties and clauses of the Federal Phoenix Assurance Co., Inc. Motor Vehicle Policy)", 0, 4, fonttype: 1));

            if (trans.PolicyPeriodFrom < trans.DateCreated)
            {
                t.AddCell(PdfHelper.CreateCellWithText("Warranted No Loss and No Claims(s) shall be processed and paid to the prior to the Insurance of this Policy", 0, 4, fonttype: 1, bottomborder: 1, topborder: 1));
            }

            //35th row
            t.AddCell(PdfHelper.CreateCellWithText(" ", 0, 4));
            //36th row
            t.AddCell(PdfHelper.CreateCellWithText("In Witness Whereof, the company has caused this policy to be signed by its duly authorized officer in Makati City, Philippines.", 0, 4, fonttype: 1));

            //37th row
            t.AddCell(PdfHelper.CreateCellWithText("Documentary Stamps to the value stated above have been affixed to the Policy.", 0, 4));
            //38th row
            t.AddCell(PdfHelper.CreateCellWithText("It is understood that upon the issuance of the Policy, no payment for Documentary Stamp Tax will be refunded as a result of the cancellation or endorsement of the policy or a reduction in the premium due for whatever reason. ", 0, 3));
            t.AddCell(PdfHelper.CreateCellWithText(" "));
            //39th row
            t.AddCell(PdfHelper.CreateCellWithText(" ", 0, 2));
            t.AddCell(PdfHelper.CreateCellWithText("FEDERAL PHOENIX ASSURANCE CO., INC.", 0, 2, fonttype: 2));
            //40th row
            t.AddCell(PdfHelper.CreateCellWithText(" ", 0, 2));
            iTextSharp.text.Image sign = iTextSharp.text.Image.GetInstance(string.Format("{0}/{1}", basePath, "sign.jpg"));
            t.AddCell(
                new PdfPCell(sign) { 
                    Colspan = 2,
                    HorizontalAlignment = 1,
                    Border = 0
                });
            //41st 
            t.AddCell(PdfHelper.CreateCellWithText(" ", 0, 2));
            t.AddCell(PdfHelper.CreateCellWithText("RAMON YAP DIMACALI President", 1, 2, fonttype: 1));

            document.Add(t);
            document.Close();



            byte[] bytes = output.ToArray();

            Response.ContentType = "application/pdf";
            Response.BinaryWrite(bytes);
        }

        //private static double GetAONValue(Transaction trans)
        //{
        //    var v = trans.Perils.Where(x => x.PerilID == 182).FirstOrDefault();
        //    if(v == null){
        //        return 0;
        //    } else {
        //       return v.NewPolicyPremium;
        //    }
        //}

        //private static double GetOWNDValue(Transaction trans)
        //{
        //    var v = trans.Perils.Where(x => x.PerilID == 274).FirstOrDefault();
        //    if(v == null){
        //        return 0;
        //    } else {
        //        return v.NewPolicyPremium;
        //    }
        //}

        //private static double GetCtplValue(Transaction trans)
        //{
        //    var v = trans.Perils.Where(x => x.PerilID == 187).FirstOrDefault();
        //    if(v == null) {
        //        return 0;
        //    } else {
        //        return v.NewPolicyPremium;
        //    }
        //}

        private static double GetNewPolicyPremium(Transaction t, int type)
        {
            var v = t.Perils.Where(x => x.PerilID == type).FirstOrDefault();
            if (v == null)
            {
                return 0;
            }
            else
            {
                return v.NewPolicyPremium;
            }
        }

        private static double GetNewLimitSI(Transaction t, int type)
        {
            var v = t.Perils.Where(x => x.PerilID == type).FirstOrDefault();
            if (v == null)
            {
                return 0;
            }
            else
            {
                return v.NewLimitSI;
            }
        }

        private static PdfPTable CreateTransactionDetailsTable(Transaction trans)
        {
            PdfPTable tDetails = new PdfPTable(new float[] { 15f, 35f, 21f, 29f });

            //1st row
            tDetails.AddCell(PdfHelper.CreateCellWithText("Subline:", 0, fonttype: 1));

            tDetails.AddCell(PdfHelper.CreateCellWithText(trans.SublineText, 0, fonttype: 1));

            tDetails.AddCell(PdfHelper.CreateCellWithText("Policy No.: ", fonttype: 1));
            tDetails.AddCell(PdfHelper.CreateCellWithText(trans.PolicyNo, 0));

            //2nd row
            tDetails.AddCell(PdfHelper.CreateCellWithText("Insured Name:", 0, fonttype: 1));
            tDetails.AddCell(PdfHelper.CreateCellWithText(string.Format("{0},{1}", trans.Customer.LastName, trans.Customer.FirstName), 0, fonttype: 1));
            tDetails.AddCell(PdfHelper.CreateCellWithText("Date of insurance:", 0, fonttype: 1));
            tDetails.AddCell(PdfHelper.CreateCellWithText(trans.DateCreated.ToString("MMMM dd, yyyy"), 0));

            //3rd row
            tDetails.AddCell(PdfHelper.CreateCellWithText("Address:", 0, fonttype: 1));
            tDetails.AddCell(PdfHelper.CreateCellWithText(trans.Customer.Address, 0));
            tDetails.AddCell(PdfHelper.CreateCellWithText("Period of insurance From:", 0, fonttype: 1));
            tDetails.AddCell(PdfHelper.CreateCellWithText(trans.PolicyPeriodFrom.ToString("MMMM dd, yyyy"), 0));

            //4th row
            tDetails.AddCell(PdfHelper.CreateCellWithText("", 0, 2, bottomborder: 1));
            tDetails.AddCell(PdfHelper.CreateCellWithText("To:", 2, bottomborder: 1, fonttype: 1));
            tDetails.AddCell(PdfHelper.CreateCellWithText(trans.PolicyPeriodTo.ToString("MMMM dd, yyyy"), 0, bottomborder: 1));
            return tDetails;
        }

        private string FormatToPhilippneCurrency(double value) {
            return string.Format("PHP {0:N2}", value);
        }

        private string GetCoverType(int type) {
            switch (type)
            {
                case 1:
                    return "CTPL ONLY";
                case 2:
                    return "COMPREHENSIVE W/ CTPL";
                case 3:
                    return "COMPREHENSIVE W/O CTPL";
                default:
                    return "";
            }
        }
    }
}