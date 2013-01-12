using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using StructurizerNEW.Domain;
using StructurizerNEW.Extra;

namespace StructurizerNEW
{
    /// <summary>
    /// The program is responsible for managing different content roots.
    /// </summary>
    class Program
    {
        private static readonly List<FolderEntity> activeRoots = new List<FolderEntity>();

        static void Main(string[] args)
        {
            Console.WriteLine("StructurizR ||| Simple, convention-based document processor");            
            Console.WriteLine();

            if (!args.Any())
            {
                Console.WriteLine("Please provide a config file as parameter");
                Console.ReadLine();
                return;
            }

            var configFilePath = args.First();

            if (!File.Exists(configFilePath))
            {
                Console.WriteLine("Specified config file not found: {0}", configFilePath);
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Loading {0}...", configFilePath);

            using (var sr = new StreamReader(configFilePath))
            {
                var configObj = JsonConvert.DeserializeObject<CoreFile>(sr.ReadToEnd());

                FolderEntity newobj = null;

                foreach (var root in configObj.Roots)
                {
                    switch (root.Type)
                    {
                        case "Project":
                            newobj = new Project(new DirectoryInfo(root.Path));
                            break;                        
                        default:
                            Console.WriteLine("Unsupported type");
                            break;
                    }

                    if (root.WatchFiles)
                    {
                        new FileWatcher(newobj);
                    }   

                    activeRoots.Add(newobj);
                }
            }        

            var loop = true;

            do
            {
                switch (Console.ReadLine())
                {
                    case "exit":
                        loop = false;
                        break;
                    case "build":
                        foreach (var processingRoot in activeRoots)
                        {
                            processingRoot.Process();
                        }
                        break;
                }
            } 
            while (loop);
        }     
    }
}
