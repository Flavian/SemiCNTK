using System;
using System.Windows.Input;

namespace SzabolcsLecke1
{
    class SimpleCommand : ICommand
    {
        private readonly Action _executeMethod;


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public SimpleCommand(Action action)
        {
            _executeMethod = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeMethod.Invoke();
        }

    }

}
