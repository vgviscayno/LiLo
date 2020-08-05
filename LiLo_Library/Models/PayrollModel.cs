using System;
using System.Collections.Generic;

namespace LiLo_Library.Models
{
    //not implemented in DB yet
    public class PayrollModel : IEquatable<PayrollModel>
    {
        public int PayrollID { get; set; }

        public int EmployeeID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double DaysWorked { get; set; }

        public double GrossSalary { get; set; }

        public double NetSalary { get; set; }

        public bool Equals(PayrollModel other)
        {
            bool overlap = this.StartDate < other.EndDate && other.StartDate < this.EndDate;

            return overlap && (this.EmployeeID == other.EmployeeID);
        }
    }
}
