using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using StructurizerNEW.Domain;
using StructurizerNEW.Extra;

namespace StructurizerNEW
{
    public partial class frmMain : Form
    {
        private readonly List<FolderEntity> activeRoots = new List<FolderEntity>();

        private readonly string configPath;

        private FileSystemWatcher fileWatcher;

        public frmMain(string configPath = null)
        {
            InitializeComponent();

            this.configPath = configPath;

            LoadConfig();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void picGitHub_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/bramdemoor/Structurizr");
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            lblModifications.Visible = false;

            foreach (var processingRoot in activeRoots)
            {
                processingRoot.Process();
            }
        }

        private void LoadConfig()
        {
            if(fileWatcher != null)
            {
                fileWatcher.Dispose();
                fileWatcher = null;
            }

            treeView1.Nodes.Clear();
            var applicationNode = treeView1.Nodes.Add("Structurizr");           

            using (var sr = new StreamReader(configPath))
            {
                var configObj = JsonConvert.DeserializeObject<CoreFile>(sr.ReadToEnd());

                FolderEntity newobj = null;

                foreach (var root in configObj.Roots)
                {
                    switch (root.Type)
                    {
                        case "Project":
                            newobj = new Project(new DirectoryInfo(root.Path));

                            var projectNode = applicationNode.Nodes.Add(root.Path);

                            foreach (var chapter in newobj.Children)
                            {
                                var chapterNode = projectNode.Nodes.Add(chapter.Path.Name);

                                foreach (var section in chapter.Children)
                                {
                                    var sectionNode = chapterNode.Nodes.Add(section.Path.Name);    
                                }
                            }

                            break;
                    }

                    CreateFileWatcher(root.Path);

                    activeRoots.Add(newobj);
                }
            }

            applicationNode.ExpandAll();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        /// <summary>
        /// Sets up a FileWatcher, wires up the events and starts watching 
        /// </summary>
        private void CreateFileWatcher(string path)
        {
            fileWatcher = new FileSystemWatcher { Path = path };

            fileWatcher.Changed += (s, e) => OnChangeDected();
            fileWatcher.Created += (s, e) => OnChangeDected();
            fileWatcher.Deleted += (s, e) => OnChangeDected();
            fileWatcher.Renamed += (s, e) => OnChangeDected();

            fileWatcher.EnableRaisingEvents = true;
        }

        private void OnChangeDected()
        {
            lblModifications.Invoke(new MethodInvoker(delegate
                                                          {
                                                              lblModifications.Visible = true;
                                                              lblModifications.Text = "Changes detected";                                                  
                                                          }));

            
        } 
    }
}
