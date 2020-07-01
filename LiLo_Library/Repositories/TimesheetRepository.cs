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
        #endregion
    }
}
