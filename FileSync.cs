using System.Diagnostics;

namespace FileSync
{
    public class FileSync
    {
        public string Source;
        public string Destination;
        public string Pattern;

        private FileSystemWatcher? fsWatcher;

        public FileSync(string source, string destination, string pattern = "*.*")
        {
            this.Source = source;
            this.Destination = destination;
            this.Pattern = pattern;
        }

        public void Start()
        {
            fsWatcher = new FileSystemWatcher
            {
                Path = Source,
                Filter = Pattern,
                //NotifyFilter = NotifyFilters.LastWrite,
            };

            Console.WriteLine("Watching changes in " + Source);

            string[] files = Directory.GetFiles(Source, Pattern);
            Console.WriteLine("Found " + files.Length + " file(s)");

            foreach (string filepath in files)
            {
                fsWatcherEvent(fsWatcher, new FileSystemEventArgs(0, Source, Path.GetFileName(filepath)));
            }

            fsWatcher.Created += fsWatcherEvent;
            fsWatcher.Changed += fsWatcherEvent;
            fsWatcher.Deleted += fsWatcherEvent;
            
            fsWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            if (fsWatcher != null)
            {
                fsWatcher.EnableRaisingEvents = false;
                fsWatcher.Dispose();
            }
        }

        private void fsWatcherEvent(object sender, FileSystemEventArgs e)
        {
            if (e.Name != null)
            {
                string source = e.FullPath;
                string destination = Path.Combine(Destination, e.Name);

                switch (e.ChangeType)
                {
                    case 0:
                    case WatcherChangeTypes.Created:
                    case WatcherChangeTypes.Changed:
                        if (e.ChangeType == 0)
                        {
                            Console.WriteLine(source + " => " + destination);
                        }
                        else
                        {
                            Console.WriteLine(source + " (" + e.ChangeType + ") => " + destination);
                        }
                        File.Copy(source, destination, true);
                        break;
                }
            }
        }
    }
}
