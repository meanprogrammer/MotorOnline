using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MotorOnline.Library.Entity;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using MotorOnline.Business;

namespace MotorOnline.Web
{
    public partial class EndorsementPrintOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TransactionBusiness transactionBL = new TransactionBusiness();
            var id = Request.QueryString["id"];
            cls_data_access_layer da = new cls_data_access_layer();
            EndorsementBusiness endorsementBL = new EndorsementBusiness();
            Transaction trans = transactionBL.GetTransactionById(int.Parse(id));
            EndorsementDetail ed = endorsementBL.GetEndorsementDetail(int.Parse(id));

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


            PdfPTable t = new PdfPTable(new float[] { 25f, 25f, 25f, 25f });
            t.WidthPercentage = 100;
            //Logo row
            t.AddCell(new PdfPCell(logo) { HorizontalAlignment = 0, Colspan = 4, Border = 0 });

            //Title row
            t.AddCell(PdfHelper.CreateCellWithText("ENDORSEMENT", 1, 4, fontSize: 8, fonttype: 1));
            //Empty row
            t.AddCell(PdfHelper.CreateCellWithText(" ", 1, 4));

            PdfPTable tDetails = CreateTransactionDetailsTable(trans);
            t.AddCell(new PdfPCell(tDetails) { Colspan = 4, Border = 0 });

            //Expires
            t.AddCell(PdfHelper.CreateCellWithText(
                string.Format("This Endorsement is effective as of {0}",
                ed.EffectivityDate.ToString("dd MMMM yyyy")), 0, 2, fontSize: 8, fonttype: 1, paddingBottom: 5, bottomborder: 1));

            t.AddCell(PdfHelper.CreateCellWithText(
                string.Format("Expiry Date 12:00 noon of {0}",
                ed.ExpiryDate.ToString("dd MMMM yyyy")), 0, 2, fontSize: 8, fonttype: 1, paddingBottom: 5, bottomborder: 1));

            //Endorsement text
            t.AddCell(PdfHelper.CreateCellWithText(
                    ed.EndorsementText, 0, 4, fontSize: 8, fonttype: 1, paddingBottom: 5, bottomborder: 1));




