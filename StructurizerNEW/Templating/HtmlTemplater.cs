using System.IO;
using System.Linq;
using System.Text;
using MarkdownSharp;
using StructurizerNEW.Domain;

namespace StructurizerNEW.Templating
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

            var html = GetHtml(project);

            html = html.Replace("$CONTENT$", new Markdown().Transform(sb.ToString()));

            using (var streamWriter = new StreamWriter(project.Path + project.MetaData.OutputDir + "\\" + outputFilename))
            {
                streamWriter.Write(html);
            }

            return new HtmlTemplaterResult(project.Path + project.MetaData.OutputDir + "\\" + outputFilename);
        }

        private string GetHtml(Project project)
        {
            using (var sr = new StreamReader(Directory.GetCurrentDirectory() + relativeTemplatesPath + bootstrapTemplate1))
            {
                var normalHtml = sr.ReadToEnd();

                // Run this through razor first!
                normalHtml = RazorEngine.Razor.Parse(normalHtml, project);

                return normalHtml;
            }
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