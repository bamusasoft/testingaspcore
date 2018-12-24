using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testaspcoreavail
{
    public class PdfDocumentWriter
    {
        public PdfDocumentWriter()
        {
            //We need to explicit import Encoding Windows-1252 for iTextSharp BaseFont work as expteced.
            //Maybe this is due to that the iTextSharper not fully ported to .net core.
            //Immportant: use iTextSharp with caution because it not fully ported to .net core
            //For that heavly test the used features to avoid any breaking code like the Encdoing Windows-1252 example.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1252 = Encoding.GetEncoding(1252);
        }

        public string WriteArabicIdentification(string title, string bodyText, string src, string dest, string regularFontPath, string boldFontPath)
        {
            using (var fs = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                PdfReader reader = new PdfReader(src);
                Rectangle size = reader.GetPageSizeWithRotation(1);
                Document document = new Document(size);


                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                //write the content of template into the new Pdf
                PdfContentByte canvas = writer.DirectContent;
                PdfImportedPage page = writer.GetImportedPage(reader, 1);
                canvas.AddTemplate(page, 1f, 0, 0, 1, 0, 0);

                //
                Font arabicBold = CreateBoldFont(boldFontPath);
                Font arabicRegular = CreateRegularFont(regularFontPath);

                //This is mandatory Paragraph for "SpacingBefore" property of table take effect.spacing iTextSharp, otherwise SpacingBefore will be ignored.
                Paragraph emptyParag = new Paragraph(" ");

                PdfPTable table = CreateTable();
                //Title
                PdfPCell titleCell = CreateTitle(title, arabicBold);
                //Body Text
                PdfPCell bodyCell = CreateBodyText(bodyText, arabicRegular);

                //Add the tow paragraph
                table.AddCell(titleCell);
                table.AddCell(bodyCell);

                document.Add(emptyParag);
                document.Add(table);

                //Clean resources
                document.Close();
                writer.Close();
                reader.Close();
            }
            return dest;
        }
        private Font CreateRegularFont(string path)
        {
            BaseFont reqularFont = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            return new Font(reqularFont, 18);
        }
        private Font CreateBoldFont(string path)
        {
            BaseFont boldFont = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            return new Font(boldFont, 20);
        }
        private PdfPTable CreateTable()
        {
            PdfPTable table = new PdfPTable(1);
            table.DefaultCell.NoWrap = false;
            table.SpacingBefore = 150f;
            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            return table;
        }
        private PdfPCell CreateTitle(string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.NoWrap = false;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            return cell;
        }
        private PdfPCell CreateBodyText(string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.NoWrap = false;
            cell.Border = 0;
            cell.SetLeading(4f, 2f);
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            return cell;
        }

    }
}
