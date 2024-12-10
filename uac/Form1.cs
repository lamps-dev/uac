using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Principal;

namespace uac
{
    public partial class Form1 : Form
    {
        private string selectedFilePath;

        public Form1()
        {
            InitializeComponent();
            // Enable drag and drop for the form
            this.AllowDrop = true;
            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;
        }

        public void RunAsAdmin(string filePath)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.FileName = filePath;
                startInfo.Verb = "runas"; // This triggers the UAC prompt
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to launch with admin rights: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Script and Executable Files (*.exe;*.bat;*.vbs)|*.exe;*.bat;*.vbs|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePath = openFileDialog.FileName;
                    RunAsAdmin(selectedFilePath);
                }
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;  // Fixed from DragEffects to DragDropEffects
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            // The error is here - you need to cast the data after checking if it's present
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);  // Changed from GetDataPresent to GetData
            if (files != null && files.Length > 0)
            {
                string filePath = files[0];
                if (filePath.EndsWith(".exe") || filePath.EndsWith(".bat") || filePath.EndsWith(".vbs"))
                {
                    RunAsAdmin(filePath);
                }
            }
        }
    }
}
