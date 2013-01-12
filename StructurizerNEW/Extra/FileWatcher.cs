using System;
using System.IO;
using StructurizerNEW.Domain;

namespace StructurizerNEW.Extra
{
    public class FileWatcher
    {
        private readonly FolderEntity processor;

        public FileWatcher(FolderEntity processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Sets up a FileWatcher, wires up the events and starts watching 
        /// </summary>
        private void CreateFileWatcher()
        {
            var watcher = new FileSystemWatcher { Path = processor.Path.FullName };

            watcher.Changed += (s, e) => OnChangeDected();
            watcher.Created += (s, e) => OnChangeDected();
            watcher.Deleted += (s, e) => OnChangeDected();
            watcher.Renamed += (s, e) => OnChangeDected();

            watcher.EnableRaisingEvents = true;
        }

        private void OnChangeDected()
        {
            Console.WriteLine("Filesystem change detected. Triggering build...");
            processor.Process();
        } 
    }
}