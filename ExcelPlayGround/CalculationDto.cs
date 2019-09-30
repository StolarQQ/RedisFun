namespace ExcelPlayGround
{
    public class CalculationDto
    {
        public int CalculationId { get; set; }
        public double Rate { get; set; }
        public Case Case { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }

        public CalculationDto(string description, int calculationId, double rate, int userId, Case @case)
        {

            CalculationId = calculationId;
            Rate = rate;
            UserId = userId;
            Case = @case;
            Description = description;
        }

    }
}