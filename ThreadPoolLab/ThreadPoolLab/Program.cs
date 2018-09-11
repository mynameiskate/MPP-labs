using System;
using System.Threading;

namespace ThreadPoolLab
{
    class Program
    {
        private const string THREAD_PROMPT = "Type in amount of threads.";
        private const string TASK_PROMPT = "Type in amount of tasks.";
        private const string THREAD_ERROR_MSG = "The amount of threads should fall in range (0; ..].";
        private const string TASK_ERROR_MSG = "The amount of tasks should fall in range (0; ..].";
        private const string QUIT_PROMPT = "Type \"q\" to exit or any other key to continue...";
        private const string QUIT_COMMAND = "q";

        static void Main(string[] args)
        {
            string input = string.Empty;

            while (input.Trim() != QUIT_COMMAND)
            {

                int threadCount = 0;
                int taskCount = 0;

                if (ReadNumber(out threadCount, THREAD_PROMPT, THREAD_ERROR_MSG)
                    && ReadNumber(out taskCount, TASK_PROMPT, TASK_ERROR_MSG))
                {
                    using (var taskQueue = new TaskQueue(threadCount))
                    {
                        for (int i = 0; i < taskCount; i++)
                        {
                            taskQueue.EnqueueTask(ThreadPrint);
                        }
                    }
                }

                Console.WriteLine(QUIT_PROMPT);
                input = Console.ReadLine();
            }
        }

        private static bool ReadNumber(out int threadNumber, string promptMsg, string errorMsg)
        {
            Console.WriteLine(promptMsg);

            bool parseResult = Int32.TryParse(Console.ReadLine(), out threadNumber)
                && (threadNumber > 0);

            if (!parseResult)
            {
                Console.WriteLine(errorMsg);
            }

            return parseResult;
        }

        private static void ThreadPrint()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"Thread {threadId} started.");
            Thread.Sleep(1000);
            Console.WriteLine($"Thread {threadId} finished.");
        }
    }
}
