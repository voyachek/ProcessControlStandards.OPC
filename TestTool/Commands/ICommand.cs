namespace ProcessControlStandards.OPC.TestTool.Commands
{
    public interface ICommand : System.Windows.Input.ICommand
    {
        void RiseCanExecuteChanged();
    }
}
