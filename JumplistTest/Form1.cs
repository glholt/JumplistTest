using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shell;

namespace JumplistTest
{
    public partial class Form1 : Form
    {
        public const int WM_CUSTOMMESSAGE = 0x0400 + 100;
        public const int WM_COPYDATA = 0x004A;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_COPYDATA)
            {
                try
                {
                    // Get the COPYDATASTRUCT struct from lParam
                    COPYDATASTRUCT cds = (COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT));

                    // Read the string from the lpData pointer
                    string receivedString = Marshal.PtrToStringUni(cds.lpData, cds.cbData / 2);

                    MessageBox.Show("Received message: " + receivedString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error receiving message: " + ex.Message);
                }
            }
            base.WndProc(ref m);
        }



        public Form1()
        {
            InitializeComponent();
            this.Text = "JumplistTest";

            if (System.Windows.Application.Current == null)
            {
                new System.Windows.Application(); // Create a new WPF application context
            }

            InitializeJumpList();
        }

        private void InitializeJumpList()
        {
            // Create a new JumpList for this application
            JumpList jumpList = new JumpList();

            // Create a JumpTask for opening a specific file or performing an action
            JumpTask openFileTask = new JumpTask
            {
                Title = "Open File",
                Arguments = "/open=100",
                Description = "Opens a file in the application",
                CustomCategory = "Tasks",
                IconResourcePath = Application.ExecutablePath,
                ApplicationPath = Application.ExecutablePath
            };

            // Create another JumpTask as an example
            JumpTask settingsTask = new JumpTask
            {
                Title = "Settings",
                Arguments = "/settings",
                Description = "Open application settings",
                CustomCategory = "Settings",
                IconResourcePath = Application.ExecutablePath,
                ApplicationPath = Application.ExecutablePath                
            };

            JumpPath recentFilePath = new JumpPath
            {
                Path = @"C:\Users\glhol\Downloads\YouMakeMyDreams.pdf",
                CustomCategory = "Tasks"
            };

            // Add the tasks to the JumpList
            jumpList.JumpItems.Add(recentFilePath);
            jumpList.JumpItems.Add(openFileTask);
            jumpList.JumpItems.Add(settingsTask);
            

            // Apply the JumpList to the application
            JumpList.SetJumpList(System.Windows.Application.Current, jumpList);
        }
    }
}
