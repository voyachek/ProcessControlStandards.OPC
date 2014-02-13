#region using

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

#endregion

namespace ProcessControlStandards.OPC.TestTool
{
	public class WorkerThread : IDisposable
	{
		public class Task
		{
			public object Argument { get; set; }

			public Action<Task, DoWorkEventArgs> Do { get; set; }

			public Action<Task, RunWorkerCompletedEventArgs> Completed { get; set; }
		}

		public WorkerThread(string name)
		{
			synchronizationContext = SynchronizationContext.Current;
			newItemEvent = new AutoResetEvent(false);
			exitThreadEvent = new ManualResetEvent(false);
			eventArray = new WaitHandle[2];
			eventArray[0] = newItemEvent;
			eventArray[1] = exitThreadEvent;

			thread = new Thread(ThreadRun) { Name = name };
			thread.Start();
		}

 		public void Post(Task task)
		{
			lock (((ICollection)queue).SyncRoot)
			{
				queue.Add(task);
				newItemEvent.Set();
			}
		}

		public void Dispose()
		{
			exitThreadEvent.Set();
			thread.Join();
		}

		private void ThreadRun()
		{
			while (WaitHandle.WaitAny(eventArray) != 1)
			{
				DoTasks();
			}

            DoTasks();
		}

	    private void DoTasks()
	    {
            List<Task> tasks;
	        lock (((ICollection) queue).SyncRoot)
	        {
                tasks = new List<Task>(queue);
                queue.Clear();
	        }

            foreach(var task in tasks)
                DoTask(task);
	    }

	    private void DoTask(Task task)
		{
			RunWorkerCompletedEventArgs resultArgs;
			try
			{
				var args = new DoWorkEventArgs(task.Argument);
				task.Do(task, args);				
				resultArgs = new RunWorkerCompletedEventArgs(args.Result, null, false);
			}
			catch (Exception e)
			{
				resultArgs = new RunWorkerCompletedEventArgs(null, e, false);
			}
            if (task.Completed != null)
			    synchronizationContext.Post(state => task.Completed(task, resultArgs), resultArgs);			
		}

		private readonly SynchronizationContext synchronizationContext;

		private readonly EventWaitHandle newItemEvent;
		
		private readonly EventWaitHandle exitThreadEvent;
		
		private readonly WaitHandle[] eventArray;

        private readonly List<Task> queue = new List<Task>();

		private readonly Thread thread;
	}
}
