using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.Visio;

namespace StructurizerNEW.Extra
{
    public class VisioToPngProcessor
    {
        // TODO BDM: Support multiple pages (with png numbering/naming convention)

         public void FindAndFlattenVSDs(string rootDirectory)
         {
             ApplicationClass visapp = null;
             Document doc = null;
             try
             {
                 visapp = new ApplicationClass();
                 visapp.Visible = false;

                 int pngIndex = 0;

                 foreach (var vsdFile in DirSearch(rootDirectory))
                 {
                     try
                     {
                         doc = visapp.Documents.Open(vsdFile);

                         var fileInfo = new FileInfo(vsdFile);
                         visapp.ActivePage.Export(string.Format(fileInfo.Directory.FullName + @"\{0}.png", fileInfo.Name));

                         pngIndex++;
                     }
                     finally
                     {
                         doc.Close();
                     }
                 }
             }
             finally
             {                 
                 visapp.Quit();
             }
         }


         IEnumerable<string> DirSearch(string sDir)
         {
            foreach (string d in Directory.GetDirectories(sDir))
            {
                foreach (string f in Directory.GetFiles(d, "*.vsd"))
                {
                    yield return f;
                }
                foreach (var innerResult in DirSearch(d))
                {
                    yield return innerResult;
                }
            }
         }
    
    }
}