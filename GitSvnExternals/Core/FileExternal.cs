using System;
using System.IO;
using System.Linq;

namespace GitSvnExternals.Core
{
    public class FileExternal : SvnExternal
    {
        private readonly string _parentRemote;
        private readonly string _cloneDir;

        public FileExternal(Uri remotePath, string localPath) 
            : base(remotePath, localPath)
        {
            _parentRemote = GetParentUriString();
            _cloneDir = new Uri(_parentRemote).AbsolutePath.Replace(@"/", @"\");
        }

        public override string CloneDir
        {
            get { return _cloneDir; }
        }

        public override void Clone(IRunCommand runner, string workingDir)
        {
            CreateDirIfNotExists(CloneDir, workingDir);

            var args = string.Format(@"svn clone -r HEAD {0} git_externals{1}", _parentRemote, CloneDir);
            var cmd = new CommandWithArgs("git", args);

            runner.Run(cmd, workingDir);
        }

        public override void Link(string workingDir)
        {            
            var fileName = Path.GetFileName(RemotePath.AbsolutePath);
                
            var link = Path.GetFullPath(Path.Combine(workingDir, LocalPath));
            var target = Path.GetFullPath(Path.Combine(workingDir, "git_externals" + CloneDir, fileName));

            var linkParentDir = Directory.GetParent(link);
            if (!linkParentDir.Exists)
                linkParentDir.Create();

            CreateLink(link, target, LinkTypeFlag.File);
        }

        private string GetParentUriString()
        {            
            var charactersToRemove = RemotePath.AbsoluteUri.Length - RemotePath.Segments.Last().Length - 1;
            var parentFolder = RemotePath.AbsoluteUri.Remove(charactersToRemove);

            return parentFolder;
        }

        private static void CreateDirIfNotExists(string absolutePath, string workingDir)
        {
            var dirToCreate = Path.GetFullPath(Path.Combine(workingDir, "git_externals" + absolutePath));

            if (!Directory.Exists(dirToCreate))
                Directory.CreateDirectory(dirToCreate);
        }
    }
}