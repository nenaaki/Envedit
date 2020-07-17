using System;
using System.Windows.Input;

namespace Oniqys.Wpf.MVVM
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action _execute;

        private readonly Func<bool> _canExecute;

        public DelegateCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;
        public void Execute(object parameter) => _execute?.Invoke();

        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public class DelegateCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action<T> _execute;

        private readonly Func<T, bool> _canExecute;

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke((T)parameter) ?? true;
        public void Execute(object parameter) => _execute?.Invoke((T)parameter);

        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
