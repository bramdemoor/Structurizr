using System.IO;
using System.Text;
using OfficeOpenXml;

namespace StructurizerNEW.Extra
{
    public class ExcelToHtmlTableProcessor
    {
        public string ProcessXlsTableToHtml(string path)
        {            
            using (var pck = new ExcelPackage(File.Open(path, FileMode.Open)))
            {
                var sheet = pck.Workbook.Worksheets[1];

                var sb = new StringBuilder();

                sb.AppendLine("<table class=\"table\" style=\"width: 700px;\">");
                sb.AppendLine("<thead><tr>");

                for (int colIndex = 1; colIndex < sheet.Dimension.End.Column + 1; colIndex++)
                {
                    sb.AppendLine(string.Format("<th>{0}</td>", Encode(sheet.Cells[1, colIndex].Value)));
                }   
                
                sb.AppendLine("</tr></thead>");
                sb.AppendLine("<tbody>");
                
                for (int rowIndex = 2; rowIndex < sheet.Dimension.End.Row + 1; rowIndex++)
                {
                    sb.AppendLine("<tr>");
                    for (int colIndex = 1; colIndex < sheet.Dimension.End.Column + 1; colIndex++)
                    {
                        sb.AppendLine(string.Format("<td>{0}</td>", Encode(sheet.Cells[rowIndex, colIndex].Value)));
                    }                
                }

                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");

                return sb.ToString();
            }
        } 

        private string Encode(object str)
        {
            return str == null ? "" : System.Web.HttpUtility.HtmlEncode(str.ToString().ReplaceOfficeCrap());
        }
    }
}