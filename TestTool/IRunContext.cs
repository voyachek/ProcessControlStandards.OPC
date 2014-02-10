using System.Diagnostics;

namespace ProcessControlStandarts.OPC.TestTool
{
	public interface IRunContext
	{
		bool IsBusy { get; set; }

		TraceSource Log { get; }
	}
}
