using System;
using System.Windows.Input;

namespace DemoApp.ViewModel
{
    /// <summary>
    /// Represents an actionable item displayed by a View.
    /// </summary>
    public class CommandViewModel : ViewModelBase
    {
        public CommandViewModel(ICommand command, CommandType commandType)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            this.Command = command;
            this.CommandType = commandType;
        }

        public CommandViewModel(string displayName, ICommand command, CommandType commandType)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            base.DisplayName = displayName;
            this.Command = command;
            this.CommandType = commandType;
        }

        public ICommand Command { get; private set; }

        public CommandType CommandType { get; private set; }
    }
}