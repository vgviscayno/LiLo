using LiLo_Library.Models;
using LiLo_Library.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LiLoManager_WpfApp.ViewModels
{
    public class TimesheetSummaryViewModel
    {
        public ObservableCollection<string> Months { get; set; }

        public string SelectedMonth { get; set; }

        public string Year { get; set; }

        public ObservableCollection<TimesheetSummaryModel> TimesheetSummary { get; set; }

        TimesheetRepository _timesheetRepository { get; set; }
        EmployeeRepository _employeeRepository { get; set; }

        void loadData()
        {
            TimesheetSummary.Clear();

            //selected year and month
            string SelectedYearAndMonth = DateTime.Parse(Year + "/" + SelectedMonth).ToString("yyyy-MM");

            var Employees = _employeeRepository.GetAll();

            //Loop employees
            foreach(var emp in Employees)
            {
                //get all records for selected year and month
                var timesheetsForTheMonth = _timesheetRepository.GetMonthlySummaryForEmployee(SelectedYearAndMonth, emp);
                if(timesheetsForTheMonth.Count > 0)
                {
                    TimesheetSummary.Add(new TimesheetSummaryModel(emp, timesheetsForTheMonth));
                }
            }
        }

        public ICommand LoadCommand { get; set; }

        public TimesheetSummaryViewModel()
        {
            #region Repositories
            _timesheetRepository = new TimesheetRepository();
            _employeeRepository = new EmployeeRepository();
            #endregion

            #region Commands
            LoadCommand = new RelayCommand((object obj) => { loadData(); });
            #endregion

            Months = new ObservableCollection<string>((new List<string> {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"}));

            #region Public Properties

            //initialize Timesheet Summary
            TimesheetSummary = new ObservableCollection<TimesheetSummaryModel>();

            //initialize default values
            SelectedMonth = DateTime.Now.ToString("MMMM");
            Year = DateTime.Now.ToString("yyyy");

            //load data
            loadData();

            #endregion
        }
    }
}
