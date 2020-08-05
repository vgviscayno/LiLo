using System;
using System.Collections.Generic;

namespace LiLo_Library.Models
{
    public class TimesheetSummaryModel
    {
        public EmployeeModel Employee { get; set; }

        public TimeSpan[] Days { get; set; }

        /// <summary>
        /// Calculates the day-to-day hours rendered of the employee's
        /// list of TimesheetModels.
        /// </summary>
        /// <param name="employee">Employee to whom the timesheetModels belong to</param>
        /// <param name="timesheetModels">List of TimesheetModels to be set to Days array</param>
        /// <param name="ForPayroll">Set to true if to be used for generating payroll</param>
        public TimesheetSummaryModel(EmployeeModel employee, List<TimesheetModel> timesheetModels, bool ForPayroll)
        {
            Employee = employee;

            int DaysInAMonth = 0;
            int index = 0;

            if (ForPayroll)
            {
                Days = new TimeSpan[timesheetModels.Count];
                DaysInAMonth = timesheetModels.Count;
            }
            else
            {
                Days = new TimeSpan[30];
                DaysInAMonth = DateTime.DaysInMonth(timesheetModels[0].CurrentDate.Year, timesheetModels[0].CurrentDate.Month);
                DaysInAMonth--;
            }

            TimeSpan hoursForTheDay = default;

            DateTime date = timesheetModels[0].CurrentDate;
            

            //iterate timesheet list by date
            for (int i = 0; i < DaysInAMonth && i < timesheetModels.Count; i++)
            {

                //if next day
                if (i > 0 && timesheetModels[i].CurrentDate != date)
                {
                    if (ForPayroll)
                    {
                        Days[index] = hoursForTheDay;
                        index++;
                    }
                    else
                    {
                        Days[date.Day - 1] = hoursForTheDay;
                    }
                    date = timesheetModels[i].CurrentDate;
                    hoursForTheDay = default;
                    hoursForTheDay += timesheetModels[i].HoursRendered;
                }
                else
                {
                    hoursForTheDay += timesheetModels[i].HoursRendered;
                }
            }

            if (ForPayroll)
            {
                Days[index] = hoursForTheDay;
            }
            else
            {
                //add last recorded day
                Days[date.Day - 1] = hoursForTheDay;
            }
        }

        /// <summary>
        /// iterates Days and check for each day the number of hours rendered
        /// 
        /// Half-day: [4-8)
        /// Whole-day: 8 and up
        /// </summary>
        public double TotalDays
        {
            get
            {
                double res = 0;
                foreach(var day in Days)
                {
                    if(day >= new TimeSpan(4, 0, 0))
                    {
                        if(day >= new TimeSpan(8, 0, 0))
                        {
                            res += 1;
                        }
                        else
                        {
                            res += 0.5;
                        }
                    }
                }
                return res;
            }
        }
    }
}