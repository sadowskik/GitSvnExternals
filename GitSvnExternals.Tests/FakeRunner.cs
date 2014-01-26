using System.Collections.Generic;
using System.IO;
using System.Text;
using GitSvnExternals.Core;

namespace GitSvnExternals.Tests
{
    public class FakeRunner : IRunCommand
    {
        public string ReturnedSvnExternals { get; set; }

        public List<CommandWithArgs> ExecutedCommands { get; private set; }

        public FakeRunner()
        {
            ExecutedCommands = new List<CommandWithArgs>();
        }
        
        public StreamReader Run(CommandWithArgs commandWithArgs, string workingDir)
        {
            ExecutedCommands.Add(commandWithArgs);

            byte[] payload = {};

            if (commandWithArgs.Command == "git" && commandWithArgs.Arguments == "svn show-externals")
                payload = Encoding.UTF8.GetBytes(ReturnedSvnExternals);

            return new StreamReader(new MemoryStream(payload));
        }
    }    
}