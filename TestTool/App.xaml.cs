using AutoMapper;

using ProcessControlStandards.OPC.DataAccessClient;

namespace ProcessControlStandards.OPC.TestTool
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
	    public App()
	    {
	        Mapper.CreateMap<DAGroupProperties, GroupProperties>();
	    }
	}
}
