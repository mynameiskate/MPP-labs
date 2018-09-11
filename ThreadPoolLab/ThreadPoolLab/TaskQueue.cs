using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPoolLab
{
    public class TaskQueue : IDisposable
    {
        public delegate void TaskDelegate();

        private ConcurrentQueue<TaskDelegate> _taskQueue = new ConcurrentQueue<TaskDelegate>();
        private List<Thread> _threadPool = new List<Thread>();
        private volatile bool _isDisposed = false;
        private int _activeThreads;

        public TaskQueue(int threadCount)
        {
            for (int i = 0; i < threadCount; i++)
            {
                var thread = new Thread(ThreadTask);
                _threadPool.Add(thread);
                thread.Start();
            }
        }

        public bool EnqueueTask(TaskDelegate task)
        {
            if (!_isDisposed)
            {
                _taskQueue.Enqueue(task);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ThreadTask()
        {
            while (true)
            {
                TaskDelegate task;

                if (!_isDisposed)
                {
                    Interlocked.Increment(ref _activeThreads);
                    if (_taskQueue.TryDequeue(out task))
                    {
                        task();
                    }
                    Interlocked.Decrement(ref _activeThreads);
                }
                else
                {
                    return;
                }
            }
        }

        ~TaskQueue()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                while (_activeThreads != 0 || _taskQueue.Count != 0)
                {

                }
                foreach (var thread in _threadPool)
                {
                    thread.Abort();
                }

                _isDisposed = true;
            }
        }
    }
}