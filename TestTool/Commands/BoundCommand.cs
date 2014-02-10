#region using

using System.Windows;
using System.Windows.Controls;

#endregion

namespace ProcessControlStandarts.OPC.TestTool.Commands
{
	public static class BoundCommand
	{
		public static object GetParameter(DependencyObject obj)
		{
			return obj.GetValue(ParameterProperty);
		}

		public static void SetParameter(DependencyObject obj, object value)
		{
			obj.SetValue(ParameterProperty, value);
		}

		public static readonly DependencyProperty ParameterProperty = DependencyProperty.RegisterAttached("Parameter", typeof(object), typeof(BoundCommand), new UIPropertyMetadata(null, ParameterChanged));

		private static void ParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var item = d as MenuItem;
			if (item == null)
				return;
			
			item.CommandParameter = e.NewValue;
			var cmd = item.Command as Command;
			if (cmd != null)
				cmd.RiseCanExecuteChanged();
		}
	}
}
