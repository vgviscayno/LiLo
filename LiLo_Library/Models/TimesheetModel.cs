using LiLo_Library.Repositories;
using System;

namespace LiLo_Library.Models
{
    public class TimesheetModel : IEquatable<TimesheetModel>
    {
        #region Database Table Properties
        public int TimesheetID { get; set; }

        public int EmployeeID { get; set; }

        public DateTime CurrentDate { get; set; }

        public DateTime InTime { get; set; }

        public DateTime OutTime { get; set; }

        public Shift CurrentShift { get; set; }
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

        public TimeSpan HoursRendered
        {
            get
            {
                var res = OutTime.Subtract(InTime);
                return ((OutTime.Subtract(InTime)) <= new TimeSpan()) ? default : OutTime.Subtract(InTime);
            }
        }
        #endregion
         
        public bool Equals(TimesheetModel other)
        {
            return this.EmployeeID == other.EmployeeID && this.CurrentShift == other.CurrentShift;
        }
    }

    public enum Shift
    {
        Morning = 1,
        Afternoon = 2,
        Overtime = 3 //To be implemented...
    }
}