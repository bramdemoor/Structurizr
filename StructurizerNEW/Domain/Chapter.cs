using System.IO;
using System.Linq;
using MarkdownSharp;
using StructurizerNEW.Extra;
using StructurizerNEW.Templating;

namespace StructurizerNEW.Domain
{
    public class Chapter : FolderEntity
    {
        public int Level { get; set; }

        public Chapter( DirectoryInfo path, int level) : base(path, "chapter")
        {
            Level = level;
        }

        public void Process(HtmlTemplater templater, string outputDir)
        {
            ProcessedBody = "";

            Parent.Children.Add(this);   

            foreach (var file in Path.EnumerateFiles().Where(f => f.Extension.ToLower() == ".png" || f.Extension.ToLower() == ".jpg").OrderBy(k => k.Name, new MixedNumbersAndStringsComparer()))
            {
               File.Copy(file.FullName, outputDir + "\\" + file.Name, true);
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

            // Sub chapters
             foreach (var chapterDir in SubDirectories.OrderBy(k => k.Name, new MixedNumbersAndStringsComparer()))
             {
                var ch = new Chapter(chapterDir, Level + 1);
                 ch.Parent = this;
                 ch.Process(templater, Path.FullName);
             }            
        }
    }
}