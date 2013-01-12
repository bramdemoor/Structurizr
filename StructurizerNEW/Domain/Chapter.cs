using System.IO;
using System.Linq;
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
            Parent.Children.Add(this);

            if(Level == 1)
            {
                // Real chapter
                
                templater.AppendChapterStart(Path.Name);
            }
            else
            {
                // Sub chapter

                templater.AppendSubChapterStart(Path.Name);
            }        

            foreach (var file in Path.EnumerateFiles().Where(f => f.Extension.ToLower() == ".png" || f.Extension.ToLower() == ".jpg").OrderBy(k => k.Name, new MixedNumbersAndStringsComparer()))
            {
               File.Copy(file.FullName, outputDir + "\\" + file.Name, true);
            }

             foreach (var file in Path.GetFiles("*.md").OrderBy(k => k.Name, new MixedNumbersAndStringsComparer()))
             {
                 templater.Append(file.ReadMarkdown());
             }

             var exc = new ExcelToHtmlTableProcessor();
            foreach (var VARIABLE in Path.GetFiles("*.xlsx").OrderBy(k => k.Name, new MixedNumbersAndStringsComparer()))
            {
                var ht = exc.ProcessXlsTableToHtml(VARIABLE.FullName);
                templater.Append(ht);                
            }

            // Sub chapters
             foreach (var chapterDir in SubDirectories.OrderBy(k => k.Name, new MixedNumbersAndStringsComparer()))
             {
                var ch = new Chapter(chapterDir, Level + 1);
                 ch.Parent = this;
                 ch.Process(templater, Path.FullName);
             }

             if (Level == 1)
             {
                templater.AppendChapterEnd();     
             }
             else
             {
                 templater.AppendSubChapterEnd();
             }
            
        }
    }
}