#region using

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using ProcessControlStandards.OPC.Core;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// OPC DA Server client. Helps to connect to the OPC DA server.
    /// </summary>
	public class DAServer : Server
	{
        /// <summary>
        /// Category UUID of OPC DA 1.0.
        /// </summary>
		public static readonly Guid Version10 = new Guid("{63D5F430-CFE4-11d1-B2C8-0060083BA1FB}");

        /// <summary>
        /// Category UUID of OPC DA 2.0.
        /// </summary>
        public static readonly Guid Version20 = new Guid("{63D5F432-CFE4-11d1-B2C8-0060083BA1FB}");

        /// <summary>
        /// Connects to specified OPC DA Server.
        /// </summary>
        /// <param name="id">UUID of OPC DA Server.</param>
		public DAServer(Guid id) :
			this(id, string.Empty)
		{
		}

        /// <summary>
        /// Connects to specified OPC DA Server.
        /// </summary>
        /// <param name="id">Program ID of OPC DA Server.</param>
		[SecurityPermission(SecurityAction.LinkDemand)] 
		public DAServer(string id) :
			this(id, string.Empty)
		{
		}

        /// <summary>
        /// Connects to specified remote OPC DA Server.
        /// </summary>
        /// <param name="id">UUID of OPC DA Server.</param>
        /// <param name="host">Host name or IP address of computer with OPC DA Server.</param>
		public DAServer(Guid id, string host) : base(id, host)
		{
			server = (IOPCServer) Common;
		}

        /// <summary>
        /// Connects to specified remote OPC DA Server.
        /// </summary>
        /// <param name="id">Program ID of OPC DA Server.</param>
        /// <param name="host">Host name or IP address of computer with OPC DA Server.</param>
		[SecurityPermission(SecurityAction.LinkDemand)] 
		public DAServer(string id, string host) : base(id, host)
		{		
			server = (IOPCServer) Common;
		}

        /// <summary>
        /// Returns browser of OPC DA Server namespace.
        /// </summary>
        /// <param name="blockSize">Number of item names to read in one call.</param>
        /// <returns>Browser of OPC DA Server namespace.</returns>
		public ServerAddressSpaceBrowser GetAddressSpaceBrowser(int blockSize = 1000)
		{
			DisposedCheck();

            return new ServerAddressSpaceBrowser(server, blockSize);
		}
		
        /// <summary>
        /// Returns an object that helps to read the information about items.
        /// </summary>
        /// <returns>Object that helps to read the information about items.</returns>
		public ItemProperties GetItemProperties()
		{
			DisposedCheck();

			return new ItemProperties(server);
		}
	
        /// <summary>
        /// Creates new OPC DA group.
        /// </summary>
        /// <param name="clientId">Group client ID.</param>
        /// <param name="name">Group name.</param>
        /// <param name="active">Initial state.</param>
        /// <param name="updateRate">Item values update rate in milliseconds.</param>
        /// <param name="percentDeadband">Item values deadband in percents.</param>
        /// <returns>New OPC DA group.</returns>
        /// <remarks>Uses current timezone.</remarks>
		public Group AddGroup(int clientId, string name, bool active, int updateRate, float percentDeadband)
		{
			var timeBias = (int)TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalMinutes;

			return AddGroup(clientId, name, active, updateRate, percentDeadband, timeBias);
		}

        /// <summary>
        /// Creates new OPC DA group.
        /// </summary>
        /// <param name="clientId">Group client ID.</param>
        /// <param name="name">Group name.</param>
        /// <param name="active">Initial state.</param>
        /// <param name="updateRate">Item values update rate in milliseconds.</param>
        /// <param name="percentDeadband">Item values deadband in percents.</param>
        /// <param name="timeBias">Timezone time bias.</param>
        /// <returns>New OPC DA group.</returns>
        public Group AddGroup(int clientId, string name, bool active, int updateRate, float percentDeadband, int timeBias)
		{
			DisposedCheck();
			name.ArgumentNotNullOrEmpty("name");

			object group;
			int serverId;
			var id = typeof(IOPCItemMgt).GUID;

			server.AddGroup(
				name,
				active ? 1 : 0,
				updateRate,
				clientId,
				ref timeBias,
				ref percentDeadband,
				(uint)CultureInfo.CurrentUICulture.LCID,
				out serverId,
				out updateRate,
                ref id,
				out group);

			return new Group(this, clientId, serverId, name, updateRate, (IOPCItemMgt)group);
		}

        /// <summary>
        /// Converts error code to localized message based on current OPC Server locale.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <returns>Message for specified error code.</returns>
		public override string GetErrorString(int code)
		{
			DisposedCheck();

			string result;
			server.GetErrorString(code, (uint)CultureInfo.CurrentUICulture.LCID, out result);
			return result;
		}

        /// <summary>
        /// Retrieves OPC DA Server properties.
        /// </summary>
        /// <returns>OPC DA Server properties.</returns>
		[SecurityPermission(SecurityAction.LinkDemand)] 
		public ServerProperties GetProperties()
		{
			DisposedCheck();

			IntPtr dataPtr;
			server.GetStatus(out dataPtr);
			try
			{
				var position = 0;
				var result = new ServerProperties();

				// FILETIME ftStartTime;
				var time = Marshal.ReadInt64(dataPtr, position);
				result.StartTime = DateTime.FromFileTimeUtc(time);
				position += sizeof(long);

				// FILETIME ftCurrentTime;
				time = Marshal.ReadInt64(dataPtr, position);
				result.CurrentTime = DateTime.FromFileTimeUtc(time);
				position += sizeof(long);

				// FILETIME ftLastUpdateTime;
				time = Marshal.ReadInt64(dataPtr, position);
				result.LastUpdateTime = DateTime.FromFileTimeUtc(time);
				position += sizeof(long);

				// tagOPCSERVERSTATE dwServerState;
				result.ServerState = (ServerState)Marshal.ReadInt32(dataPtr, position);
				position += sizeof(int);

				// uint dwGroupCount;
				result.GroupCount = Marshal.ReadInt32(dataPtr, position);
				position += sizeof(int);

				// uint dwBandWidth;
				result.Bandwidth = Marshal.ReadInt32(dataPtr, position);
				position += sizeof(int);

				// ushort wMajorVersion;
				result.MajorVersion = Marshal.ReadInt16(dataPtr, position);
				position += sizeof(short);

				// ushort wMinorVersion;
				result.MinorVersion = Marshal.ReadInt16(dataPtr, position);
				position += sizeof(short);

				// ushort wBuildNumber;
				result.BuildNumber = Marshal.ReadInt16(dataPtr, position);
				position += sizeof(short);

				// ushort wReserved;
				position += sizeof(short);

				if (IntPtr.Size == 8)
					position += sizeof(int);

				// string szVendorInfo;
				var vendorInfo = Marshal.ReadIntPtr(dataPtr, position);
			    if (vendorInfo != IntPtr.Zero)
			    {
                    result.VendorInfo = Marshal.PtrToStringUni(vendorInfo);
                    Marshal.FreeCoTaskMem(vendorInfo);
			    }

				return result;
			}
			finally
			{
				if(dataPtr != IntPtr.Zero)
					Marshal.FreeCoTaskMem(dataPtr);				
			}
		}

		internal void RemoveGroup(int groupId)
		{
			if(Common == null)
				return;

			server.RemoveGroup(groupId, 0);
		}

		private readonly IOPCServer server;
	}
}
