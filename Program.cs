using System;
using System.Diagnostics;

namespace FileSync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string source = args.Length > 0 ? args[0] : "";
            string destination = args.Length > 1 ? args[1] : "";
            string pattern = args.Length > 2 ? args[2] : "*.*";
            
            if (source.Length == 0 || destination.Length == 0)
            {
                Console.WriteLine("Usage: filesync <source> <destination> <pattern>");
                Environment.Exit(1);
            }
            if (!Path.Exists(source))
            {
                Console.Error.WriteLine("Source path does not exist");
                Environment.Exit(1);
            }
            if (!Path.Exists(destination))
            {
                Console.Error.WriteLine("Destination path does not exist");
                Environment.Exit(1);
            }
            FileSync sync = new FileSync(source, destination, pattern);
            EventWaitHandle waitHandle = new ManualResetEvent(false);

            sync.Start();

            waitHandle.Reset();

            Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
                waitHandle.Set();
                Debug.WriteLine("WaitHandle set");
            };

            waitHandle.WaitOne();

            Debug.WriteLine("Stopping sync...");
            sync.Stop();
        }
    }
}