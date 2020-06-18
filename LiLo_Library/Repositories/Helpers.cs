using System.Configuration;

namespace LiLo_Library.Repositories
{
    /// <summary>
    /// Class containing helper methods
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Retrieves connection string from app.config
        /// </summary>
        /// <param name="name">Name of connection string from app.config</param>
        /// <returns>Returns connection string with given name</returns>
        public static string LoadConnectionString(string name = "Default") => ConfigurationManager.ConnectionStrings[name].ToString();
    }
}
