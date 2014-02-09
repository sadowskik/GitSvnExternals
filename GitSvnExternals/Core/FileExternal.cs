using System;
using System.IO;
using System.Linq;

namespace GitSvnExternals.Core
{
    public class FileExternal : SvnExternal
    {
        public FileExternal(Uri remotePath, string localPath) 
            : base(remotePath, localPath)
        {
        }

        public override void Clone(IRunCommand runner, string workingDir)
        {
            var parentRemote = GetParentUriString();
            var parentLocal = new Uri(parentRemote).AbsolutePath.Replace(@"/", @"\");

            CreateDirIfNotExists(parentLocal, workingDir);

            var args = string.Format(@"svn clone -r HEAD {0} git_externals{1}", parentRemote, parentLocal);
            var cmd = new CommandWithArgs("git", args);

            runner.Run(cmd, workingDir);
        }

        public override void Link(string workingDir)
        {
            var parentRemote = GetParentUriString();
            var parentLocal = new Uri(parentRemote).AbsolutePath.Replace(@"/", @"\");
            var fileName = Path.GetFileName(RemotePath.AbsolutePath);
                
            var link = Path.GetFullPath(Path.Combine(workingDir, LocalPath));
            var target = Path.GetFullPath(Path.Combine(workingDir, "git_externals" + parentLocal, fileName));

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