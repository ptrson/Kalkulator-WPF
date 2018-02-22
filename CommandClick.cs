using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kalkulator
{
    public class CommandClick : ICommand
    {
        public event EventHandler CanExecuteChanged;

        CommandDelegate _command;

        public CommandClick (CommandDelegate metoda) //kontener w formie delegacji który zapewni działanie akcji do wszystkich przycisków
        {
            _command = metoda; 
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _command (parameter);
        }
    }

    public delegate void CommandDelegate(object p);

}
