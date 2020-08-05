using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Used for looping from start date to end date
        /// </summary>
        /// <param name="start">Start date</param>
        /// <param name="end">End date</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> EachDay(DateTime start, DateTime end)
        {
            for (var day = start.Date; day.Date <= end.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
