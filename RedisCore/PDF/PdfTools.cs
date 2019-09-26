using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfDocument = PdfSharp.Pdf.PdfDocument;
using PdfReader = PdfSharp.Pdf.IO.PdfReader;


namespace RedisCore.PDF
{
    public class PdfTools : IPdf
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

        public bool IsPdf(string path)
        {
            var pdfString = "%PDF-";
            var pdfBytes = Encoding.ASCII.GetBytes(pdfString);
            var len = pdfBytes.Length;
            var buf = new byte[len];
            var remaining = len;
            var pos = 0;

            using (var f = File.OpenRead(path))
            {
                while (remaining > 0)
                {
                    var amtRead = f.Read(buf, pos, remaining);
                    if (amtRead == 0) return false;
                    remaining -= amtRead;
                    pos += amtRead;
                }
            }
            return pdfBytes.SequenceEqual(buf);
        }
    }

    public interface IPdf
    {
        void MergePdfs(string[] inputs);
        byte[] CreatePdf(byte inputs);
        bool IsPdf(string path);
    }
}