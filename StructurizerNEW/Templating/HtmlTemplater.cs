using System.IO;
using RazorEngine.Templating;
using StructurizerNEW.Domain;

namespace StructurizerNEW.Templating
{
    public class ProjectDocumentationTemplate<T> : TemplateBase<T>
    {
    }

    /// <summary>
    /// Simple Twitter Bootstrap html templater. Single file output.
    /// </summary>
    public class HtmlTemplater
    {
        private const string relativeTemplatesPath = "\\App_Data\\Templates\\";
        private const string bootstrapTemplate1 = "markdowncss.cshtml";
        private const string outputFilename = "index.html";
       
        public HtmlTemplaterResult Process(Project project)
        {
            // Copy theme files
            File.Copy(Directory.GetCurrentDirectory() + relativeTemplatesPath + "\\markdown.png", project.Path + project.MetaData.OutputDir + "\\markdown.png", true);

            var html = GetHtml(project);

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