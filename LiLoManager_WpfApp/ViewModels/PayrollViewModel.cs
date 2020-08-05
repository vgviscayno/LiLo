using LiLo_Library.Models;
using LiLo_Library.Repositories;
using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LiLoManager_WpfApp.ViewModels
{
    public class PayrollViewModel : BaseViewModel
    {
        #region Public Properties

        #region Employee Selection
        public ObservableCollection <EmployeeModel> EmployeesList { get; set; }

        public EmployeeModel SelectedEmployee { get; set; }

        #endregion

        #region Date range setting
        public DateTime SelectedStartDate { get; set; }

        public DateTime SelectedEndDate { get; set; }

        /// <summary>
        /// Checks days between start date and end date
        /// 
        /// In the event of the end date being earlier than the start date,
        /// start date and end date are set to their default values:
        /// 
        /// end date: current date
        /// start date: current date - 30 days
        /// </summary>
        public TimeSpan DatesDifference 
        {
            get 
            {
                if(SelectedEndDate < SelectedStartDate)
                {
                    ResetStartAndEndDates();
                }
                return SelectedEndDate - SelectedStartDate + new TimeSpan(24, 0, 0);

            }  
        }

        public DateTime MaxDate { get; set; }

        public DateTime MinDate { get; set; }
        #endregion

        #region Gross Salary
        public double NumberOfDaysWorked 
        {
            get 
            {
                var timesheets = _timesheetRepository.GetSpecificDatesForEmployee(SelectedEmployee, SelectedStartDate, SelectedEndDate);
                TimesheetSummaryModel emp = new TimesheetSummaryModel(SelectedEmployee, timesheets, true);
                return emp.TotalDays;
            }
        }

        public double GrossSalary
        {
            get => SelectedEmployee.Position.DailyRate * NumberOfDaysWorked;
        }
        #endregion

        #region Expenses

        public List<ExpenseModel> ExpensesNames { get; set; }

        public ObservableCollection<ExpenseModel> ExpensesList { get; set; }

        public double TotalExpenses { get; set; }

        public ExpenseModel SelectedExpense { get; set; }

        public ExpenseModel SelectedExpenseName { get; set; }

        private string userEnteredExpenseName;
        
        public string UserEnteredExpenseName
        {
            get 
            { 
                return userEnteredExpenseName;
            }

            set 
            {
                var newExpenseName = new ExpenseModel() { Name = value };

                SelectedExpenseName = newExpenseName;
            }
        }

        #endregion

        #region Income
        public ObservableCollection<IncomeModel> IncomeList { get; set; }

        public double TotalIncome { get; set; }

        public IncomeModel SelectedIncome { get; set; }
        #endregion

        public double NetSalary { get; set; }

        #endregion

        #region Button methods

        #region Expenses List Methods
        /// <summary>
        /// Calculates total amount of expenses in ExpensesList
        /// </summary>
        public bool CalculateExpenses()
        {
            double res = 0;
            foreach (var expense in ExpensesList)
            {
                if (expense.IsNull)
                {
                    MessageBox.Show("One of listed expenses is invalid. Please check the listed expenses.", "Invalid Expense", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                res += expense.Amount;
            }
            TotalExpenses = res;
            return true;
        }

        public void DeleteExpense()
        {
            MessageBoxResult confirmation = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);

            if (confirmation != MessageBoxResult.Yes)
                return;

            if (SelectedExpense.IsNull)
            {
                MessageBox.Show("No expense was selected.", "Delete Result", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ExpensesList.Remove(SelectedExpense);
        }
        #endregion

        #region Income List Methods
        public bool CalculateIncome()
        {
            double res = 0;
            
            foreach (var income in IncomeList)
            {
                if (income.IsNull)
                {
                    MessageBox.Show("One of listed income is invalid. Please check the listed income.", "Invalid Income", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                res += income.Amount;
            }

            TotalIncome = res;
            return true;
        }

        public void DeleteIncome()
        {
            MessageBoxResult confirmation = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);

            if (confirmation != MessageBoxResult.Yes)
                return;

            if (SelectedIncome.IsNull)
            {
                MessageBox.Show("No expense was selected.", "Delete Result", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IncomeList.Remove(SelectedIncome);
        }
        #endregion

        /// <summary>
        /// Validates entered expenses and income before calculating the net salary
        /// </summary>
        public void CalculateNetSalary()
        {
            if(CalculateExpenses() && CalculateIncome())
                NetSalary = (GrossSalary + SelectedEmployee.Position.PositionalAllowance + TotalIncome) - TotalExpenses;
            else
                MessageBox.Show("An error was detected in calculating the expenses and income. To proceed, please resolve these problems first.", "Invalid Expense/Income", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Saves Payroll and Its corresponding expenses to database
        /// </summary>
        public void SavePayroll()
        {
            PayrollModel payrollRow = new PayrollModel()
            {
                EmployeeID = SelectedEmployee.EmployeeID,
                StartDate = SelectedStartDate,
                EndDate = SelectedEndDate,
                DaysWorked = NumberOfDaysWorked,
                GrossSalary = GrossSalary,
                NetSalary = NetSalary
            };

            if (_payrollRepository.Exists(payrollRow))
            {
                MessageBox.Show("Payroll already generated for the following date range and employee", "Save Payroll Result", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<ExpenseModel> expenseRows = new List<ExpenseModel>(ExpensesList.ToList());

            List<IncomeModel> incomeRows = new List<IncomeModel>(IncomeList.ToList());
            
            _payrollRepository.AddPayroll(payrollRow, expenseRows, incomeRows);

            //reset fields

            ResetStartAndEndDates();
            InitializeExpenses();
        }

        /// <summary>
        /// Export Payroll to PDF
        /// </summary>
        public void ExportToPDF()
        {
            Document document = new Document();

            document.Info.Title = $"{SelectedEmployee.LastName}_{SelectedStartDate:MM-dd}_{SelectedEndDate:MM-dd}";

            document.Info.Subject = $"Payroll for {SelectedEmployee.LastName} from {SelectedStartDate:MM-dd} to {SelectedEndDate:MM-dd}";

            document.Info.Author = "LiLoManager";

            Section section = document.AddSection();

            Paragraph paragraph = section.AddParagraph();

            paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);

            paragraph.AddFormattedText("Exported PDF file");

            document.UseCmykColor = true;

            const bool unicode = false;

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode);

            pdfRenderer.Document = document;

            pdfRenderer.RenderDocument();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);      
            saveFileDialog.Title = "Save As PDF file";
            saveFileDialog.FileName = $"{SelectedEmployee.LastName}_{SelectedStartDate:MM-dd}_{SelectedEndDate:MM-dd}";
            saveFileDialog.DefaultExt = "pdf";
            saveFileDialog.Filter = "Pdf Files|*.pdf";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            
            if (saveFileDialog.ShowDialog() == true)
            {
                pdfRenderer.PdfDocument.Save(saveFileDialog.FileName);

                Process.Start(saveFileDialog.FileName);
            }
        }

        #endregion

        #region Repositories
        public EmployeeRepository _employeeRepository { get; set; }
        
        public TimesheetRepository _timesheetRepository { get; set; }

        public PayrollRepository _payrollRepository { get; set; }
        #endregion

        #region Commands
        public ICommand CalculateExpensesCommand { get; set; }

        public ICommand DeleteExpenseCommand { get; set; }

        public ICommand CalculateIncomeCommand { get; set; }

        public ICommand DeleteIncomeCommand { get; set; }

        public ICommand CalculateNetSalaryCommand { get; set; }

        public ICommand SavePayrollCommand { get; set; }

        public ICommand ExportToPDFCommand { get; set; }
        #endregion

        #region Constructors
        public PayrollViewModel()
        {
            #region Initialzing repositories
            _employeeRepository = new EmployeeRepository();

            _timesheetRepository = new TimesheetRepository();

            _payrollRepository = new PayrollRepository();
            #endregion

            #region Initializing public properties

            SelectedStartDate = DateTime.Now.AddDays(-30);

            SelectedEndDate = DateTime.Now;

            MaxDate = _timesheetRepository.GetLatestDate();

            MinDate = _timesheetRepository.GetEarliestDate();

            ResetStartAndEndDates();

            ExpensesList = new ObservableCollection<ExpenseModel>();

            InitializeExpenses();

            IncomeList = new ObservableCollection<IncomeModel>();

            EmployeesList = new ObservableCollection<EmployeeModel>(_employeeRepository.GetAll());
            
            SelectedEmployee = EmployeesList[0];
            #endregion

            #region Initializing commands
            CalculateExpensesCommand = new RelayCommand((object obj) => CalculateExpenses()) ;
            DeleteExpenseCommand = new RelayCommand((object obj) => DeleteExpense());
            CalculateIncomeCommand = new RelayCommand((object obj) => CalculateIncome());
            DeleteIncomeCommand = new RelayCommand((object obj) => DeleteIncome());
            CalculateNetSalaryCommand = new RelayCommand((object obj) => CalculateNetSalary());
            SavePayrollCommand = new RelayCommand((object obj) => SavePayroll());
            ExportToPDFCommand = new RelayCommand((object obj) => ExportToPDF());
            #endregion
        }
        #endregion

        /// <summary>
        /// Adds usual expenses to the list of expenses,
        /// such as government contributions, etc.
        /// </summary>
        public void InitializeExpenses()
        {
            ExpensesList.Clear();
            
            ExpenseModel SSSContribution = new ExpenseModel() { Name = "SSS Contribution", Remarks = $"{DateTime.Now:MMMM yyyy}", Amount = 0 };
            ExpenseModel PhilHealthContribution = new ExpenseModel() { Name = "Philhealth Contribution", Remarks = $"{DateTime.Now:MMMM yyyy}", Amount = 150.00 };
            ExpenseModel PagIbigContribution = new ExpenseModel() { Name = "Pag-Ibig Contribution", Remarks = $"{DateTime.Now:MMMM yyyy}", Amount = 100.00 };

            ExpensesList.Add(SSSContribution);
            ExpensesList.Add(PhilHealthContribution);
            ExpensesList.Add(PagIbigContribution);
        }

        /// <summary>
        /// Sets end date to current date and start date to current - 15 days,
        /// given that both dates are within the maxdate and mindate
        /// </summary>
        public void ResetStartAndEndDates()
        {
            if (DateTime.Now.AddDays(-15) > MaxDate)
                SelectedStartDate = MaxDate;
            else if (DateTime.Now.AddDays(-15) < MinDate)
                SelectedStartDate = MinDate;
            else
                SelectedStartDate = DateTime.Now.AddDays(-15);

            if (DateTime.Now > MaxDate)
                SelectedEndDate = MaxDate;
            else if (DateTime.Now < MinDate)
                SelectedEndDate = MinDate;
            else
                SelectedEndDate = DateTime.Now;
        }
    }
}
