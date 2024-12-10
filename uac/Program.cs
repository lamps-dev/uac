using System;
using System.Windows.Forms;

namespace uac
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)  // Add args parameter here
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 mainForm = new Form1();
            mainForm.AllowDrop = true;  // Enable drag-drop for the main form

            // Check if files were dropped onto the exe
            if (args.Length > 0)
            {
                string filePath = args[0];
                if (filePath.EndsWith(".exe") || filePath.EndsWith(".bat") || filePath.EndsWith(".vbs"))
                {
                    mainForm.RunAsAdmin(filePath);
                    return;
                }
            }

            Application.Run(mainForm);
        }
    }
}