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
            var absoluteLocal = RemotePath.AbsolutePath.Replace(@"/",@"\");

            CreateDirIfNotExists(absoluteLocal, workingDir);

            var args = string.Format(@"svn clone -r HEAD {0} git_externals{1}", RemotePath, absoluteLocal);
            var cmd = new CommandWithArgs("git", args);

            runner.Run(cmd, workingDir);
        }

        public override void Link(string workingDir)
        {
            var absoluteLocal = RemotePath.AbsolutePath.Replace(@"/", @"\");

            var link = Path.GetFullPath(Path.Combine(workingDir, LocalPath));
            var target = Path.GetFullPath(Path.Combine(workingDir, "git_externals" + absoluteLocal));

            CreateLink(link, target, LinkTypeFlag.Directory);
        }

        private static void CreateDirIfNotExists(string absolutePath, string workingDir)
        {
            var dirToCreate = Path.GetFullPath(Path.Combine(workingDir, "git_externals" + absolutePath));

            if (!Directory.Exists(dirToCreate))
                Directory.CreateDirectory(dirToCreate);
        }
    }
}