using System;
using System.IO;

namespace FileCopier
{
    class Program
    {
        private static ProgressBar _progressBar;
        private const int ARG_COUNT = 2;

        private static void OnProgressUpdated(object sender, double update)
        {
            _progressBar.Report(update);
        }

        static int Main(string[] args)
        {
            if (args.Length != ARG_COUNT)
            {
               Console.WriteLine("You should provide source and destination folders as parameters.");
               return 1;
            }

            try
            {
                var fileCopier = new FileCopier();
                _progressBar = new ProgressBar();
                fileCopier.ProgressUpdated += OnProgressUpdated;
                int copiedFiles = fileCopier.CopyFolder(args[0], args[1]);
                Console.WriteLine();
                Console.WriteLine($"Number of copied files: {copiedFiles}");
                return 0;
            }
            catch(DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }

            return 0;
        }
    }
}
