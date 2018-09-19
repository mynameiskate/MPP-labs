using System;
using System.IO;
using System.Threading;
using ThreadPoolLab;

namespace FileCopier
{
    public class FileCopier
    {
        public event EventHandler<double> ProgressUpdated;
        private long _bytesCopied;
        private long _totalBytesCount;
        private string _source;
        private string _destination;
        private int _copiedCount;

        private double Progress
        {
            set
            {
                ProgressUpdated?.Invoke(this, value);
            }
        }

        public int CopyFolder(string source, string destination)
        {
            if (!Directory.Exists(source))
            {
                throw new DirectoryNotFoundException("Source folder does not exist.");
            }

            if (!Directory.Exists(destination))
            {
                throw new DirectoryNotFoundException("Destination folder does not exist.");
            }

            _source = source;
            _destination = destination;
            CopyFilesAndFolders();
            return _copiedCount;
        }

        private void CopySingleFolder(string path)
        {
            Directory.CreateDirectory(path.Replace(_source, _destination));
        }

        private void CopySingleFile(string path)
        {
            File.Copy(path, path.Replace(_source, _destination), true);

            _bytesCopied += (new FileInfo(path)).Length;

            ProgressUpdated?.Invoke(this, _bytesCopied / _totalBytesCount);
            //Progress = _bytesCopied / _totalBytesCount;

            Interlocked.Increment(ref _copiedCount);
        }

        private long DirSize(DirectoryInfo d)
        {
            long size = 0;
            FileInfo[] fis = d.GetFiles();

            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }

            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        private void CopyFilesAndFolders()
        {
            _bytesCopied = 0;
            _totalBytesCount = DirSize(new DirectoryInfo(_source));

            Progress = 0;

            using (var taskQueue = new TaskQueue())
            {
                string[] directories = Directory.GetDirectories(_source, "*", SearchOption.AllDirectories);
                foreach (string path in directories)
                {
                    taskQueue.EnqueueTask(() => CopySingleFolder(path));
                }

                string[] files = Directory.GetFiles(_source, "*", SearchOption.AllDirectories);
                foreach (string path in files)
                {
                    taskQueue.EnqueueTask(() => CopySingleFile(path));
                }
            }
        }
    }
}
