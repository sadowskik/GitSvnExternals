using System;
using System.Runtime.InteropServices;

namespace GitSvnExternals.Core
{
    public abstract class SvnExternal : IEquatable<SvnExternal>
    {        
        protected enum LinkTypeFlag
        {
            File = 0,
            Directory = 1
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, LinkTypeFlag dwFlags);

        public Uri RemotePath { get; private set; }
        public string LocalPath { get; private set; }

        public abstract string CloneDir { get; }

        public static readonly SvnExternal Empty = new EmptyExternal();
        
        protected SvnExternal(Uri remotePath, string localPath)
        {
            RemotePath = remotePath;
            LocalPath = localPath;
        }

        public virtual void Clone(IRunCommand runner, string workingDir)
        {
        }

        public virtual void Link(string workingDir)
        {
        }

        protected virtual bool CreateLink(string link, string target, LinkTypeFlag type)
        {
            return CreateSymbolicLink(link, target, type);
        }

        public bool Equals(SvnExternal other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(RemotePath, other.RemotePath) && string.Equals(LocalPath, other.LocalPath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SvnExternal) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((RemotePath != null ? RemotePath.GetHashCode() : 0)*397) ^ (LocalPath != null ? LocalPath.GetHashCode() : 0);
            }
        }

        public static bool operator ==(SvnExternal left, SvnExternal right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SvnExternal left, SvnExternal right)
        {
            return !Equals(left, right);
        }

        private class EmptyExternal : SvnExternal
        {
            public EmptyExternal()
                : base(null, null)
            {
            }

            public override string CloneDir
            {
                get { return string.Empty; }
            }
        }
    }
}