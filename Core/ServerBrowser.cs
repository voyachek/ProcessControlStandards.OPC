#region using

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#endregion

namespace ProcessControlStandards.OPC.Core
{
    /// <summary>
    /// Helps to retrieve information about installed OPC Servers.
    /// </summary>
	public class ServerBrowser : IDisposable
	{
        /// <summary>
        /// Connects to local computer.
        /// </summary>
		public ServerBrowser() : this(string.Empty)
		{
		}

        /// <summary>
        /// Connects to remote computer.
        /// </summary>
        /// <param name="host">Host name or IP address of remote computer.</param>
		public ServerBrowser(string host)
		{
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

        /// <summary>
        /// Returns an enumerator to allow to determine available OPC servers.
        /// </summary>
        /// <param name="categories">OPC Server categories (OPCDA/OPCHDA etc.)</param>
        /// <returns>Enumerator to allow to determine available OPC servers.</returns>
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
                    var res = 0;
                    uint fetched = 0;
                    var ids = new Guid[1];
                    if (opcEnumerator != null)
                        res = opcEnumerator.Next((uint)ids.Length, ids, out fetched);
					else if (enumerator != null)
                        res = enumerator.Next(1, ids, out fetched);
                    if (res > 1)
                        Marshal.ThrowExceptionForHR(res);
					if(fetched == 0)
						break;

				    var id = ids[0];
				    ServerDescription serverDescription;
                    try 
					{
					    string name;
                        string programId;
                        var versionIndependentProgramId = string.Empty;
                        if (serverList2 != null)
                            serverList2.GetClassDetails(ref id, out programId, out name, out versionIndependentProgramId);
						else
							serverList.GetClassDetails(ref id, out programId, out name);
                        serverDescription = new ServerDescription(id, programId, versionIndependentProgramId, name);
					}
					catch(Exception e)
					{
                        serverDescription = new ServerDescription(id, e);
					}

                    yield return serverDescription;
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

        /// <summary>
        /// Disconnects from OPC Server browser.
        /// </summary>
		[EnvironmentPermission(SecurityAction.Demand, Unrestricted=true)]
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

        /// <summary>
        /// Disconnects from OPC Server browser.
        /// </summary>
        /// <param name="dispose">true - call in Dispose.</param>
		[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted=true)]
		protected virtual void Dispose(bool dispose)
		{
			if(serverList != null)
				Marshal.ReleaseComObject(serverList);
			serverList = null;
		}

		private IOPCServerList serverList;

		private readonly IOPCServerList2 serverList2;

		private static readonly Guid OpcEnum = new Guid("{13486D51-4821-11D2-A494-3CB306C10000}");
	}
}
