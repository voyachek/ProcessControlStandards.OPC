#region using

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#endregion

namespace ProcessControlStandarts.OPC.Core
{
	public class Server : IDisposable
	{
		public Server(Guid id) :
			this(id, string.Empty)
		{
		}

		[SecurityPermission(SecurityAction.LinkDemand)] 
		public Server(string id) :
			this(id, string.Empty)
		{
		}

		public Server(Guid id, string host)
		{
			var type = string.IsNullOrEmpty(host) ? 
				Type.GetTypeFromCLSID(id, true) : 
				Type.GetTypeFromCLSID(id, host, true);
			Common = (IOPCCommon) Activator.CreateInstance(type);
		}

        [SecurityPermission(SecurityAction.LinkDemand)] 
		public Server(string id, string host)
		{		
			id.ArgumentNotNullOrEmpty("id");

			var type = string.IsNullOrEmpty(host) ? 
				Type.GetTypeFromProgID(id, true) : 
				Type.GetTypeFromProgID(id, host, true);
			Common = (IOPCCommon) Activator.CreateInstance(type);
		}

		[SecurityPermission(SecurityAction.LinkDemand)] 
		~Server()
		{
			Dispose(false);
		}

		[SecurityPermission(SecurityAction.LinkDemand)] 
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		[SecurityPermission(SecurityAction.LinkDemand)] 
		protected virtual void Dispose(bool dispose)
		{
			if(Common != null)
				Marshal.ReleaseComObject(Common);
			Common = null;
		}

		protected IOPCCommon Common { get; private set; }

		public void SetLocale()
		{
			Common.SetLocaleID(CultureInfo.CurrentUICulture.LCID);
		}

		public void SetLocale(CultureInfo locale)
		{
			locale.ArgumentNotNull("locale");

			Common.SetLocaleID(locale.LCID);
		}

		public CultureInfo GetLocale()
		{
			int localeId;
			Common.GetLocaleID(out localeId);
			return new CultureInfo(localeId);
		}

		public CultureInfo[] QueryAvailableLocales()
		{
			uint size;
			int[] locales;
			Common.QueryAvailableLocaleIDs(out size, out locales);

			var result = new CultureInfo[size];
			for(var i = 0; i < size; i++)
				result[i] = new CultureInfo(locales[i]);
			return result;
		}

		public virtual string GetErrorString(int error)
		{
			string description;
			Common.GetErrorString(error, out description);
			return description;
		}

		public void SetClientName(string name)
		{
			Common.SetClientName(name);
		}

		protected void DisposedCheck()
		{
			if(Common == null)
				throw new ObjectDisposedException("Server");
		}
	}
}
