#region using

using System;
using System.Windows.Input;

#endregion

namespace ProcessControlStandards.OPC.TestTool.Commands
{
	public class Command<T> : ICommand
	{
		public Command(string name)
		{
			Name = name;
		}

		#region ICommand

		public string Name { get; private set; }

		public void Execute(object parameter)
		{
            Execute((T)parameter);
		}

		public bool CanExecute(object parameter)
		{
            return CanExecute((T)parameter);
		}

		public event EventHandler CanExecuteChanged;

		#endregion

		public void RiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

        protected virtual void Execute(T parameter)
        {
        }

        protected virtual bool CanExecute(T parameter)
        {
            return true;
        }
	}
}