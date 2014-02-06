using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace MotorOnline.Web
{
    public static class PdfHelper
    {
       static BaseFont defaultBasefont = BaseFont.CreateFont(
                        BaseFont.COURIER,
                        BaseFont.CP1252,
                        BaseFont.EMBEDDED);

       static BaseFont boldBasefont = BaseFont.CreateFont(
                BaseFont.COURIER_BOLD,
                BaseFont.CP1252,
                BaseFont.EMBEDDED);

        public static PdfPCell CreateCellWithText(string text) {
            return CreateCellWithText(text, 0, 1);
        }

        public static PdfPCell CreateCellWithText(string text, int alignment = 0) {
            return CreateCellWithText
                (text, alignment, 1);
        }

        public static PdfPCell CreateCellWithText(string text, int alignment = 0, 
            int colspan = 1, int topborder = 0, int bottomborder = 0,
            int leftborder = 0, int rightborder =0, 
            int fonttype = 0, int fontSize = 7, float indent = 0f,
            float paddingTop = 0, float paddingRight = 0, float paddingBottom = 0,
            float paddingLeft = 0) {
            return new PdfPCell()
            {
                Phrase = new iTextSharp.text.Phrase(text, GetFont(fonttype, fontSize)),
                HorizontalAlignment = alignment,
                Colspan = colspan,
                BorderWidthTop = topborder,
                BorderWidthBottom = bottomborder,
                BorderWidthLeft = leftborder,
                BorderWidthRight = rightborder,
                Indent = indent,
                PaddingTop = paddingTop,
                PaddingRight = paddingRight,
                PaddingBottom = paddingBottom,
                PaddingLeft = paddingLeft
            };
        }

        private static Font GetFont(int type, int fontSize) {
            switch (type)
            {
                case 0:
                    return new Font(defaultBasefont, fontSize);
                case 1:
                    return new Font(boldBasefont, fontSize);
                case 2:
                    return new Font(defaultBasefont, fontSize, Font.BOLDITALIC);
                default:
                    return new Font(defaultBasefont, fontSize);
            }
        }
    }
}