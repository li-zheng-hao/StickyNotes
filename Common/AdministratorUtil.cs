using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Management;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace Common
{
    public class AdministratorUtil
    {
        public static void RunAsAdmin(string argment="")
        {
            //Put this code in the main entry point for the application
            // Check if user is NOT admin 
            if (!IsRunningAsAdministrator())
            {
                // Setting up start info of the new process of the same application
                ProcessStartInfo processStartInfo = new ProcessStartInfo(Assembly.GetEntryAssembly().CodeBase);

                // Using operating shell and setting the ProcessStartInfo.Verb to “runas” will let it run as admin
                processStartInfo.UseShellExecute = true;
                processStartInfo.Verb = "runas";
                processStartInfo.Arguments = argment;
                // Start the application as new process
                Process.Start(processStartInfo);

            }

        }


        /// <summary>
        /// Function that check's if current user is in Aministrator role
        /// </summary>
        /// <returns></returns>
        private static bool IsRunningAsAdministrator()
        {
            // Get current Windows user
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();

            // Get current Windows user principal
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(windowsIdentity);

            // Return TRUE if user is in role "Administrator"
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        // kill process by name
        public static void KillProcess(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    clsProcess.Kill();
                }
            }
        }
    }
}
