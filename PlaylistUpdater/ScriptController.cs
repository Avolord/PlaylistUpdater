using System;
using System.IO;
using System.Management.Automation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace PlaylistUpdater
{
    class ScriptController
    {
        public static string LoadScriptFromFile(string path)
        {
            if(File.Exists(path)) {
                return File.ReadAllText(path);
            }
            return "";
        }

        public static void RunCommand(Action<PowerShell> func, string script)
        {
            Task.Factory.StartNew(() =>
            {
                using (var powershell = PowerShell.Create())
                {
                    powershell.AddScript(script, false);

                    powershell.Invoke();

                    powershell.Commands.Clear();

                    func(powershell);
                }
            });
        }

        public static void PowershellJob(PowerShell powershell)
        {
            powershell.AddCommand("getPlaylistItemData").AddParameter("url", "PLSrMfDSgltgfvmLgVH1TE9vCq6rRmugOi").AddParameter("range", "1-2");

            var results = powershell.Invoke();
            foreach (PSObject pso in results)
            {
                Console.WriteLine(pso);
            }
        }
    }
}


//Console.WriteLine(results[0]);
