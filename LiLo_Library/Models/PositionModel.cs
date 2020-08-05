namespace LiLo_Library.Models
{
    public class PositionModel //not yet implemented in DB
    {
        public int PositionID { get; set; }
        
        public string PositionName { get; set; }

        public double DailyRate { get; set; }

        public double PositionalAllowance { get; set; }
    }
}
