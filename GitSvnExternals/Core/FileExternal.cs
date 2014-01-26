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
            var parentRemote = GetParentUriString(RemotePath);
            var parentLocal = new Uri(parentRemote).Segments.Last();

            var args = string.Format(@"svn clone -r HEAD {0} .git_externals\{1}", parentRemote, parentLocal);
            var cmd = new CommandWithArgs("git", args);

            runner.Run(cmd, workingDir);
        }

        public override void Link(string workingDir)
        {
            var parentRemote = GetParentUriString(RemotePath);
            var parentLocal = new Uri(parentRemote).Segments.Last();

            var fileName = Path.GetFileName(LocalPath) ?? "";
                
            var link = Path.GetFullPath(Path.Combine(workingDir, LocalPath));
            var target = Path.GetFullPath(Path.Combine(workingDir, ".git_externals", parentLocal, fileName));

            CreateLink(link, target, LinkTypeFlag.File);
        }

        private static string GetParentUriString(Uri uri)
        {
            return uri.AbsoluteUri.Remove(uri.AbsoluteUri.Length - uri.Segments.Last().Length - 1);
        }
    }
}