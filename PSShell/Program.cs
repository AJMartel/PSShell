using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using RGiesecke.DllExport;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace PSShell
{
    public class WinApp
    {
        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();
        private const int STD_OUTPUT_HANDLE = -11;
        private const int MY_CODE_PAGE = 437;

        public void winshell()
        {
            AllocConsole();
            IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
            FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
            Encoding encoding = System.Text.Encoding.GetEncoding(MY_CODE_PAGE);
            StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);

            Console.Title = "PSShell - rui@deniable.org";
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            string command = null;

            do
            {
                Console.Write("PS > ");
                command = Console.ReadLine();

                try
                {
                    Pipeline pipeline = runspace.CreatePipeline();
                    pipeline.Commands.AddScript(command);
                    pipeline.Commands.Add("Out-String");
                    Collection<PSObject> results = pipeline.Invoke();
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (PSObject obj in results)
                    {
                        stringBuilder.AppendLine(obj.ToString());
                    }
                    Console.Write(stringBuilder.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0}", e.Message);
                }
            } while (command != "exit");

            runspace.Close();
            Environment.Exit(0);
        }
    }

    class Program
    {
        [DllExport("EntryPoint", CallingConvention = CallingConvention.StdCall)]
        public static void EntryPoint()
        {
            PSShell.WinApp app = new PSShell.WinApp();
            app.winshell();
        }

        [DllExport("DllRegisterServer", CallingConvention = CallingConvention.StdCall)]
        public static void DllRegisterServer()
        {
            PSShell.WinApp app = new PSShell.WinApp();
            app.winshell();
        }

        [DllExport("DllUnregisterServer", CallingConvention = CallingConvention.StdCall)]
        public static void DllUnregisterServer()
        {
            PSShell.WinApp app = new PSShell.WinApp();
            app.winshell();
        }
    }
}
