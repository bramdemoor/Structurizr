using System.IO;
using System.Linq;
using MarkdownSharp;
using StructurizerNEW.Extra;

namespace StructurizerNEW.Domain
{
    public class Chapter : FolderEntity
    {
        public int Level { get; set; }

        public Chapter( DirectoryInfo path, int level, string outputDir) : base(path, "chapter")
        {            
            OutputDir = outputDir;
            Level = level;

            // Sections
            foreach (var chapterDir in SubDirectories.OrderBy(k => k.Name, new MixedNumbersAndStringsComparer()))
            {
                var ch = new Chapter(chapterDir, Level + 1, OutputDir);
                ch.Parent = this;
                Children.Add(ch);
            }
        }

        public override void Process()
        {
            ProcessedBody = "";

            foreach (var file in Path.EnumerateFiles().Where(f => f.Extension.ToLower() == ".png" || f.Extension.ToLower() == ".jpg").OrderBy(k => k.Name, new MixedNumbersAndStringsComparer()))
            {
               File.Copy(file.FullName, OutputDir + "\\" + file.Name, true);
            }

             foreach (var file in Path.GetFiles("*.md").OrderBy(k => k.Name, new MixedNumbersAndStringsComparer()))
             {
                 ProcessedBody += new Markdown().Transform(file.ReadMarkdown());
             }

             var exc = new ExcelToHtmlTableProcessor();
            foreach (var VARIABLE in Path.GetFiles("*.xlsx").OrderBy(k => k.Name, new MixedNumbersAndStringsComparer()))
            {
                ProcessedBody += exc.ProcessXlsTableToHtml(VARIABLE.FullName);                
            }

             foreach (var chapterDir in Children)
             {
                 chapterDir.Process();
             }            
        }
    }
}