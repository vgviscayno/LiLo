using LiLoManager_WpfApp.ViewModels;
using System.Windows.Controls;


namespace LiLoManager_WpfApp.Views
{
    /// <summary>
    /// Interaction logic for TimesheetSummaryView.xaml
    /// </summary>
    public partial class TimesheetSummaryView : UserControl
    {
        public TimesheetSummaryView()
        {
            InitializeComponent();
            this.DataContext = new TimesheetSummaryViewModel();
        }
    }
}
