using System.Threading;

namespace MutexLab
{
    class Mutex : IMutex
    {
        private const int EMPTY_ID = -1;
        private int _lockerId = EMPTY_ID;

        public void Lock()
        {
            while (!CompareIdExchange(Thread.CurrentThread.ManagedThreadId, EMPTY_ID))
            {
                Thread.Yield();
            }
        }

        public bool Unlock()
        {
            return CompareIdExchange(EMPTY_ID, Thread.CurrentThread.ManagedThreadId);
        }

        private bool CompareIdExchange(int newId, int compareToId)
        {
            return Interlocked.CompareExchange(ref _lockerId, newId, compareToId) == compareToId;
        }

    }
}
