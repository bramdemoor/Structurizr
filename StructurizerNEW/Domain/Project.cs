using System;
using System.Collections.Generic;
using System.IO;
using StructurizerNEW.Extra;
using System.Linq;

namespace StructurizerNEW
{
    public class Project : FolderEntity
    {
        public const bool OpenAfterManualBuildTrigger = true;

        public Project(DirectoryInfo path) : base(path, "project")
        {
        }

        public override void Process()
        {
            var templater = new HtmlTemplater();

            //new VisioToPngProcessor().FindAndFlattenVSDs(Path.FullName);

            foreach (var chapterDir in SubDirectories.OrderBy(k => k.Name, new MixedNumbersAndStringsComparer()))
            {
                var chapter = new Chapter(chapterDir, 1);
                chapter.Parent = this;
                chapter.Process(templater, Path + MetaData.OutputDir);
            }

            var result = templater.Process(this);

            if (OpenAfterManualBuildTrigger)
            {
                if (File.Exists(result.GeneratedFilename))
                {
                    System.Diagnostics.Process.Start(result.GeneratedFilename);
                }
                else
                {
                    Console.WriteLine("Failed to find generated file");
                }                
            }
        }
    }
}