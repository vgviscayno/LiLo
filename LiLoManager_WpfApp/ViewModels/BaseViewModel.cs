using PropertyChanged;
using System.ComponentModel;

namespace LiLoManager_WpfApp.ViewModels
{
    [ImplementPropertyChanged]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}