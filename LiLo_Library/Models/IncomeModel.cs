using System;

namespace LiLo_Library.Models
{
    public class IncomeModel : IEquatable<IncomeModel>
    {
        public int IncomeID { get; set; }

        public int PayrollID { get; set; }

        public string Name { get; set; }

        public string Remarks { get; set; }

        public double Amount { get; set; }

        public bool Equals(IncomeModel other)
        {
            return (this.Name.Equals(other.Name)) && (this.Amount == other.Amount) && (this.Remarks.Equals(other.Remarks));
        }

        public bool IsNull
        {
            get
            {
                return this.Name == null || this.Remarks == null || this.Amount <= 0;
            }
        }
    }
}
