using System;
using System.Diagnostics;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfDocument = PdfSharp.Pdf.PdfDocument;
using PdfReader = PdfSharp.Pdf.IO.PdfReader;


namespace RedisCore.PDF
{
    public class PdfSharper
    {
        public void MergePdfs(string[] inputs)
        {
            var outPdfDocument = new PdfDocument();

            foreach (var pdf in inputs)
            {
                var inputDocument = PdfReader.Open(pdf, PdfDocumentOpenMode.Import);
                outPdfDocument.Version = inputDocument.Version;

                foreach (var page in inputDocument.Pages)
                {
                    outPdfDocument.AddPage(page);
                }
            }

            var name = "WTF.pdf";
            outPdfDocument.Info.Creator = string.Empty;
            outPdfDocument.Save($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/lens.pdf"); 

            Process.Start(name);
        }

        public void MergePdfsIText(string[] inputs)
        {
            var outPdfDocument = new PdfDocument();

            foreach (var pdf in inputs)
            {
                var inputDocument = PdfReader.Open(pdf, PdfDocumentOpenMode.Import);
                outPdfDocument.Version = inputDocument.Version;

                foreach (var page in inputDocument.Pages)
                {
                    outPdfDocument.AddPage(page);
                }
            }

            var name = "WTF.pdf";
            outPdfDocument.Info.Creator = string.Empty;
            //outPdfDocument.Info.Producer = string.Empty;

            outPdfDocument.Save($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/lens.pdf");

            Process.Start(name);
        }

        public byte[] CreatePdf(byte inputs)
        {
            var document = new PdfDocument();
            PdfPage page = document.AddPage();
            var gtx = XGraphics.FromPdfPage(page);
            var font = new XFont("Verdana", 20, XFontStyle.Bold);

            gtx.DrawString("Hello, World!", font, XBrushes.Black,
            new XRect(0, 0, page.Width, page.Height),
                XStringFormats.Center);

            string fileName = "HelloWorld.pdf";
            document.Save(fileName);

            return new byte[1];
        }

        public bool IsPdf(byte[] inputs)
        {
            throw new NotImplementedException();
        }
    }

    public interface IPdf
    {
        byte[] MergePdfs(byte[] inputs);
        //byte[] CreatePdf(byte inputs);
        //bool IsPdf(byte[] inputs);
    }
}