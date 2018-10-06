using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ThreadPoolLab.TaskQueue;

namespace ThreadingLab
{
    class Program
    {
        private const string THREAD_PROMPT = "Type in amount of threads.";
        private const string THREAD_ERROR_MSG = "The amount of threads should fall in range (0; ..].";
        private const string QUIT_PROMPT = "Type \"q\" to exit or any other key to continue...";
        private const string QUIT_COMMAND = "q";
        private const string PROCESS_MSG = "Waiting for all tasks to finish..";
        private const string FINISH_MSG = "Task execution completed.";

        static void Main(string[] args)
        {
            string input = string.Empty;

            while (input.Trim() != QUIT_COMMAND)
            {

                int threadCount = 0;

                if (ReadNumber(out threadCount, THREAD_PROMPT, THREAD_ERROR_MSG))
                {
                    Console.WriteLine(PROCESS_MSG);
                    try
                    {
                        Parallel.WaitAll(CreateDelegateArray(threadCount));
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Couldn't process tasks: {e.Message}");
                    }
                    
                    Console.WriteLine(FINISH_MSG);
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

        private static TaskDelegate[] CreateDelegateArray(int delegateCount)
        {
            var taskDelegates = new TaskDelegate[delegateCount];
            for (int i = 0; i < delegateCount; i++)
            {
                taskDelegates[i] = ThreadPrint;
            }

            return taskDelegates;
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
