using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace StructurizerNEW.Domain
{
    public abstract class FolderEntity
    {
        public FolderEntity Parent { get; set; }
        public List<FolderEntity> Children { get; set; }

        public DirectoryInfo Path { get; set; }

        public MetaFile MetaData { get; private set; }
        public IEnumerable<DirectoryInfo> SubDirectories { get; private set; }

        public string OutputDir { get; set; }

        public string ProcessedBody { get; set; }

        public bool HasChildren
        {
            get { return Children.Any(); }
        }

        public bool HasOwnContent
        {
            get { return !string.IsNullOrWhiteSpace(ProcessedBody); }
        }

        public string PathNameWithoutSpacesAndSlashes
        {
            get { return Path.Name.Replace(" ", "").Replace("/", "-"); }
        }

        public string FullPathNameWithoutSpacesAndSlashes
        {
            get
            {
                var sb = new StringBuilder();
                if (Parent != null && Parent is Chapter)
                {
                    sb.Append(Parent.PathNameWithoutSpacesAndSlashes);
                    sb.Append("-");
                }
                sb.Append(PathNameWithoutSpacesAndSlashes);

                return sb.ToString();               
            }
        }

        public string NavItemHash
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append("#/");
                if(Parent != null && Parent is Chapter)
                {
                    sb.Append(Parent.PathNameWithoutSpacesAndSlashes);
                    sb.Append("/");
                }
                sb.Append(PathNameWithoutSpacesAndSlashes);

                return sb.ToString();
            }
        }

        public string PathNameWithoutStartingNumbers
        {
            get { return RemoveNr(Path.Name); }
        }

        protected FolderEntity(DirectoryInfo path, string context)
        {
            Children = new List<FolderEntity>();

            Path = path;

            ReadMetadata();

            if(MetaData == null) MetaData = new MetaFile();
            MetaData.Context = context;

            ReadSubdirectories();
        }

        private void ReadMetadata()
        {
            var temp = Path.GetFiles("meta.json").FirstOrDefault();
            if (temp != null)
            {
                using (var fs = temp.OpenRead())
                {
                    using (var sr = new StreamReader(fs))
                    {
                        MetaData = JsonConvert.DeserializeObject<MetaFile>(sr.ReadToEnd());
                    }                    
                }
            }
        }

        private void ReadSubdirectories()
        {
            //MetaData.Include

            SubDirectories = Path.GetDirectories().Where(d => !d.Name.StartsWith("_"));     // Convention: ignore names that start with _
        }

        public abstract void Process();

        private static string RemoveNr(string name)
        {
            if (name.Contains("-"))
            {
                name = name.Split('-').ElementAt(1);
            }
            return name;
        }
    }
}