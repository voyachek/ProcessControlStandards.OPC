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
	        Mapper.CreateMap<Models.DAGroupProperties, GroupProperties>();
            Mapper.CreateMap<GroupProperties, Models.DAGroupProperties>();
	    }
	}
}
