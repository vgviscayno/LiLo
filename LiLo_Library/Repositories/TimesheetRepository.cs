using Dapper;
using LiLo_Library.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace LiLo_Library.Repositories
{
    /// <summary>
    /// Repository for Timesheet Table
    /// </summary>
    public class TimesheetRepository : IRepository<TimesheetModel>
    {
        #region CRUD operations
        public bool Add(TimesheetModel row)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                cnn.Execute("Insert into Timesheet(EmployeeID, CurrentDate, InTime, OutTime, CurrentShift) values (@EmployeeID, @CurrentDate, @InTime, @OutTime, @CurrentShift)", row);
                return true;
            }
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<TimesheetModel> GetAll()
        {
            using(IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                return cnn.Query<TimesheetModel>("select * from Timesheet", new DynamicParameters()).ToList();
            }
        }

        /// <summary>
        /// Displays day-to-day timesheet
        /// </summary>
        /// <returns></returns>
        public List<TimesheetModel> GetAllForTheDay()
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                DynamicParameters ff = new DynamicParameters();
                var g = $"{DateTime.Now:yyyy-MM-dd}%";
                ff.Add("@DateNow", g);
                return cnn.Query<TimesheetModel>("select * from Timesheet where CurrentDate like @DateNow", ff).ToList();
            }
        }

        public List<TimesheetModel> GetMonthlySummaryForEmployee(string YearAndMonth, EmployeeModel employee)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                DynamicParameters queryParameters = new DynamicParameters();
                queryParameters.Add("@DateNow", $"{YearAndMonth}%");
                queryParameters.Add("@EmployeeID", $"{employee.EmployeeID}");
                return cnn.Query<TimesheetModel>("select * from Timesheet where CurrentDate like @DateNow and EmployeeID = @EmployeeID", queryParameters).ToList();
            }
        }

        public List<TimesheetModel> GetSpecificDatesForEmployee(EmployeeModel employee, DateTime startDate, DateTime endDate)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                List<TimesheetModel> timesheets = new List<TimesheetModel>();
                //get employee's for each date
                foreach (var currentDay in Helpers.EachDay(startDate, endDate))
                {
                    DynamicParameters queryParameters = new DynamicParameters();
                    queryParameters.Add("@DateNow", $"{currentDay:yyyy-MM-dd}%");
                    queryParameters.Add("@EmployeeID", $"{employee.EmployeeID}");
                    List<TimesheetModel> timesheet = cnn.Query<TimesheetModel>("select * from Timesheet where CurrentDate like @DateNow and EmployeeID = @EmployeeID", queryParameters).ToList();
                    
                    if( timesheet.Count > 0)
                        timesheets.AddRange(timesheet);
                }

                return timesheets;
            }
        }

        public TimesheetModel GetById(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                DynamicParameters idparam = new DynamicParameters();
                idparam.Add("@id", id);
                return cnn.Query<TimesheetModel>("select * from Timesheet where TimesheetID = @id", idparam).Single();
            }
        }

        public bool Update(TimesheetModel row)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                cnn.Execute("update Timesheet set CurrentDate = @CurrentDate, InTime = @InTime, OutTime = @OutTime, CurrentShift = @CurrentShift where TimesheetID = @TimesheetID", row);
                return true;
            }
        }

        public DateTime GetEarliestDate()
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                return cnn.Query<DateTime>("SELECT Timesheet.CurrentDate from Timesheet order by date(Timesheet.CurrentDate) ASC limit 1", new DynamicParameters()).Single();
            }
        }

        public DateTime GetLatestDate()
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                return cnn.Query<DateTime>("SELECT Timesheet.CurrentDate from Timesheet order by date(Timesheet.CurrentDate) DESC limit 1", new DynamicParameters()).Single();
            }
        }
        #endregion
    }
}