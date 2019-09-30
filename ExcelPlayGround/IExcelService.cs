using NPOI.HSSF.UserModel;

namespace ExcelPlayGround
{
    public interface IExcelService
    {
        void CreateXlsFile();
        void EditExistXlsFromFile(string filePath);
        void EditExistXlsFromHSSFWorkbookDocument(HSSFWorkbook xlsDocument);
        void Experiment();
        void RunMacro();
        void EditSettings(HSSFWorkbook xlsWorkbook);
    }
}