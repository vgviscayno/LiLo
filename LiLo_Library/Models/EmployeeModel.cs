using LiLo_Library.Repositories;

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

        //to be implemented in DB
        public int PositionID { get; set; }
        #endregion

        #region Derived Properties
        /// <summary>
        /// Derived property from FirstName and LastName
        /// </summary>
        public string FullName
        {
            get => FirstName + " " + LastName;
        }

        public PositionModel Position
        {
            get
            {
                PositionRepository positionRepository = new PositionRepository();
                return positionRepository.GetById(this.PositionID);
            }
        }
        #endregion
    }
}