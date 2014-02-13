using System.Diagnostics;

namespace ProcessControlStandards.OPC.TestTool
{
	public interface IRunContext
	{
		bool IsBusy { get; set; }

		TraceSource Log { get; }
	}
}
