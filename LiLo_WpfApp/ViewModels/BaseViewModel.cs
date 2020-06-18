using PropertyChanged;
using System.ComponentModel;

namespace LiLo_WpfApp.ViewModels
{
    /// <summary>
    /// Base class for view models
    /// </summary>
    [ImplementPropertyChanged]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}
