using System.Threading;
using ThreadPoolLab;
using static ThreadPoolLab.TaskQueue;

namespace ThreadingLab
{
    public class Parallel
    {
        private static AutoResetEvent _resetHandler = new AutoResetEvent(true);

        public static void WaitAll(TaskDelegate[] delegates)
        {
            using (var threadPool = new TaskQueue(delegates.Length))
            {
                foreach (var taskDelegate in delegates)
                {
                    threadPool.EnqueueTask(() =>
                    {
                        _resetHandler.WaitOne();
                        taskDelegate();
                        _resetHandler.Set();
                    });
                }
            }
        }
    }
}
