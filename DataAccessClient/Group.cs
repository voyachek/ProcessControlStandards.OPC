#region using

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;

using ProcessControlStandarts.OPC.Core;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
	public class Group : IDisposable
	{
		#region DataCallback

		private class DataCallback : IOPCDataCallback
		{
			public DataCallback(Group @group)
			{
				this.@group = @group;
			}

			public void OnDataChange(int transactionId, int groupId, int quality, int error, uint count, int[] clientIds, IntPtr values, short[] qualities, long[] timeStamps, int[] errors)
			{
				if (@group.ClientId != groupId)
					return;

				using (var reader = new ItemValueReader(clientIds, values, qualities, timeStamps, errors))
				{
					var handler = @group.dataChangeHandlers;
					if(handler != null)
						handler(@group, new DataChangeEventArgs(groupId, transactionId, quality, error, reader.Values));
				}
			}

			public void OnReadComplete(int transactionId, int groupId, int quality, int error, uint count, int[] clientIds, IntPtr values, short[] qualities, long[] timeStamps, int[] errors)
			{
				if (@group.ClientId != groupId)
					return;

				using (var reader = new ItemValueReader(clientIds, values, qualities, timeStamps, errors))
				{
					var handler = @group.readCompleteHandlers;
					if(handler != null)
						handler(@group, new DataChangeEventArgs(groupId, transactionId, quality, error, reader.Values));
				}
			}

			public void OnWriteComplete(int transactionId, int groupId, int error, uint count, int[] clientIds, int[] errors)
			{
				if (@group.ClientId != groupId)
					return;

				var handler = @group.writeCompleteHandlers;
				if (handler != null)
				{
					var results = new KeyValuePair<int, int>[count];
					for (var i = 0; i < count; i++)
						results[i] = new KeyValuePair<int, int>(clientIds[i], errors[i]);
					
					handler(@group, new WriteCompleteEventArgs(groupId, transactionId, error, results));
				}
			}

			public void OnCancelComplete(int transactionId, int groupId)
			{
				if (@group.ClientId != groupId)
					return;

				var handler = @group.cancelCompleteHandlers;
				if (handler != null)
					handler(@group, new CompleteEventArgs(groupId, transactionId));
			}

			private readonly Group @group;
		}

		#endregion

		internal Group(DAServer server, int clientId, int serverId, string name, int updateRate, IOPCItemMgt @group)
		{
			this.server = server;
			ClientId = clientId;
			ServerId = serverId;
			Name = name;
			this.@group = @group;
			UpdateRate = updateRate;

			syncIO = (IOPCSyncIO) @group;
			groupManagment = (IOPCGroupStateMgt) @group;
			try
			{
				asyncIO = (IOPCAsyncIO2) @group;				
			}
			catch (InvalidCastException)
			{				
			}
			try
			{
				connectionPointContainer = (IConnectionPointContainer) @group;				
			}
			catch (InvalidCastException)
			{				
			}
		}

		[SecurityPermission(SecurityAction.LinkDemand)] 
		~Group()
		{
			Dispose(false);
		}

		public event EventHandler<DataChangeEventArgs> DataChange
		{
			add
			{
				InitializeAsyncMode();

				dataChangeHandlers += value;
			}
			remove
			{
				// ReSharper disable DelegateSubtraction
				dataChangeHandlers -= value;
				// ReSharper restore DelegateSubtraction
			}
		}

		public event EventHandler<DataChangeEventArgs> ReadComplete
		{
			add
			{
				InitializeAsyncMode();

				readCompleteHandlers += value;
			}
			remove
			{
				// ReSharper disable DelegateSubtraction
				readCompleteHandlers -= value;
				// ReSharper restore DelegateSubtraction
			}
		}

		public event EventHandler<WriteCompleteEventArgs> WriteComplete
		{
			add
			{
				InitializeAsyncMode();

				writeCompleteHandlers += value;
			}
			remove
			{
				// ReSharper disable DelegateSubtraction
				writeCompleteHandlers -= value;
				// ReSharper restore DelegateSubtraction
			}
		}

		public event EventHandler<CompleteEventArgs> CancelComplete
		{
			add
			{
				InitializeAsyncMode();

				cancelCompleteHandlers += value;
			}
			remove
			{
				// ReSharper disable DelegateSubtraction
				cancelCompleteHandlers -= value;
				// ReSharper restore DelegateSubtraction
			}
		}

		public int ClientId { get; private set; }

		public int ServerId { get; private set; }

		public string Name { get; private set; }

		public int UpdateRate { get; private set; }

        public bool IsAsyncIOSupported
        {
            get { return asyncIO != null && connectionPointContainer != null; }
        }

		public GroupProperties GetProperties()
		{
			if(@group == null)
				throw new ObjectDisposedException("Group");

			string name;
			float percentDeadband;
			int updateRate, activeAsInt, timeBias, locale, clientId, serverId;

			groupManagment.GetState(
				out updateRate, 
				out activeAsInt, 
				out name, 
				out timeBias, 
				out percentDeadband, 
				out locale, 
				out clientId, 
				out serverId);

			return new GroupProperties
			{
				UpdateRate = updateRate,
				Active = activeAsInt != 0,
				Name = name,
				TimeBias = timeBias,
				PercentDeadband = percentDeadband,
				Locale = new CultureInfo(locale),
				ClientId = clientId,
				ServerId = serverId,
			};
		}

		public void SetProperties(GroupProperties properties)
		{
			if(@group == null)
				throw new ObjectDisposedException("Group");

			int revisedUpdateRate;
			groupManagment.SetState(
				properties.UpdateRate, 
				out revisedUpdateRate, 
				properties.Active ? 1 : 0, 
				properties.TimeBias, 
				properties.PercentDeadband, 
				properties.Locale.LCID, 
				properties.ClientId);

			ClientId = properties.ClientId;
			UpdateRate = revisedUpdateRate;

			if(string.Equals(properties.Name, Name))
				groupManagment.SetName(properties.Name);
			Name = properties.Name;
		}

		[SecurityPermission(SecurityAction.LinkDemand)] 
		public ItemResult[] AddItems(Item[] items)
		{
			if(@group == null)
				throw new ObjectDisposedException("Group");
			items.ArgumentNotNull("items");
			if(items.Length == 0)
				return new ItemResult[0];

			using(var reader = new ItemResultReader(items))
			{
				int[] errors;
				IntPtr dataPtr;
				@group.AddItems((uint)items.Length, reader.Items, out dataPtr, out errors);

				return reader.Read(dataPtr, errors);
			}				
		}

		[SecurityPermission(SecurityAction.LinkDemand)] 
		public ItemResult[] ValidateItems(Item[] items)
		{
			if(@group == null)
				throw new ObjectDisposedException("Group");
			items.ArgumentNotNull("items");
			if(items.Length == 0)
				return new ItemResult[0];

			using(var reader = new ItemResultReader(items))
			{
				int[] errors;
				IntPtr dataPtr;
				@group.ValidateItems((uint)items.Length, reader.Items, 0, out dataPtr, out errors);

				return reader.Read(dataPtr, errors);
			}				
		}

		public int[] RemoveItems(int[] serverIds)
		{
			if(@group == null)
				throw new ObjectDisposedException("Group");
			serverIds.ArgumentNotNull("serverIds");
			if(serverIds.Length == 0)
				return new int[0];

			int[] errors;
			@group.RemoveItems((uint)serverIds.Length, serverIds, out errors);

			return errors;
		}

		public int[] SetActiveState(int[] serverIds, bool active)
		{
			if(@group == null)
				throw new ObjectDisposedException("Group");
			serverIds.ArgumentNotNull("serverIds");
			if(serverIds.Length == 0)
				return new int[0];

			int[] errors;
			@group.SetActiveState((uint)serverIds.Length, serverIds, active ? 1 : 0, out errors);

			return errors;
		}

		public int[] SetClientHandles(int[] serverIds, int[] clientIds)
		{
			if(@group == null)
				throw new ObjectDisposedException("Group");
			serverIds.ArgumentNotNull("serverIds");
			if(serverIds.Length == 0)
				return new int[0];

			int[] errors;
			@group.SetClientHandles((uint)serverIds.Length, serverIds, clientIds, out errors);

			return errors;			
		}

		public int[] SetDatatypes(int[] serverIds, VarEnum[] types)
		{
			if(@group == null)
				throw new ObjectDisposedException("Group");
			serverIds.ArgumentNotNull("serverIds");
			if(serverIds.Length == 0)
				return new int[0];

			int[] errors;
			var typesAsShort = new short[types.Length];
			for (var i = 0; i < types.Length; i++)
				typesAsShort[i] = (short)types[i];
			@group.SetDatatypes((uint)serverIds.Length, serverIds, typesAsShort, out errors);

			return errors;				
		}

		[SecurityPermission(SecurityAction.LinkDemand)] 
		public ItemValue[] SyncReadItems(DataSource source, int[] serverIds)
		{
			if(@group == null)
				throw new ObjectDisposedException("Group");
			serverIds.ArgumentNotNull("serverIds");
			if(serverIds.Length == 0)
				return new ItemValue[0];

			int[] errors;
			IntPtr dataPtr;
			syncIO.Read(source, (uint)serverIds.Length, serverIds, out dataPtr, out errors);

			return ItemValueReader.Read(dataPtr, errors);
		}

		public int[] SyncWriteItems(int[] serverIds, object[] values)
		{
			if(@group == null)
				throw new ObjectDisposedException("Group");
			serverIds.ArgumentNotNull("serverIds");
			values.ArgumentNotNull("values");
			if(serverIds.Length == 0)
				return new int[0];

			using(var writer = new ItemValueWriter(values))
			{
				int[] errors;
				syncIO.Write((uint)serverIds.Length, serverIds, writer.Values, out errors);

				return errors;
			}
		}

        public int[] AsyncReadItems(int[] serverIds, int transactionId, out int cancelId)
        {
            cancelId = 0;
			if(@group == null)
				throw new ObjectDisposedException("Group");
			if(asyncIO == null)
				throw new NotSupportedException();
			serverIds.ArgumentNotNull("serverIds");
            if (serverIds.Length == 0)
                return new int[0];

            int tmp;
            int[] errors;
            asyncIO.Read((uint)serverIds.Length, serverIds, transactionId, out tmp, out errors);

			cancelId = tmp;
	        return errors;
        }

        public int[] AsyncWriteItems(int[] serverIds, object[] values, int transactionId, out int cancelId)
        {
            cancelId = 0;
			if(@group == null)
				throw new ObjectDisposedException("Group");
			if(asyncIO == null)
				throw new NotSupportedException();
			serverIds.ArgumentNotNull("serverIds");
            if (serverIds.Length == 0)
                return new int[0];

            using(var writer = new ItemValueWriter(values))
            {
                int tmp;
                int[] errors;
				asyncIO.Write((uint)serverIds.Length, serverIds, writer.Values, transactionId, out tmp, out errors);

                cancelId = tmp;
                return errors;
            }
        }

        public void AsyncRefresh(DataSource source, int transactionId, out int cancelId)
        {
            cancelId = 0;
			if(@group == null)
				throw new ObjectDisposedException("Group");
			if(asyncIO == null)
				throw new NotSupportedException();

            int tmp;
            asyncIO.Refresh2(source, transactionId, out tmp);

            cancelId = tmp;
        }

        public void AsyncCancel(int cancelId)
        {
			if(@group == null)
				throw new ObjectDisposedException("Group");

            asyncIO.Cancel2(cancelId);
        }

        public void AsyncSetEnable(bool enable)
        {
			if(@group == null)
				throw new ObjectDisposedException("Group");
			if(asyncIO == null)
				throw new NotSupportedException();

            asyncIO.SetEnable(enable ? 1 : 0);	        
        }

        public bool AsyncGetEnable()
        {
			if(@group == null)
				throw new ObjectDisposedException("Group");
			if(asyncIO == null)
				throw new NotSupportedException();

	        int tmp;
            asyncIO.GetEnable(out tmp);

	        return tmp != 0;
        }

		[SecurityPermission(SecurityAction.LinkDemand)] 
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		[SecurityPermission(SecurityAction.LinkDemand)] 
		protected virtual void Dispose(bool disposing)
		{
			if(@group != null)				
			{
				if (connectionPoint != null)
				{
					try
					{
						connectionPoint.Unadvise(asyncCookie);
					}
					finally
					{
						Marshal.ReleaseComObject(connectionPoint);
						connectionPoint = null;            	
					}
				}

				Marshal.ReleaseComObject(@group);

				if(disposing)
					server.RemoveGroup(ServerId);

				@group = null;
			}			
		}

		private void InitializeAsyncMode()
		{
			if(@group == null)
				throw new ObjectDisposedException("Group");
			if(asyncIO == null || connectionPointContainer == null)
				throw new NotSupportedException();

			if (asyncCallback != null)
				return;

            var dataCallbackId = new Guid("39c13a70-011e-11d0-9675-0020afd8adb3");
            IConnectionPoint point;
            connectionPointContainer.FindConnectionPoint(ref dataCallbackId, out point);
            if (point == null)
                throw new NotSupportedException();

			var callback = new DataCallback(this);
            point.Advise(callback, out asyncCookie);

			asyncCallback = callback;
            connectionPoint = point;
		}

		private readonly DAServer server;

		private IOPCItemMgt @group;

		private readonly IOPCGroupStateMgt groupManagment;

		private readonly IOPCSyncIO syncIO;

		private readonly IOPCAsyncIO2 asyncIO;

		private readonly IConnectionPointContainer connectionPointContainer;

		private DataCallback asyncCallback;

		private int asyncCookie;

		private IConnectionPoint connectionPoint;

		private EventHandler<DataChangeEventArgs> dataChangeHandlers;

		private EventHandler<DataChangeEventArgs> readCompleteHandlers;

		private EventHandler<WriteCompleteEventArgs> writeCompleteHandlers;

		private EventHandler<CompleteEventArgs> cancelCompleteHandlers;
	}
}
