using System;
using System.Windows.Input;

namespace LiLo_WpfApp.ViewModels
{
    /// <summary>
    /// Command interface
    /// </summary>
    public class RelayCommand : ICommand
    {
        public Action<object> _execute { get; set; }
        public Predicate<object> _canExecute { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return (_canExecute == null) ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(_execute));
            _execute = execute;
            _canExecute = canExecute;
        }
    }
}
