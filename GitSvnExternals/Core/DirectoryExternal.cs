using System;
using System.IO;

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
            var link = Path.GetFullPath(Path.Combine(workingDir, LocalPath));
            var target = Path.GetFullPath(Path.Combine(workingDir, ".git_externals", LocalPath));

            CreateLink(link, target, LinkTypeFlag.Directory);
        }
    }
}