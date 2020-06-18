using Dapper;
using LiLo_Library.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace LiLo_Library.Repositories
{
    /// <summary>
    /// Repository for queries executed on Employee Table
    /// </summary>
    public class EmployeeRepository : IRepository<EmployeeModel>
    {
        #region CRUD operations
        /// <summary>
        /// Insert query
        /// </summary>
        /// <param name="row">Row to be inserted</param>
        /// <returns>True if query is succesful</returns>
        public bool Add(EmployeeModel row)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                cnn.Execute("insert into Employee (FirstName, LastName) values (@FirstName, @LastName);", row);
                return true;
            }
        }

        /// <summary>
        /// Delete querty
        /// </summary>
        /// <param name="id">id of row to be deleted</param>
        /// <returns>True if query is succesful</returns>
        public bool Delete(int id)
        {
            using(IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                cnn.Execute("delete from Employee where EmployeeID = @EmployeeID;", new EmployeeModel() { EmployeeID = id });
                return true;
            }
        }

        /// <summary>
        /// Get all rows in table
        /// </summary>
        /// <returns>all rows in a list</returns>
        public List<EmployeeModel> GetAll()
        {
            using(IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                var output = cnn.Query<EmployeeModel>("select * from Employee;", new DynamicParameters());
                return output.ToList();
            }
        }

        /// <summary>
        /// Select by Id query
        /// </summary>
        /// <param name="id">Id of the row</param>
        /// <returns>True if query is succesful</returns>
        public EmployeeModel GetById(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                return cnn.QuerySingle<EmployeeModel>("Select * from Employee where EmployeeID = @EmployeeID;", new EmployeeModel() { EmployeeID = id });
            }
        }

        /// <summary>
        /// Update query
        /// </summary>
        /// <param name="row">row to be updated</param>
        /// <returns></returns>
        public bool Update(EmployeeModel row)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                cnn.Execute("update Employee set FirstName = @FirstName, LastName = @LastName where EmployeeID = @EmployeeID", row);
                return true;
            }
        }
        #endregion
    }
}
