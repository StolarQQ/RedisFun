namespace ExcelPlayGround
{
    public class Case
    {
        public int CaseId { get; set; }
        public string Type { get; set; }
        public string Info { get; set; }

        public Case(int caseId, string type, string info)
        {
            CaseId = caseId;
            Type = type;
            Info = info;
        }
    }
}