using System.IO;

namespace GitSvnExternals.Core
{
    public interface IRunCommand
    {
        StreamReader Run(string command, string arguments, string workingDir = "");
    }
}