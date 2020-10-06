﻿using System;
using System.Collections.Generic;
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

        private static PowerShell GetPowerShell(string script)
        {
            PowerShell powerShell = PowerShell.Create();
            powerShell.AddScript(script, false);
            powerShell.Invoke();
            powerShell.Commands.Clear();

            return powerShell;
        }

        public static async Task<List<string>> GetPlaylistItemData(string script)
        {
            //Maaaan i have to do this whole thing async because i have to await the return value.
            //I mean i do net the PSObject result in order to process everything
            //at least if i want to port the functions from powershell to c#
            var powerShell = GetPowerShell(script);
            powerShell.AddCommand("getPlaylistItemData").AddParameter("url", "PLSrMfDSgltgfvmLgVH1TE9vCq6rRmugOi").AddParameter("range", "1-2");

            List<string> list = new List<string>();
            var results = powerShell.Invoke();
            foreach (PSObject pso in results)
            {
                list.Add(pso.ToString());
            }

            return list;
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
