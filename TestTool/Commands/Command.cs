#region using

using System;
using System.Windows.Input;

#endregion

namespace ProcessControlStandarts.OPC.TestTool.Commands
{
	public class Command : ICommand
	{
		public Command(string name)
		{
			Name = name;
		}

		#region ICommand

		public string Name { get; private set; }

		public virtual void Execute(object parameter)
		{
		}

		public virtual bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;

		#endregion

		public void RiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}
	}
}