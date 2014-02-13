#region using

using System.Globalization;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// OPC DA group properties.
    /// </summary>
	public class GroupProperties
	{
        /// <summary>
        /// OPC DA group client ID.
        /// </summary>
		public int ClientId { get; set; }

        /// <summary>
        /// OPC DA group server ID.
        /// </summary>
		public int ServerId { get; set; }

        /// <summary>
        /// OPC DA group name.
        /// </summary>
		public string Name { get; set; }

        /// <summary>
        /// OPC DA group state.
        /// </summary>
		public bool Active { get; set; }

        /// <summary>
        /// OPC DA group items update rate.
        /// </summary>
		public int UpdateRate { get; set; }

        /// <summary>
        /// Timezone time bias.
        /// </summary>
		public int TimeBias { get; set; }

        /// <summary>
        /// Item values deadband in percents.
        /// </summary>
		public float PercentDeadband { get; set; }

        /// <summary>
        /// OPC DA group locale.
        /// </summary>
		public CultureInfo Locale { get; set; }
	}
}
