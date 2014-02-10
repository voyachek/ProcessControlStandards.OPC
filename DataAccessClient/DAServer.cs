#region using

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using ProcessControlStandarts.OPC.Core;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
	public class DAServer : Server
	{
		public static readonly Guid Version10 = new Guid("{63D5F430-CFE4-11d1-B2C8-0060083BA1FB}");

		public static readonly Guid Version20 = new Guid("{63D5F432-CFE4-11d1-B2C8-0060083BA1FB}");

		public DAServer(Guid id) :
			this(id, string.Empty)
		{
		}

		[SecurityPermission(SecurityAction.LinkDemand)] 
		public DAServer(string id) :
			this(id, string.Empty)
		{
		}

		public DAServer(Guid id, string host) : base(id, host)
		{
			server = (IOPCServer) Common;
		}

		[SecurityPermission(SecurityAction.LinkDemand)] 
		public DAServer(string id, string host) : base(id, host)
		{		
			server = (IOPCServer) Common;
		}

		public ServerAddressSpaceBrowser GetAddressSpaceBrowser()
		{
			DisposedCheck();

			return new ServerAddressSpaceBrowser(server);
		}
		
		public ItemProperties GetItemProperties()
		{
			DisposedCheck();

			return new ItemProperties(server);
		}
	
		public Group AddGroup(int clientId, string name, bool active, int updateRate, float percentDeadband)
		{
			var timeBias = (int)TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalMinutes;

			return AddGroup(clientId, name, active, updateRate, percentDeadband, timeBias);
		}

		public Group AddGroup(int clientId, string name, bool active, int updateRate, float percentDeadband, int timeBias)
		{
			DisposedCheck();
			name.ArgumentNotNullOrEmpty("name");

			object group;
			int serverId;
			var iid = typeof(IOPCItemMgt).GUID;

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
				ref iid,
				out group);

			return new Group(this, clientId, serverId, name, updateRate, (IOPCItemMgt)group);
		}

		public override string GetErrorString(int code)
		{
			DisposedCheck();

			string result;
			server.GetErrorString(code, (uint)CultureInfo.CurrentUICulture.LCID, out result);
			return result;
		}

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
				result.VendorInfo = Marshal.PtrToStringUni(vendorInfo);
				if(vendorInfo != IntPtr.Zero)
					Marshal.FreeCoTaskMem(vendorInfo);

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
