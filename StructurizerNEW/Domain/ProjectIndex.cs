using System.IO;

namespace StructurizerNEW.Domain
{
    public class ProjectIndex : FolderEntity
    {
        public ProjectIndex(DirectoryInfo path) : base(path, "ProjectIndex")
        {
        }
    }
}