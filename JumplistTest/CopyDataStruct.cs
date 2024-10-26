using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JumplistTest
{
    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;    // Any value to identify the data
        public int cbData;       // The size of the data in bytes
        public IntPtr lpData;    // A pointer to the data
    }
}
