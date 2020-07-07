using System.Windows;
using LiLoManager_WpfApp.ViewModels;

namespace LiLoManager_WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new TimesheetSummaryViewModel();
        }
    }
}