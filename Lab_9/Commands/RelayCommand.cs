using System;
using System.Windows.Input;
namespace Lab_9.Commands{
    public class RelayCommand : ICommand {
        private readonly Action execute;
        public RelayCommand(Action execute){
            this.execute = execute;
        }
        public bool CanExecute(object? parameter) {
            return true;
        }
        public void Execute(object? parameter){
            execute();
        }
        public event EventHandler? CanExecuteChanged{
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}