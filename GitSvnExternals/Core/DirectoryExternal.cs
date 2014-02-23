using System;
using System.IO;

namespace GitSvnExternals.Core
{
    public class DirectoryExternal : SvnExternal
    {
        private readonly string _cloneDir;

        public DirectoryExternal(Uri remotePath, string localPath) 
            : base(remotePath, localPath)
        {
            _cloneDir = RemotePath.AbsolutePath.Replace(@"/", @"\");
        }

        public override string CloneDir
        {
            get { return _cloneDir; }
        }

        public override void Clone(IRunCommand runner, string workingDir)
        {            
            CreateDirIfNotExists(CloneDir, workingDir);

            var args = string.Format(@"svn clone -r HEAD {0} git_externals{1}", RemotePath, CloneDir);
            var cmd = new CommandWithArgs("git", args);

            runner.Run(cmd, workingDir);
        }

        public override void Link(string workingDir)
        {            
            var link = Path.GetFullPath(Path.Combine(workingDir, LocalPath));
            var target = Path.GetFullPath(Path.Combine(workingDir, "git_externals" + CloneDir));

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