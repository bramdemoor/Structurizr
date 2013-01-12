using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace StructurizerNEW
{
    public abstract class FolderEntity
    {
        public FolderEntity Parent { get; set; }
        public List<FolderEntity> Children { get; set; }

        public DirectoryInfo Path { get; set; }

        public MetaFile MetaData { get; private set; }
        public IEnumerable<DirectoryInfo> SubDirectories { get; private set; }

        protected FolderEntity(DirectoryInfo path, string context)
        {
            Children = new List<FolderEntity>();

            Path = path;

            ReadMetadata();

            if(MetaData == null) MetaData = new MetaFile();
            MetaData.Context = context;

            ReadSubdirectories();
        }

        protected void ReadMetadata()
        {
            var temp = Path.GetFiles("meta.json").FirstOrDefault();
            if (temp != null)
            {
                MetaData = JsonConvert.DeserializeObject<MetaFile>(new StreamReader(temp.OpenRead()).ReadToEnd());
            }
        }

        protected void ReadSubdirectories()
        {
            //MetaData.Include

            SubDirectories = Path.GetDirectories().Where(d => !d.Name.StartsWith("_"));     // Convention: ignore names that start with _
        }

        public virtual void Process()
        {
            
        }
    }
}