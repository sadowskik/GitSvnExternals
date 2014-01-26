using System.IO;

namespace GitSvnExternals.Core
{
    public interface IRunCommand
    {
        StreamReader Run(CommandWithArgs commandWithArgs, string workingDir ="");
    }

    public class CommandWithArgs
    {
        public CommandWithArgs(string command, string arguments)
        {
            Command = command;
            Arguments = arguments;            
        }

        public string Command { get; private set; }

        public string Arguments { get; private set; }        
    }
}