            PdfPTable innerTable = new PdfPTable(new float[] { 25f, 25f, 25f, 25f });
            //14th row - headers
            innerTable.AddCell(PdfHelper.CreateCellWithText("INSURED VEHICLE", 1, colspan: 1, topborder: 1, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText("COVERAGE", 1, topborder: 1, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText("LIMIT OF LIABILITY", 1, topborder: 1, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText("PREMIUMS", 1, topborder: 1, fonttype: 1));



            //15th row
            innerTable.AddCell(PdfHelper.CreateCellWithText(string.Format("Type of Cover: {0}", GetCoverType(trans.CarDetail.TypeOfCover)), 0, colspan: 3, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 1, fonttype: 1, bottomborder: 1));

            //16th row
            innerTable.AddCell(PdfHelper.CreateCellWithText(string.Format("Car Company: {0}", trans.CarDetail.CarCompanyText), 0, colspan: 2, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText("Total Gross Premium", 2, colspan: 1, fonttype: 1));
            innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(trans.Computations.GrossComputationDetails.GrandTotal), 1, topborder: 1, paddingRight: 20f));

            //17th row
            innerTable.AddCell(PdfHelper.CreateCellWithText(string.Format("Car Make: {0} Series {1}", trans.CarDetail.CarMakeText, trans.CarDetail.CarSeries), 0, colspan: 4, fonttype: 1));

            //18th row
            innerTable.AddCell(PdfHelper.CreateCellWithText(string.Format("Engine Model:", trans.CarDetail.EngineSeries.Replace("_", " ")), 0, fonttype: 1, colspan: 4));
            //innerTable.AddCell(PdfHelper.CreateCellWithText(trans.CarDetail.EngineSeries.Replace("_", " "), 0));

            //19th row
            innerTable.AddCell(PdfHelper.CreateCellWithText(string.Format("Plate No.: {0}", trans.CarDetail.PlateNo), 0, fonttype: 1, colspan: 4));

            //20th row
            innerTable.AddCell(PdfHelper.CreateCellWithText(string.Format("Engine No.:", trans.CarDetail.EngineNo), 0, fonttype: 1, colspan: 4));

            //21th row
            innerTable.AddCell(PdfHelper.CreateCellWithText(string.Format("Chassis No.:", trans.CarDetail.ChassisNo), 0, fonttype: 1, colspan: 4));

            //22th row
            innerTable.AddCell(PdfHelper.CreateCellWithText(string.Format("Color:", trans.CarDetail.Color), 0, fonttype: 1, colspan: 4));

            //23th row
            innerTable.AddCell(PdfHelper.CreateCellWithText(string.Format("Accessories:", trans.CarDetail.Accessories), 0, fonttype: 1, colspan: 4));

            //Authentication No
            innerTable.AddCell(PdfHelper.CreateCellWithText(string.Format("Authentication No: {0}", trans.CarDetail.AuthenticationNo), 0, fonttype: 1, colspan: 4));

            //COC No
            innerTable.AddCell(PdfHelper.CreateCellWithText(string.Format("COC No: {0}", trans.CarDetail.COCNo), 0, fonttype: 1, colspan: 4));

            //24th row
            //innerTable.AddCell(PdfHelper.CreateCellWithText(" ", 0, 3));
            //innerTable.AddCell(PdfHelper.CreateCellWithText("Total Gross Premium", 2, fonttype: 1));
            //innerTable.AddCell(PdfHelper.CreateCellWithText(FormatToPhilippneCurrency(trans.Computations.GrossComputationDetails.GrandTotal), 2, topborder: 1, paddingRight: 20f));

            t.AddCell(new PdfPCell(innerTable) { Colspan = 4, Border = 0 });

            t.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 4, bottomborder: 1));
            t.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 4));
            t.AddCell(PdfHelper.CreateCellWithText(
                "Nothing herein contained shall be held to vary, alter, waive or change any of the terms, limitations or conditions," +
                "clauses or warranties of the Policy, except as set forth above. This endorsement atteches to and forming part of" +
                "policy number indicated above.",
                0, colspan: 4));
            t.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 4));
            t.AddCell(PdfHelper.CreateCellWithText(" ", colspan: 4));

            t.AddCell(PdfHelper.CreateCellWithText("Conforme:", 0, colspan: 2));
            t.AddCell(PdfHelper.CreateCellWithText("FEDERAL PHOENIX ASSURANCE CO., INC.", 1, colspan: 2, fonttype: 2, paddingLeft: 20f));

            PdfPTable conformeTable = new PdfPTable(1);
            conformeTable.AddCell(PdfHelper.CreateCellWithText(" ", 0));
            conformeTable.AddCell(PdfHelper.CreateCellWithText(" ", 0));
            conformeTable.AddCell(PdfHelper.CreateCellWithText(" ", 0, bottomborder: 1));
            conformeTable.AddCell(PdfHelper.CreateCellWithText(
                "Important Note: kindly sign and return duplicate copy of " +
                "this endorsement to signify your conformity thereto. Should we " +
                "not receive your signed conformity within fifteen (15) days from " +
                "issuance of this endorsement it shall be presumed that you are in " +
                "conformity thereto.",
                0));

            t.AddCell(new PdfPCell(conformeTable) { Colspan = 2, Border = 0, VerticalAlignment=1 });

            //t.AddCell(PdfHelper.CreateCellWithText(" ", 0, colspan: 2));
            //t.AddCell(PdfHelper.CreateCellWithText(" ", 0, bottomborder: 1, colspan: 2));

            PdfPTable signTable = new PdfPTable(1);

            iTextSharp.text.Image sign = iTextSharp.text.Image.GetInstance(string.Format("{0}/{1}", basePath, "sign.jpg"));
            signTable.AddCell(
                new PdfPCell(sign)
                {
                    HorizontalAlignment = 1,
                    Border = 0,
                    PaddingLeft = 20f
                });

            signTable.AddCell(PdfHelper.CreateCellWithText(" ", alignment: 1));
            signTable.AddCell(PdfHelper.CreateCellWithText("RAMON YAP DIMACALI", alignment: 1, fonttype: 1));

            signTable.AddCell(PdfHelper.CreateCellWithText(" ", alignment: 1));
            signTable.AddCell(PdfHelper.CreateCellWithText("President", alignment: 1, fonttype: 1));

            t.AddCell(new PdfPCell(signTable) { Colspan = 2, Border = 0 });

            //t.AddCell(PdfHelper.CreateCellWithText(" ", 0, bottomborder: 1, colspan: 2));

            document.Add(t);
            document.Close();



            byte[] bytes = output.ToArray();

            Response.ContentType = "application/pdf";
            Response.BinaryWrite(bytes);

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

        private string GetCoverType(int type)
        {
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

        private string FormatToPhilippneCurrency(double value)
        {
            return string.Format("PHP {0:N2}", value);
        }

    }
}