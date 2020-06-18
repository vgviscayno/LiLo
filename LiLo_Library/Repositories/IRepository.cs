using System.Collections.Generic;

namespace LiLo_Library.Repositories
{
    /// <summary>
    /// Interface for Repositories
    /// Which will containt queries to the database using model class T
    /// </summary>
    /// <typeparam name="T">Model class</typeparam>
    public interface IRepository<T> where T : class
    {
        #region CRUD operations
        List<T> GetAll();

        bool Add(T row);

        T GetById(int id);

        bool Update(T row);

        bool Delete(int id);
        #endregion
    }
}