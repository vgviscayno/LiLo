using LiLo_Library.Repositories;
using System;

namespace LiLo_Library.Models
{
    public class TimesheetModel : IEquatable<TimesheetModel>
    {
        #region Database Table Properties
        public int TimesheetID { get; set; }

        public int EmployeeID { get; set; }

        public DateTime Date { get; set; }

        public DateTime InTime { get; set; }

        public DateTime OutTime { get; set; }
        #endregion

        #region Derived Properties
        public string EmployeeName 
        {
            get
            {
                EmployeeRepository er = new EmployeeRepository();
                return er.GetById(EmployeeID).FullName;
            }
        }
        #endregion
         
        public bool Equals(TimesheetModel other)
        {
            return this.EmployeeID == other.EmployeeID;
        }
    }
}