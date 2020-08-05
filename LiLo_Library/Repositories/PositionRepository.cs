using Dapper;
using LiLo_Library.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace LiLo_Library.Repositories
{
    public class PositionRepository : IRepository<PositionModel>
    {
        public bool Add(PositionModel row)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                cnn.Execute("insert into Position (PositionName, DailyRate) values (@PositionName, @DailyRate);", row);
                return true;
            }
        }

        public bool Delete(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                cnn.Execute("delete from Position where PositionID = @PositionID;", new PositionModel() { PositionID = id });
                return true;
            }
        }

        public List<PositionModel> GetAll()
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                var output = cnn.Query<PositionModel>("select * from Position;", new DynamicParameters());
                return output.ToList();
            }
        }

        public PositionModel GetById(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                return cnn.QuerySingle<PositionModel>("Select * from Position where PositionID = @PositionID;", new PositionModel() { PositionID = id });
            }
        }

        public bool Update(PositionModel row)
        {
            using (IDbConnection cnn = new SQLiteConnection(Helpers.LoadConnectionString()))
            {
                cnn.Execute("update Position set PositionName = @PositionName, DailyRate = @DailyRate where PositionID = @PositionID", row);
                return true;
            }
        }
    }
}
