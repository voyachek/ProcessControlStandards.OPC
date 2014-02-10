#region using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using ProcessControlStandarts.OPC.Core.Properties;

#endregion

namespace ProcessControlStandarts.OPC.Core
{
	public class ServerBrowser : IDisposable
	{
		public ServerBrowser(TraceSource log = null) : this(string.Empty)
		{
			this.log = log;
		}

		public ServerBrowser(string host, TraceSource log = null)
		{
			this.log = log;

			var serverType = string.IsNullOrEmpty(host) ? 
				Type.GetTypeFromCLSID(OpcEnum, true) : 
				Type.GetTypeFromCLSID(OpcEnum, host, true);
				
			serverList = (IOPCServerList)Activator.CreateInstance(serverType);
			try
			{
				serverList2 = (IOPCServerList2)serverList;
			}
			catch (InvalidCastException)
			{
			}
		}

		[EnvironmentPermission(SecurityAction.Demand, Unrestricted=true)]
		~ServerBrowser()
		{
			Dispose(false);
		}

		public IEnumerable<ServerDescription> GetEnumerator(params Guid[] categories)
		{
			IEnumGUID enumerator = null;
			IOPCEnumGUID opcEnumerator = null;
			try
			{
				var tmp = Guid.Empty;
				if(serverList2 != null)
					serverList2.EnumClassesOfCategories(
						(uint)categories.Length, 
						categories, 
						0,
						ref tmp, 
						out opcEnumerator);					
				else
					serverList.EnumClassesOfCategories(
						(uint)categories.Length, 
						categories, 
						0,
						ref tmp, 
						out enumerator);					

				while(true)
				{
					var id = Guid.Empty;
					var programId = string.Empty;
					var name = string.Empty;
					var verIndProgramId = string.Empty;

					uint fetched = 0;
					if(opcEnumerator != null)
						opcEnumerator.Next(1, out id, out fetched);
					else if (enumerator != null) 
						enumerator.Next(1, out id, out fetched);
					if(fetched == 0)
						break;

					try 
					{
						if(serverList2 != null)
							serverList2.GetClassDetails(ref id, out programId, out name, out verIndProgramId);
						else
							serverList.GetClassDetails(ref id, out programId, out name);
					}
					catch(Exception e)
					{
						log.Warning(Resources.ErrorRetrieveServerName.FormatString(id), e);
					}
		
					yield return new ServerDescription(id, programId, verIndProgramId, name);
				}
			}
			finally
			{
				if(enumerator != null)
					Marshal.ReleaseComObject(enumerator);
				if(opcEnumerator != null)
					Marshal.ReleaseComObject(opcEnumerator);
			}
		}

		[EnvironmentPermission(SecurityAction.Demand, Unrestricted=true)]
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted=true)]
		protected virtual void Dispose(bool dispose)
		{
			if(serverList != null)
				Marshal.ReleaseComObject(serverList);
			serverList = null;
		}

		private readonly TraceSource log;

		private IOPCServerList serverList;

		private readonly IOPCServerList2 serverList2;

		private static readonly Guid OpcEnum = new Guid("{13486D51-4821-11D2-A494-3CB306C10000}");
	}
}
