using System;

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
        }

        public override void Link(string workingDir)
        {
        }
    }
}