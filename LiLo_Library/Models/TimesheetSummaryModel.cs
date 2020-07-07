using System;
using System.Collections.Generic;

namespace LiLo_Library.Models
{
    public class TimesheetSummaryModel
    {
        public EmployeeModel Employee { get; set; }

        public TimeSpan[] Days { get; set; }

        public TimesheetSummaryModel(EmployeeModel employee, List<TimesheetModel> timesheetModels)
        {
            Employee = employee;

            Days = new TimeSpan[30];

            var DaysInAMonth = DateTime.DaysInMonth(timesheetModels[0].CurrentDate.Year, timesheetModels[0].CurrentDate.Month);

            DaysInAMonth--;

            TimeSpan hoursForTheDay = default;

            DateTime date = timesheetModels[0].CurrentDate;

            //iterate timesheet list by date
            for (int i = 0; i < DaysInAMonth && i < timesheetModels.Count; i++)
            {

                //if next day
                if (i > 0 && timesheetModels[i].CurrentDate != date)
                {
                    Days[date.Day - 1] = hoursForTheDay;
                    date = timesheetModels[i].CurrentDate;
                    hoursForTheDay = default;
                }
                else
                {
                    hoursForTheDay += timesheetModels[i].HoursRendered;
                }
            }

            //add last recorded day
            Days[date.Day - 1] = hoursForTheDay;
        }
    }
}