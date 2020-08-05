using LiLo_Library.Models;
using LiLo_WpfApp.ViewModels;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace LiLo_WpfApp.Views
{
    /// <summary>
    /// Interaction logic for TimesheetTabView.xaml
    /// </summary>
    public partial class TimesheetTabView : UserControl
    {
        public TimesheetTabView()
        {
            InitializeComponent();
            this.DataContext = new TimesheetTabViewModel();
        }
    }

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTimeDefaultToNullStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value == default(DateTime)) ? string.Empty : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(Shift), typeof(string))]
    public class ShiftNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.GetName(typeof(Shift), (Shift)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
