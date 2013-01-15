using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using StructurizerNEW.Domain;
using StructurizerNEW.Extra;

namespace StructurizerNEW
{
    class Program
    {        
        [STAThread]
        static void Main(string[] args)
        {
            var configFilePath = args.FirstOrDefault();

            if(configFilePath != null)
            {
                if (!File.Exists(configFilePath))
                {
                    Console.WriteLine("Specified config file not found: {0}", configFilePath);
                    Console.ReadLine();
                    return;
                }
            }

            Application.EnableVisualStyles();
            Application.Run(new frmMain(configFilePath));                              
        }     
    }
}
