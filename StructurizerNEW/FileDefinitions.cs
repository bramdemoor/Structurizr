namespace StructurizerNEW
{
    public class MetaFile
    {
        public string[] Include { get; set; }
        public string[] Exclude { get; set; }

        /// <summary>
        /// Friendly name
        /// </summary>
        public string Name { get; set; }

        public string Teaser { get; set; }

        public int Progress { get; set; }

        public string OutputDir { get; set; }

        public bool VsdProcessingEnabled { get; set; }

        /// <summary>
        /// If enabled, saves a duplicate of the png in the original folder. This allows your markdown editor to show previews of the files.
        /// </summary>
        public bool SavePngsInSourceDirectory { get; set; }
        
        public string Context { get; set; }
    }

    public class CoreFile
    {
        public Root[] Roots { get; set; }
    }

    public class Root
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public bool WatchFiles { get; set; }
    }
}