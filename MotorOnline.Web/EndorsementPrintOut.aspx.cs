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

namespace MotorOnline.Web
{
    public partial class EndorsementPrintOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"];
            cls_data_access_layer da = new cls_data_access_layer();
            Transaction trans = da.GetTransactionById(int.Parse(id));
            EndorsementDetail ed = da.GetEndorsementDetail(int.Parse(id));

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

    }
}