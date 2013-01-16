using System.IO;
using System.Net;
using System.Text;

namespace StructurizerNEW.Extra
{
    public class FtpUpload
    {
        public static void UploadFiles(string ftpServer, string userName, string password, params FileInfo[] files)
        {
            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(userName, password);

                foreach (var file in files)
                {
                    client.UploadFile(ftpServer + "/" + file, "STOR", file.FullName);    
                }                
            }
        }
    }
}