#region using

using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
	public struct ItemProperty
	{
		public int Id;

		public string Description;

		public VarEnum Type;
	}
}
