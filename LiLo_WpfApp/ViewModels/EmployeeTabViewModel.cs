using LiLo_Library.Models;
using LiLo_Library.Repositories;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LiLo_WpfApp.ViewModels
{
    /// <summary>
    /// ViewModel for EmployeeTabView
    /// </summary>
    public class EmployeeTabViewModel : BaseViewModel
    {
        #region Public Properties
        public ObservableCollection<EmployeeModel> EmployeeList { get; set; }

        public EmployeeModel SelectedEmployee { get; set; }

        public string FirstNameInput { get; set; }

        public string LastNameInput { get; set; }
        #endregion

        #region Commands
        public ICommand InsertCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand UpdateCommand { get; set; }
        #endregion

        #region CRUD Functions
        /// <summary>
        /// Insert Employee
        /// </summary>
        /// <param name="obj"></param>
        public void InsertEmployee(object obj)
        {
            //Check for blank fields
            if (string.IsNullOrEmpty(FirstNameInput) || string.IsNullOrEmpty(LastNameInput))
            {
                MessageBox.Show($"{nameof(FirstNameInput)} or {nameof(LastNameInput)} is empty.", "Add Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Perform query
            if(_employeeRepository.Add(new EmployeeModel()
            {
                FirstName = FirstNameInput,
                LastName = LastNameInput
            }))
                MessageBox.Show($"[{FirstNameInput} {LastNameInput}] has been added.","Add Success", MessageBoxButton.OK, MessageBoxImage.Information);

            //Clear input fields
            FirstNameInput = string.Empty;
            LastNameInput = string.Empty;

            //Load table
            LoadEmployees();
        }

        /// <summary>
        /// Delete Employee
        /// </summary>
        /// <param name="obj"></param>
        public void DeleteEmployee(object obj)
        {
            //Perform query
            if(_employeeRepository.Delete(SelectedEmployee.EmployeeID))
                MessageBox.Show($"Employee [{SelectedEmployee.FullName}] has been deleted.", "Delete Success", MessageBoxButton.OK, MessageBoxImage.Information);

            //Refresh table
            LoadEmployees();
        }

        /// <summary>
        /// Edit Employee
        /// </summary>
        /// <param name="obj"></param>
        public void EditEmployee(object obj)
        {
            //Check Empty fields
            if (string.IsNullOrEmpty(SelectedEmployee.FirstName) || string.IsNullOrEmpty(SelectedEmployee.LastName))
            {
                MessageBox.Show($"{nameof(FirstNameInput)} or {nameof(LastNameInput)} is empty.", "Edit Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Perform query
            if (_employeeRepository.Update(SelectedEmployee))
                MessageBox.Show($"Employee [{SelectedEmployee.FullName}] has been edited.", "Edit Success", MessageBoxButton.OK, MessageBoxImage.Information);
            
            //Refresh Table
            LoadEmployees();
        }

        /// <summary>
        /// Load EmployeeList table
        /// </summary>
        public void LoadEmployees()
        {
            var d = _employeeRepository.GetAll();
            EmployeeList = new ObservableCollection<EmployeeModel>(d);
        }

        /// <summary>
        /// Repository for CRUD functions
        /// </summary>
        EmployeeRepository _employeeRepository { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public EmployeeTabViewModel()
        {
            //repo
            _employeeRepository = new EmployeeRepository();

            //commands
            InsertCommand = new RelayCommand(InsertEmployee);
            RefreshCommand = new RelayCommand((object obj) => { LoadEmployees(); });
            DeleteCommand = new RelayCommand(DeleteEmployee);
            UpdateCommand = new RelayCommand(EditEmployee);

            //load table
            LoadEmployees();
        }
        #endregion
    }
}