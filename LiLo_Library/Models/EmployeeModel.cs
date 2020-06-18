namespace LiLo_Library.Models
{
    /// <summary>
    /// Model class for Employee Table
    /// </summary>
    public class EmployeeModel
    {
        #region Database Table Properties
        public int EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        #endregion

        #region Derived Properties
        /// <summary>
        /// Derived property from FirstName and LastName
        /// </summary>
        public string FullName
        {
            get => FirstName + " " + LastName;
        }
        #endregion
    }
}