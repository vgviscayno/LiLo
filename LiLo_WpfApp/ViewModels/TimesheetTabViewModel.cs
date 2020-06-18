using LiLo_Library.Models;
using LiLo_Library.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LiLo_WpfApp.ViewModels
{
    public class TimesheetTabViewModel : BaseViewModel
    {
        #region Public Properties
        public ObservableCollection<TimesheetModel> TimesheetTable { get; set; }

        public string TimeLog { get; set; }
        #endregion

        #region Repositories
        TimesheetRepository _timesheetRepository { get; set; }
        EmployeeRepository _employeeRepository { get; set; }
        #endregion

        #region Commands
        public ICommand LogCommand { get; set; }
        #endregion

        #region Private Methods
        void _log(object obj)
        {
            try
            {
                //get employee's data by id number
                var empLog = _employeeRepository.GetById(Int32.Parse(TimeLog));

                var LogData = new TimesheetModel()
                {
                    EmployeeID = empLog.EmployeeID
                };

                //if employee has logged in for the day, log out
                if (TimesheetTable.Contains(LogData))
                {
                    LogData = TimesheetTable[TimesheetTable.IndexOf(LogData)];
                    
                    if(LogData.OutTime == default)
                    {
                        LogData.OutTime = DateTime.Now;
                        
                        _timesheetRepository.Update(LogData);
                        
                        MessageBox.Show($"Goodbye {empLog.FullName}! You logged out at {LogData.OutTime:hh:mm tt}");
                    }
                }
                //otherwise, log in
                else
                {
                    LogData.Date = DateTime.Now;
                    LogData.InTime = DateTime.Now;

                    _timesheetRepository.Add(LogData);

                    MessageBox.Show($"Welcome {empLog.FullName}! You logged in at {LogData.InTime:hh:mm tt}");
                }

                //reload table and clear timelog textbox
                _loadTimesheet();
                TimeLog = string.Empty;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Load's today's timesheet from database to timesheet table
        /// </summary>
        void _loadTimesheet()
        {
            TimesheetTable = new ObservableCollection<TimesheetModel>(_timesheetRepository.GetAllForTheDay());
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public TimesheetTabViewModel()
        {
            //create instances of repositories
            _timesheetRepository = new TimesheetRepository();
            _employeeRepository = new EmployeeRepository();
            
            //create instances of commands
            LogCommand = new RelayCommand(_log);

            //load data
            _loadTimesheet();
        }
        #endregion
    }
}