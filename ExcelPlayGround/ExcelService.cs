using System;
using System.Diagnostics;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelPlayGround
{
    public class ExcelService : IExcelService
    {
        public void CreateXlsFile()
        {

            var userCase = new Case(1, "Osobowe", "Leasing osobowy");
            var calc = new CalculationDto("Pretty huge calculation from CEO", 1, 8.00, 1, userCase);

            // Excel 97-2000 file
            var wb = new HSSFWorkbook();

            // 
            var calculationSheet = wb.CreateSheet("Kalkulacja");
            var scheduleSheet = wb.CreateSheet("Harmonogram");


            // 
            calculationSheet.CreateRow(6).CreateCell(4).SetCellValue("Kalkulacja");
            calculationSheet.GetRow(6).CreateCell(5).SetCellValue("Rating");
            calculationSheet.GetRow(6).CreateCell(6).SetCellValue("Case-ID");
            calculationSheet.GetRow(6).CreateCell(7).SetCellValue("Description");

            calculationSheet.CreateRow(7).CreateCell(4).SetCellValue(calc.CalculationId);
            calculationSheet.GetRow(7).CreateCell(5).SetCellValue(calc.Rate);
            calculationSheet.GetRow(7).CreateCell(6).SetCellValue(calc.Case.CaseId);
            calculationSheet.GetRow(7).CreateCell(7).SetCellValue(calc.Description);

            // Forumula 
            calculationSheet.GetRow(6).CreateCell(8).CellFormula = "SUM(A2:C2)";

            // Change font

            //set column widths, the width is measured in units of 1 / 256th of a character width
            calculationSheet.SetColumnWidth(0, 256 * 6);
            calculationSheet.SetColumnWidth(1, 256 * 33);
            calculationSheet.SetColumnWidth(7, 256 * 40);

            //sheet.DisplayGridlines = (false);
            calculationSheet.IsPrintGridlines = false;
            calculationSheet.FitToPage = true;
            calculationSheet.HorizontallyCenter = true;

            var printSetup = calculationSheet.PrintSetup;
            printSetup.Landscape = true;


            calculationSheet.Autobreaks = true;
            printSetup.FitHeight = 1;
            printSetup.FitWidth = 1;

            //IRow headerRow = calculationSheet.CreateRow(0);
            //headerRow.HeightInPoints = 12.75f;

            // Write excel file
            SaveWorkbookToFile("businessplan.xls", wb);
            EditExistXlsFromFile("businessplan.xls");
            OveerideExistDataInXlsFile("businessplan.xls");


            // Exit helper
            ProcessHelper("businessplan.xls", "EXCEL");
        }

        /// <summary>
        /// Open particular file, next after readKey just kill process that open that file.
        /// </summary>
        private void ProcessHelper(string fileName, string processName)
        {
            Process.Start(fileName);
            Console.ReadKey();
            var proc = Process.GetProcessesByName(processName);
            proc[0].Kill();
        }

        public void EditExistXlsFromHSSFWorkbookDocument(HSSFWorkbook xlsDocument)
        {
            throw new NotImplementedException();
        }

        public void EditExistXlsFromFile(string filePath)
        {
            HSSFWorkbook xlsDocument;

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            xlsDocument = new HSSFWorkbook(stream);
            
            var randomSheet = xlsDocument.CreateSheet("Arkusz-1");
            randomSheet.CreateRow(1).CreateCell(1).SetCellValue("Test123");
            
            SaveWorkbookToFile("businessplan.xls", xlsDocument);

        }

        public void OveerideExistDataInXlsFile(string filePath)
        {
            HSSFWorkbook xlsDocument;

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            xlsDocument = new HSSFWorkbook(stream);

            var calcSheet = xlsDocument.GetSheetAt(0);

            calcSheet.GetRow(7).GetCell(4).SetCellValue("XD1");
            calcSheet.GetRow(7).GetCell(5).SetCellValue("XD2");
            calcSheet.GetRow(7).GetCell(6).SetCellValue("XD3");
            calcSheet.GetRow(7).GetCell(7).SetCellValue("XD4");

            SaveWorkbookToFile("businessplan.xls", xlsDocument);

        }

        public void RunMacro()
        {
            // Simply run macro via C#
            var app = new Excel.Application();
            var interpol = app.Workbooks.Open("businessplan.xls");
            app.Run("Macro", "arg1", "arg2");
        }

        public void EditSettings(HSSFWorkbook xlsWorkbook)
        {
            xlsWorkbook = new HSSFWorkbook();
            var sheet1 = xlsWorkbook.CreateSheet("Sheet1");
            sheet1.SetMargin(MarginType.RightMargin, 0.5);
            sheet1.SetMargin(MarginType.TopMargin, 0.6);
            sheet1.SetMargin(MarginType.LeftMargin, 0.4);
            sheet1.SetMargin(MarginType.BottomMargin, 0.3);
        }

        private void SaveWorkbookToFile(string filePath, HSSFWorkbook xlsWorkbook)
        {
            var file = filePath;
            using (var out1 = new FileStream(file, FileMode.Create))
            {
                xlsWorkbook.Write(out1);
                out1.Close();
            }
        }

        public void Experiment()
        {
            //Test commit
        }
    }
}
