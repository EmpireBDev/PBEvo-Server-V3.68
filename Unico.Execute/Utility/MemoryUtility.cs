using Microsoft.VisualBasic.Devices;
using Plugin.Core;
using Plugin.Core.Enums;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Executable.Utility
{
    public class MemoryUtility
    {
        #region ShellExecute
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);
        #endregion ShellExecute
        public static bool IsAdministrator()
        {
            using (WindowsIdentity Identity = WindowsIdentity.GetCurrent())
            {
                return new WindowsPrincipal(Identity).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
        public static int TotalMemory()
        {
            int Memory = 0;
            try
            {
                ComputerInfo CI = new ComputerInfo();
                ulong mem = ulong.Parse(CI.TotalPhysicalMemory.ToString());
                Memory = Convert.ToInt32(mem / (double)(1024 * 1024));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            return Memory;
        }
        public static double GetMemoryUsage()
        {
            double Memory = 0.0;
            try
            {
                using (Process CurrentProcess = Process.GetCurrentProcess())
                {
                    CurrentProcess.Refresh();
                    Memory = (CurrentProcess.PrivateMemorySize64 / (double)(1024 * 1024));
                    CurrentProcess.Dispose();
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            return Memory;
        }
        public static double GetMemoryUsagePercent()
        {
            double Usages = 0.0;
            try
            {
                Usages = ((GetMemoryUsage() * 100) / TotalMemory());
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            return Usages;
        }
        public static long GetDirectorySize(DirectoryInfo Directory, bool IncludeSubDir)
        {
            long TotalSize = Directory.EnumerateFiles().Sum(File => File.Length);
            if (IncludeSubDir)
            {
                TotalSize += Directory.EnumerateDirectories().Sum(SubDirectory => GetDirectorySize(SubDirectory, true));
            }
            return TotalSize;
        }
        public static bool ClearCache(DirectoryInfo ServerDir)
        {
            try
            {
                foreach (DirectoryInfo SubDirectory in ServerDir.GetDirectories())
                {
                    FileInfo[] Files = ServerDir.GetFiles();
                    foreach (FileInfo Item in Files)
                    {
                        Item.IsReadOnly = false;
                        Item.Delete();
                    }
                    SubDirectory.Delete(true);
                }
                CLogger.CheckDirectorys();
                return true;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return false;
            }
        }
        public static void ShellMode(string Path, string App, string Operation)
        {
            try
            {
                ShellExecute(IntPtr.Zero, Operation, App, Path, null, 1);
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
