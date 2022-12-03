using System;
using System.Windows.Input;

namespace TKompColumnsDataReader.Utility
{
	public class CommandHandler : ICommand
	{
		private Action _action;
		private bool _canExecute;

		public event EventHandler CanExecuteChanged;

		public CommandHandler(Action action, bool canExecute)
		{
			_action = action;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter) => _canExecute;

		public void Execute(object parameter) => _action();
	}
}
