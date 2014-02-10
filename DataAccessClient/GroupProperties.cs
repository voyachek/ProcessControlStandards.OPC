#region using

using System.Globalization;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
	public class GroupProperties
	{
		public int ClientId { get; set; }

		public int ServerId { get; set; }

		public string Name { get; set; }

		public bool Active { get; set; }

		public int UpdateRate { get; set; }

		public int TimeBias { get; set; }

		public float PercentDeadband { get; set; }

		public CultureInfo Locale { get; set; }
	}
}
