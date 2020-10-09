
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;

namespace PlaylistUpdater
{

    static class CommandHandler
    {
        public static int PROCESS_TIMEOUT { get; set; } = 1000000;
        public static bool IsHidden { get; set; } = true;
        public static DataReceivedEventHandler DataReceivedEventHandler { get; set; } =
            (sender, eventArgs) => { Console.WriteLine(eventArgs.Data); };

        public static string Execute(string filename, string arguments)
            {
                string output = "";
                using (Process process = new Process())
                {
                    process.StartInfo = getStartInfo(filename, arguments, true);

                    process.Start();
                    output = process.StandardOutput.ReadToEnd();
                    var processExited = process.WaitForExit(PROCESS_TIMEOUT);

                    //handle a timeout
                    if (processExited == false) // we timed out...
                    {
                        throw new Exception("ERROR: Process took too long to finish");
                    }
                }
                return output;
        }

        public static void Execute(string filename, string arguments, DataReceivedEventHandler dataReceivedEventHandler)
        {
            using (Process process = new Process())
            {
                process.StartInfo = getStartInfo(filename, arguments, true);

                process.EnableRaisingEvents = true;
                process.OutputDataReceived += dataReceivedEventHandler;
                process.Start();
                process.BeginOutputReadLine();
                var processExited = process.WaitForExit(PROCESS_TIMEOUT);
                process.CancelOutputRead();

                //handle a timeout
                if (processExited == false) // we timed out...
                {
                    throw new Exception("ERROR: Process took too long to finish");
                }
            }
        }

        public static string Cmd(string command)
        {
            Execute("CMD.exe", "/c " + command, DataReceivedEventHandler);
            return "";
        }

        private static ProcessStartInfo getStartInfo(string filename, string arguments, bool redirectOutput = true)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = filename;
            startInfo.Arguments = arguments;
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = IsHidden ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal;
            startInfo.RedirectStandardOutput = redirectOutput;
            startInfo.CreateNoWindow = true;

            return startInfo;
        }

    }
}
