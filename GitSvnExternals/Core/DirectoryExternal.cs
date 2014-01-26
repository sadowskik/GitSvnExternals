using System;

namespace GitSvnExternals.Core
{
    public class DirectoryExternal : SvnExternal
    {
        public DirectoryExternal(Uri remotePath, string localPath) 
            : base(remotePath, localPath)
        {
        }

        public override void Clone(IRunCommand runner, string workingDir)
        {
            var args = string.Format(@"svn clone -r HEAD {0} .git_externals\{1}", RemotePath, LocalPath);
            var cmd = new CommandWithArgs("git", args);

            runner.Run(cmd);
        }

        public override void Link(string workingDir)
        {            
        }
    }
}