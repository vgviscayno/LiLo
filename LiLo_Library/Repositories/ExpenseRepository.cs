using Dapper;
using LiLo_Library.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace LiLo_Library.Repositories
{
    public class ExpenseRepository : IRepository<ExpenseModel>
    {
        public bool Add(ExpenseModel row)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<ExpenseModel> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public List<ExpenseModel> GetAllDistinct()
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                var output = cnn.Query<ExpenseModel>("select distinct * from Expense;", new DynamicParameters());
                return output.ToList();
            }
        }

        public ExpenseModel GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(ExpenseModel row)
        {
            throw new System.NotImplementedException();
        }
    }
}
