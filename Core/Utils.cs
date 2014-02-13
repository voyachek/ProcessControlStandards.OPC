#region using

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

using ProcessControlStandards.OPC.Core.Properties;

#endregion

namespace ProcessControlStandards.OPC.Core
{
	static class Utils
	{
		public static string FormatString(this string template, object arg1)
		{
			return string.Format(CultureInfo.CurrentUICulture, template, arg1);
		}

		public static string FormatString(this string template, object arg1, object arg2)
		{
			return string.Format(CultureInfo.CurrentUICulture, template, arg1, arg2);
		}

		public static void Warning(this TraceSource log, Exception e)
		{
			if(log != null)
				log.TraceData(TraceEventType.Warning, 0, e);
		}

		public static void Warning(this TraceSource log, string message, Exception e)
		{
			if(log != null)
				log.TraceData(TraceEventType.Warning, 0, message, e);
		}

		public static void ArgumentNotNull(this object argument, string argumentName)
		{
			if(argument == null)
				throw new ArgumentNullException(argumentName);
		}

		public static void ArgumentNotNullOrEmpty(this string argument, string argumentName)
		{
			if(string.IsNullOrEmpty(argument))
				throw new ArgumentException(Resources.ErrorArgumentNullOrEmpty, argumentName);
		}

		public static IntPtr AllocCoTaskMem(this Guid[] array)
		{
			var buffer = Marshal.AllocCoTaskMem(
				array.Length * Guid.Empty.ToByteArray().Length);

			var pos = 0;
			foreach (var t in array)
			    foreach(var @byte in t.ToByteArray())
			        Marshal.WriteByte(buffer, pos++, @byte);

		    return buffer;
		}
	}
}
