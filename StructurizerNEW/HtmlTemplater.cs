using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MarkdownSharp;

namespace StructurizerNEW
{
    /// <summary>
    /// Simple Twitter Bootstrap html templater. Single file output.
    /// </summary>
    public class HtmlTemplater
    {
        private const string relativeTemplatesPath = "\\App_Data\\Templates\\";
        private const string bootstrapTemplate1 = "markdowncss.cshtml";
        private const string outputFilename = "index.html";
       
        private readonly StringBuilder sb;

        public HtmlTemplater()
        {
            sb = new StringBuilder();
        }

        public HtmlTemplaterResult Process(Project project)
        {
            // Copy theme files
            File.Copy(Directory.GetCurrentDirectory() + relativeTemplatesPath + "\\markdown.png", project.Path + project.MetaData.OutputDir + "\\markdown.png", true);

            using (var sr = new StreamReader(ReadTemplate(bootstrapTemplate1)))
            {
                var html = sr.ReadToEnd();
                
                html = html.Replace("$CONTENT$", new Markdown().Transform(sb.ToString()));
                html = html.Replace("$TITLE$", project.MetaData.Name);
                html = html.Replace("$SUBTITLE$", project.MetaData.Teaser);
                html = html.Replace("$INDEX$", GenerateIndex(project));

                using (var streamWriter = new StreamWriter(project.Path + project.MetaData.OutputDir + "\\" + outputFilename))
                {
                    streamWriter.Write(html);
                }
            }

            return new HtmlTemplaterResult(project.Path + project.MetaData.OutputDir + "\\" + outputFilename);
        }

        private string GenerateIndex(Project project)
        {
            var indexBuilder = new StringBuilder();

            foreach (var chapter in project.Children)
            {
                indexBuilder.AppendFormat("<li><a href=\"#\" data-target=\"{0}\" data-bind=\"click: activate\" class=\"chapter-link\">{1}</a>", chapter.Path.Name.Replace(" ", ""), RemoveNr(chapter.Path.Name));
                indexBuilder.Append("<ul class=\"nav nav-list\">");

                foreach (var subChapter in chapter.Children)
                {
                    indexBuilder.AppendFormat("<li><a href=\"#\" data-target=\"{0}\" data-bind=\"click: activateSub\" class=\"section-link\">{1}</a></li>", subChapter.Path.Name.Replace(" ", ""), RemoveNr(subChapter.Path.Name));
                }

                indexBuilder.Append("</ul>");
                indexBuilder.Append("</li>");

            }

            return indexBuilder.ToString();
        }

        private static string ReadTemplate(string name)
        {
            return Directory.GetCurrentDirectory() + relativeTemplatesPath + name;
        }

        public void AppendChapterStart(string name)
        {            
            sb.AppendFormat("<div class=\"chapter\" id=\"{0}\">", name.Replace(" ", ""));

            sb.AppendFormat("<h2>{0}</h2>", RemoveNr(name));
        }

        private static string RemoveNr(string name)
        {
            if (name.Contains("-"))
            {
                name = name.Split('-').ElementAt(1);
            }
            return name;
        }

        public void Append(string text)
        {
            sb.Append(text);
        }

        public void AppendChapterEnd()
        {
            sb.AppendFormat("</div>");
        }

        public void AppendSubChapterEnd()
        {
            sb.AppendFormat("</div>");
        }

        public void AppendSubChapterStart(string name)
        {
            sb.AppendFormat("<div class=\"subchapter\" id=\"{0}\">", name.Replace(" ", ""));

            sb.AppendFormat("<h3>{0}</h3>", RemoveNr(name));
        }
    }

    public class HtmlTemplaterResult
    {
        public string GeneratedFilename { get; set; }

        public HtmlTemplaterResult(string generatedFilename)
        {
            GeneratedFilename = generatedFilename;
        }
    }
}