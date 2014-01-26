using System;

namespace GitSvnExternals.Core
{
    public class SvnExternal : IEquatable<SvnExternal>
    {
        public Uri RemotePath { get; private set; }
        public string LocalPath { get; private set; }

        public SvnExternal(Uri remotePath, string localPath)
        {
            RemotePath = remotePath;
            LocalPath = localPath;
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
            if (obj.GetType() != this.GetType()) return false;
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
    }
}