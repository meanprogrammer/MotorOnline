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
using MotorOnline.Business;

namespace MotorOnline.Web
{
    public partial class GeneratePrintOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TransactionBusiness transactionBL = new TransactionBusiness();
            var id = Request.QueryString["id"];
            int result;
            int.TryParse(id, out result);
            if (result <= 0) {
                return;
            }

            cls_data_access_layer dl = new cls_data_access_layer();
            Transaction trans = transactionBL.GetTransactionById(result);

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

            PdfPTable firstComputation = new PdfPTable(new float[] { 20f, 20f, 20f,15f,25f });
            double ctplvalue = 0;
            if (trans.IsCTPLOnly || trans.IsComprehensiveWithCTPL)
            {
                //6th row
                firstComputation.AddCell(PdfHelper.CreateCellWithText("CTPL", 0, fonttype: 1));
                ctplvalue = GetNewPolicyPremium(trans, 187);
                firstComputation.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(ctplvalue), 0));
                firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", alignment: 0, colspan: 3));
            }

            //7th row
            double owndvalue = 0;
            if (trans.IsComprehensiveWithCTPL || trans.IsComprehensiveWithOutCTPL)
            {
                firstComputation.AddCell(PdfHelper.CreateCellWithText("OWNDAMAGE/THEFT", 0, fonttype: 1));
                owndvalue = GetNewPolicyPremium(trans, 274);
                firstComputation.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(owndvalue), 0));
                firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 3));
            }

            //8th row
            double aonvalue = 0;
            if (trans.IsComprehensiveWithOutCTPL || trans.IsComprehensiveWithCTPL)
            {
                firstComputation.AddCell(PdfHelper.CreateCellWithText("ACTS OF NATURE ", 0, fonttype: 1));
                aonvalue = GetNewPolicyPremium(trans, 182);
                firstComputation.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(aonvalue), 0));
            }
            else
            {
                firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1));
                firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1));
            }

            firstComputation.AddCell(PdfHelper.CreateCellWithText("GROSS PREMIUM", alignment: 0, fonttype: 1));
            firstComputation.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(trans.Computations.GrossComputationDetails.BasicPremium), 2, colspan: 1));
            firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1));

            //9th row
            double scvalue = 0;
            if (trans.IsComprehensiveWithOutCTPL || trans.IsComprehensiveWithCTPL)
            {
                firstComputation.AddCell(PdfHelper.CreateCellWithText("STRIKERIOT&COMOTN ", 0, fonttype: 1));
                scvalue = GetNewPolicyPremium(trans, 191); //trans.Perils.Where(x => x.PerilID == 191).FirstOrDefault().NewPolicyPremium;
                firstComputation.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(scvalue), 0));
            }
            else
            {
                firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1));
                firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1));
            }

            firstComputation.AddCell(PdfHelper.CreateCellWithText("DOC Stamps", 0, fonttype: 1));
            firstComputation.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(trans.Computations.GrossComputationDetails.DocumentaryStamps), 2));
            firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1));

            //10th row
            double vtplvalue = 0;
            if (trans.IsComprehensiveWithOutCTPL || trans.IsComprehensiveWithCTPL)
            {
                firstComputation.AddCell(PdfHelper.CreateCellWithText("VTPL-BODILY INJURED", 0, fonttype: 1));
                vtplvalue = GetNewPolicyPremium(trans, 194); //trans.Perils.Where(x => x.PerilID == 194).FirstOrDefault().NewPolicyPremium;
                firstComputation.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(vtplvalue), 0));
            }
            else
            {
                firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1));
                firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1));
            }

            firstComputation.AddCell(PdfHelper.CreateCellWithText("VALUE ADDED TAX", 0, fonttype: 1));
            firstComputation.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(trans.Computations.GrossComputationDetails.ValueAddedTax), 2));
            firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1));

            //11th row
            double vtpl2value = 0;
            if (trans.IsComprehensiveWithOutCTPL || trans.IsComprehensiveWithCTPL)
            {
                firstComputation.AddCell(PdfHelper.CreateCellWithText("VTPL PROPERTY DAMAGE", 0, fonttype: 1));
                vtpl2value = GetNewPolicyPremium(trans, 195); //trans.Perils.Where(x => x.PerilID == 195).FirstOrDefault().NewPolicyPremium;
                firstComputation.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(vtpl2value), 0));
            }
            else
            {
                firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1));
                firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1));
            }
            firstComputation.AddCell(PdfHelper.CreateCellWithText("LOCAL TAX", 0, fonttype: 1));
            firstComputation.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(trans.Computations.GrossComputationDetails.LocalGovernmentTax), 2));
            firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1));

            //12th row
            firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, 2));
            //t.AddCell(PdfHelper.CreateCellWithText("[ACTS OF NATURE]", 0));
            firstComputation.AddCell(PdfHelper.CreateCellWithText("TOTAL AMMOUNT DUE", 0, fonttype: 1));
            firstComputation.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(trans.Computations.GrossComputationDetails.GrandTotal), 2));
            firstComputation.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1));

            t.AddCell(new PdfPCell(firstComputation) { Colspan = 4, Border = 0 });

            

            //13th row - blank
            t.AddCell(PdfHelper.CreateCellWithText(" ", 0, 4));

            //=====

            PdfPTable innerTable = new PdfPTable(new float[] { 14f, 23f, 23f, 20f, 20f });
            //14th row - headers
            innerTable.AddCell(PdfHelper.CreateCellWithText("INSURED VEHICLE", 1, colspan: 2, topborder: 1, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText("COVERAGE", 1, topborder: 1, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText("LIMIT OF LIABILITY", 1, topborder: 1, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText("POLICY PREMIUM", 1, topborder: 1, fonttype: 1));

            

            //15th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Type of Cover:", 0, fonttype: 1));
            //TODO: Add a lookup
            innerTable.AddCell(PdfHelper.CreateCellWithText(GetCoverType(trans.CarDetail.TypeOfCover), 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText("CTPL", 0, indent: 10f));
            double ctpllimitsivalue = GetNewLimitSI(trans, 187); //trans.Perils.Where(x => x.PerilID == 187).FirstOrDefault().NewLimitSI;
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(ctpllimitsivalue), 2, paddingRight: 20f));
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(ctplvalue), 2, paddingRight: 20f));
            
            //16th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Car Company:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.CarCompanyText, 0));
            double ownlimitsivalue = 0;
            if (trans.IsComprehensiveWithCTPL || trans.IsComprehensiveWithOutCTPL)
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText("OWNDAMAGE/THEFT", 0, indent: 10f));
                ownlimitsivalue = GetNewLimitSI(trans, 274); //trans.Perils.Where(x => x.PerilID == 274).FirstOrDefault().NewLimitSI;
                innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(ownlimitsivalue), 2, paddingRight: 20f));
                innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(owndvalue), 2, paddingRight: 20f));
            }
            else
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 3));
            }
            
            //17th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Car Make:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(string.Format("{0} Series {1}", trans.CarDetail.CarMakeText, trans.CarDetail.CarSeries), 0));
            if (trans.IsComprehensiveWithCTPL || trans.IsComprehensiveWithOutCTPL)
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText("Deductible", 0, indent: 20f));
                innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(double.Parse("2000")), 2, paddingRight: 20f));
                innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 1));
            }
            else
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 3));
            }

            //18th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Engine Model:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.EngineSeries.Replace("_", " "), 0));
            if (trans.IsComprehensiveWithCTPL || trans.IsComprehensiveWithOutCTPL)
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText("Towing", 0, indent: 20f));
                innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(double.Parse("500")), 2, paddingRight: 20f));
                innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 1));
            }
            else
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 3));
            }

            //19th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Plate No.:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.PlateNo, 0));
            if (trans.IsComprehensiveWithCTPL || trans.IsComprehensiveWithOutCTPL)
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText("Authorized Repair Limit", 0, indent: 20f));
                innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(double.Parse("2500")), 2, paddingRight: 20f));
                innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 1));
            }
            else
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 3));
            }

            //20th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Engine No.:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.EngineNo, 0));
            double aonlimitsivalue = 0;
            if (trans.IsComprehensiveWithCTPL || trans.IsComprehensiveWithOutCTPL)
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText("ACTS OF NATURE", 0, indent: 10f));
                aonlimitsivalue = GetNewLimitSI(trans, 182); //trans.Perils.Where(x => x.PerilID == 182).FirstOrDefault().NewLimitSI;
                innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(aonlimitsivalue), 2, paddingRight: 20f));
                innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(aonvalue), 2, paddingRight: 20f));
            }
            else
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 3));
            }

            //21th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Chassis No.:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.ChassisNo, 0));
            double vtpllimitsivalue = 0;
            if (trans.IsComprehensiveWithCTPL || trans.IsComprehensiveWithOutCTPL)
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText("VTPL-BODILY INJURY", 0, indent: 10f));
                vtpllimitsivalue = GetNewLimitSI(trans, 194); //trans.Perils.Where(x => x.PerilID == 194).FirstOrDefault().NewLimitSI;
                innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(vtpllimitsivalue), 2, paddingRight: 20f));
                innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(vtplvalue), 2, paddingRight: 20f));
            }
            else
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 3));
            }

            //22th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Color:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.Color, 0));
            double vtpl2limitsivalue = 0;
            if (trans.IsComprehensiveWithCTPL || trans.IsComprehensiveWithOutCTPL)
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText("VTPL-PROPERTY DAMAGE", 0, indent: 10f));
                vtpl2limitsivalue = GetNewLimitSI(trans, 195); //trans.Perils.Where(x => x.PerilID == 195).FirstOrDefault().NewLimitSI;
                innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(vtpl2limitsivalue), 2, paddingRight: 20f));
                innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 1));
            }
            else
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 3));
            }

            //23th row
            innerTable.AddCell(PdfHelper.CreateCellWithText("Accessories:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.Accessories, 0));
            double autopalimitsivalue = 0;
            if (trans.IsComprehensiveWithCTPL || trans.IsComprehensiveWithOutCTPL)
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText("AUTO PA (PER SEAT)", 0, indent: 10f));
                autopalimitsivalue = GetNewLimitSI(trans, 184); //trans.Perils.Where(x => x.PerilID == 184).FirstOrDefault().NewLimitSI;
                innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(autopalimitsivalue), 2, paddingRight: 20f));
                innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 1));
            }
            else
            {
                innerTable.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 3));
            }

            //Authentication No
            innerTable.AddCell(PdfHelper.CreateCellWithText("Authentication No:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.AuthenticationNo, 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 3));

            //COC No
            innerTable.AddCell(PdfHelper.CreateCellWithText("COC No:", 0, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.COCNo, 0));
            innerTable.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 3));

            //24th row
            innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 0, 3));
            innerTable.AddCell(PdfHelper.CreateCellWithText("Total Gross Premium", 2, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(trans.Computations.GrossComputationDetails.GrandTotal), 2, topborder: 1, paddingRight: 20f));

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
            t.AddCell(PdfHelper.CreateCellWithText("FEDERAL PHOENIX ASSURANCE CO., INC.", 1, 2, fonttype: 2));
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
            t.AddCell(PdfHelper.CreateCellWithText("RAMON YAP DIMACALI", 1, 2, fonttype: 1));

            t.AddCell(PdfHelper.CreateCellWithText(" ", 0, 2));
            t.AddCell(PdfHelper.CreateCellWithText("President", 1, 2, fonttype: 1));

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
            PdfPTable tDetails = new PdfPTable(new float[] { 17f, 33f, 21f, 29f });

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
            tDetails.AddCell(PdfHelper.CreateCellWithText("Multiname/Corporate:", 0, bottomborder: 1, fonttype: 1, paddingBottom: 5f));
            tDetails.AddCell(PdfHelper.CreateCellWithText(trans.Customer.MultipleCorporateName, 0, bottomborder: 1, paddingBottom: 5f));
            tDetails.AddCell(PdfHelper.CreateCellWithText("To:", 2, bottomborder: 1, fonttype: 1, paddingBottom: 5f, paddingRight: 7f));
            tDetails.AddCell(PdfHelper.CreateCellWithText(trans.PolicyPeriodTo.ToString("MMMM dd, yyyy"), 0, bottomborder: 1, paddingBottom: 5f));
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