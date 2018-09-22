using System;
using System.Threading;

namespace MutexLab
{
    class Program
    {
        private const int THREAD_EXECUTION_TIME = 1000;
        private const int ARG_COUNT = 1;
        private static IMutex _mutex;
        private static int _threadCount;

        static int Main(string[] args)
        {
            if (args.Length != ARG_COUNT)
            {
                Console.WriteLine("You should provide total amount of threads as parameter.");
                return 1;
            }

            if (!Int32.TryParse(args[0], out _threadCount))
            {
                Console.WriteLine("The amount of threads should be numeric.");
                return 1;
            }

            try
            {
                _mutex = new Mutex();

                for(int i = 0; i < _threadCount; i++)
                {
                    var thread = new Thread(() => ThreadTask(i % 2 == 0));
                    thread.Start();
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        private static void ThreadTask(bool interfere)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _mutex.Lock();
            Console.WriteLine($"Mutex was locked by thread {threadId}.");
            Thread.Sleep(THREAD_EXECUTION_TIME);
            UnlockMutex(threadId);

            if (interfere)
            {
                Thread.Sleep(10);
                UnlockMutex(threadId);
            }
        }

        private static void UnlockMutex(int threadId)
        {
            if (_mutex.Unlock())
            {
                Console.WriteLine($"Mutex was unlocked by thread {threadId}.");
            }
            else
            {
                Console.WriteLine($"Thread {threadId} failed to unlock mutex.");
            }
        }
    }
}
