using System.Collections.Generic;
using System.IO;
using System.Text;
using GitSvnExternals.Core;

namespace GitSvnExternals.Tests
{
    public class FakeRunner : IRunCommand
    {
        public string ReturnedSvnExternals { get; set; }

        public List<string> ExecutedCommands { get; private set; }

        public FakeRunner()
        {
            ExecutedCommands = new List<string>();
        }

        public StreamReader Run(string command, string arguments, string workingDir = "")
        {
            ExecutedCommands.Add(string.Format("{0} {1}", command, arguments));

            byte[] payload = {};

            if (command == "git" && arguments == "svn show-externals")
                payload = Encoding.UTF8.GetBytes(ReturnedSvnExternals);

            return new StreamReader(new MemoryStream(payload));
        }
    }
}