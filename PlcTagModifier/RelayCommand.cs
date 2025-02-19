using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlcTagModifier
{
    public class RelayCommand : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Func<object, Task> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Overload for non-async methods.
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
            : this(param => { execute(param); return Task.CompletedTask; }, canExecute)
        { }

        public bool CanExecute(object parameter)
            => _canExecute == null || _canExecute(parameter);

        public async void Execute(object parameter)
            => await _execute(parameter);

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
