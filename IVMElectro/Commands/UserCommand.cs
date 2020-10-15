using System;
using System.Windows.Input;

namespace IVMElectro.Commands {
    class UserCommand : ICommand {
        readonly Action execute;
        readonly Func<bool> canExecute;
        public UserCommand(Action action, Func<bool> func) {
            execute = action ?? throw new ArgumentNullException("trouble execute");
            canExecute = func;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; } //реализация пользовательского метода для подписки клиентского кода на событие
            remove { CommandManager.RequerySuggested -= value; } //реализация пользовательского метода для отмены подписки клиентского кода на событие
        }
        public bool CanExecute(object parameter) => canExecute == null ? true : canExecute();
        public void Execute(object parameter) => execute();
    }
}
