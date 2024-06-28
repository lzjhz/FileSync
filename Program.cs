using System;
using System.Diagnostics;

namespace FileSync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string source = args[0] ?? "";
            string destination = args[1] ?? "";
            string pattern = args[2] ?? "*.*";
            
            if (source.Length == 0 || destination.Length == 0)
            {
                Console.WriteLine("Usage: filesync <source> <destination> <pattern>");
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