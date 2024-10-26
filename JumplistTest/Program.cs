using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JumplistTest
{
    internal static class Program
    {
        public const int WM_CUSTOMMESSAGE = 0x0400 + 100;
        public const int WM_COPYDATA = 0x004A;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool loadMainForm = true;

            if (args.Length > 0)
            {
                // Handle command-line arguments here, such as `/open` or `/settings`
                var arg = args[0];

                if (arg.ToLower().StartsWith("/open"))
                {
                    var appId = arg.Split('=');
                    if (appId.Length == 2)
                    {
                        MessageBox.Show($"Opening file with ID: {appId[1]}");

                        SendMessageToAnotherInstance("JumplistTest", $"Open file jumplist item selected with app ID: {appId[1]}");
                        loadMainForm = false;
                    }
                    else
                    {
                        MessageBox.Show("Invalid app ID passed with /open parameter.");
                        loadMainForm = false;
                    }
                }
                else if (arg.ToLower().StartsWith("/settings"))
                {
                    MessageBox.Show("Opening settings");
                    loadMainForm = false;
                }
            }

            if (loadMainForm)
            {
                Application.Run(new Form1());
            }
        }

        //static void SendCustomMessage(string customMessage)
        //{
        //    // Assume targetGUID is known or obtained through some means
            
        //    IntPtr hWndReceiver = FindWindow(null, "JumplistTest");

        //    if (hWndReceiver != IntPtr.Zero)
        //    {
        //        // Prepare the data to send
        //        byte[] sarr = System.Text.Encoding.Unicode.GetBytes(customMessage);
        //        int len = sarr.Length + 2; // +2 for the null terminator

        //        // Allocate memory for the data
        //        COPYDATASTRUCT cds = new COPYDATASTRUCT();
        //        cds.dwData = IntPtr.Zero;
        //        cds.cbData = len;
        //        cds.lpData = Marshal.StringToHGlobalUni(customMessage);

        //        // Send the message
        //        SendMessage(hWndReceiver, WM_COPYDATA, IntPtr.Zero, ref cds);

        //        // Free the allocated memory
        //        Marshal.FreeHGlobal(cds.lpData);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Target instance not found.");
        //    }
        //}

        public static void SendMessageToAnotherInstance(string receiverWindowTitle, string message)
        {
            // Find the handle of the window to which you want to send the message
            IntPtr hWndReceiver = FindWindow(null, receiverWindowTitle);

            if (hWndReceiver != IntPtr.Zero)
            {
                // Prepare the data to send
                byte[] sarr = Encoding.Unicode.GetBytes(message);
                int len = sarr.Length + 2; // +2 for null terminator

                // Allocate memory for the string data
                IntPtr lpData = Marshal.StringToHGlobalUni(message);

                // Fill the COPYDATASTRUCT
                COPYDATASTRUCT cds = new COPYDATASTRUCT();
                cds.dwData = IntPtr.Zero;
                cds.cbData = len;
                cds.lpData = lpData;

                // Allocate unmanaged memory for the COPYDATASTRUCT
                int cdsSize = Marshal.SizeOf(cds);
                IntPtr ptrCds = Marshal.AllocHGlobal(cdsSize);
                try
                {
                    // Marshal the COPYDATASTRUCT into the unmanaged memory
                    Marshal.StructureToPtr(cds, ptrCds, false);

                    // Send the message
                    SendMessage(hWndReceiver, WM_COPYDATA, IntPtr.Zero, ptrCds);
                }
                finally
                {
                    // Free the allocated unmanaged memory
                    Marshal.FreeHGlobal(lpData);
                    Marshal.FreeHGlobal(ptrCds);
                }
            }
            else
            {
                MessageBox.Show("Receiver application not found.");
            }
        }

    }
}
