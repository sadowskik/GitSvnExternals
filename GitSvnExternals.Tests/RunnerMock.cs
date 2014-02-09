using System.Collections.Generic;
using System.IO;
using System.Text;
using GitSvnExternals.Core;

namespace GitSvnExternals.Tests
{
    public class RunnerMock : IRunCommand
    {
        public string ReturnedSvnExternals { get; set; }

        public List<CommandWithArgs> ExecutedCommands { get; private set; }

        public RunnerMock()
        {
            ExecutedCommands = new List<CommandWithArgs>();
            ReturnedSvnExternals = string.Empty;
        }

        public StreamReader Run(CommandWithArgs commandWithArgs, string workingDir)
        {
            ExecutedCommands.Add(commandWithArgs);

            var payload = Encoding.UTF8.GetBytes(ReturnedSvnExternals);
            return new StreamReader(new MemoryStream(payload));
        }
    }    
}