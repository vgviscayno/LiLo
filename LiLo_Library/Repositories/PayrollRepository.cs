using LiLo_Library.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Linq;
using System.Net.Http;
using System.Diagnostics;
using System;

namespace LiLo_Library.Repositories
{
    public class PayrollRepository : IRepository<PayrollModel>
    {
        public bool Add(PayrollModel row)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                throw new System.NotImplementedException();
            }
        }

        public bool AddPayroll(PayrollModel payroll, List<ExpenseModel> expenses, List<IncomeModel> incomeList)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                cnn.Open();
                using (var transaction = cnn.BeginTransaction())
                {
                    try
                    {
                        string insertPayrollQuery = @"insert into Payroll(EmployeeID, StartDate, EndDate, DaysWorked, GrossSalary, Bonus, NetSalary) values(@EmployeeID, @StartDate, @EndDate, @DaysWorked, @GrossSalary, @Bonus, @NetSalary);";

                        string selectLastInsertQuery = @"SELECT last_insert_rowid();";

                        string sqlQuery = insertPayrollQuery + selectLastInsertQuery;

                        var lastInsertedId = cnn.Query<int>(sqlQuery, payroll, transaction);

                        if(expenses.Count > 0)
                        {
                            foreach(var expense in expenses)
                            {
                                expense.PayrollID = lastInsertedId.First();
                                
                                string insertExpenseQuery = @"insert into Expense(PayrollID, Name, Remarks, Amount) values (@PayrollID, @Name, @Remarks, @Amount)";
                                
                                cnn.Execute(insertExpenseQuery,expense);
                            }
                        }

                        if (incomeList.Count > 0)
                        {
                            foreach (var income in incomeList)
                            {
                                income.PayrollID = lastInsertedId.First();

                                string insertExpenseQuery = @"insert into Income(PayrollID, Name, Remarks, Amount) values (@PayrollID, @Name, @Remarks, @Amount)";

                                cnn.Execute(insertExpenseQuery, income);
                            }
                        }

                        transaction.Commit();
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        transaction.Rollback();
                    }
                }
            }
            return true;
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<PayrollModel> GetAll()
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                var output = cnn.Query<PayrollModel>("select * from Payroll;", new DynamicParameters());
                return output.ToList();
            }
        }

        public PayrollModel GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(PayrollModel row)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(PayrollModel row)
        {
            foreach(var payroll in this.GetAll())
                if (row.Equals(payroll))
                    return true;
            return false;
        }
    }
}
