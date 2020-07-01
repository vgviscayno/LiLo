using System.Collections.ObjectModel;

namespace LiLoManager_WpfApp.ViewModels
{
    public class AttendanceViewModel : BaseViewModel
    {
        public string test { get; set; }
        public AttendanceViewModel()
        {
            test = "Hello World";
        }
    }
}
