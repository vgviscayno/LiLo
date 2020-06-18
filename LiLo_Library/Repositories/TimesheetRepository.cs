using Dapper;
using LiLo_Library.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
                cnn.Execute("Insert into Timesheet(EmployeeID, Date, InTime, OutTime) values (@EmployeeID, @Date, @InTime, @OutTime)", row);
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
                return cnn.Query<TimesheetModel>("select * from Timesheet where Date like @DateNow", ff).ToList();
            }
        }

        public TimesheetModel GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(TimesheetModel row)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                cnn.Execute("update Timesheet set Date = @Date, InTime = @InTime, OutTime = @OutTime where TimesheetID = @TimesheetID", row);
                return true;
            }
        }
        #endregion
    }
}
