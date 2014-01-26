using System.Diagnostics;
using System.IO;

namespace GitSvnExternals.Core
{
    public class ConsoleRunner : IRunCommand
    {
        public StreamReader Run(CommandWithArgs commandWithArgs, string workingDir)
        {
            var process = Process.Start(new ProcessStartInfo(commandWithArgs.Command, commandWithArgs.Arguments)
            {
                WorkingDirectory = workingDir,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,                
            });
            
            process.WaitForExit(5000);
            return process.StandardOutput;
        }
    }
}