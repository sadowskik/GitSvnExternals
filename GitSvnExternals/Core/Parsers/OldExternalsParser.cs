using System;
using System.Collections.Generic;
using System.IO;

namespace GitSvnExternals.Core.Parsers
{
    public class OldExternalsParser : IParseExternals
    {        
        public SvnExternal ParseLine(string line)
        {
            if (string.IsNullOrEmpty(line))
                return SvnExternal.Empty;

            var columns = line.Split(' ');

            if (CannotBeParsed(columns))
                return SvnExternal.Empty;

            int remoteStartIndex = GetRemoteStartIndex(columns[0]);
            if (remoteStartIndex < 0)
                return SvnExternal.Empty;

            string remotePath = columns[0].Substring(remoteStartIndex);
            string localPath = Path.Combine(columns[0].Substring(0, remoteStartIndex), columns[1]);

            if (IsFile(localPath))
                return new FileExternal(new Uri(TryRemoveSlash(remotePath)), TryRemoveSlash(localPath));

            return new DirectoryExternal(new Uri(TryRemoveSlash(remotePath)), TryRemoveSlash(localPath));            
        }

        private static bool IsFile(string localPath)
        {
            var extension = Path.GetExtension(localPath);

            return extension != null
                   && Path.HasExtension(localPath)
                   && !extension.Equals(Path.GetFileName(localPath));
        }

        private static string TryRemoveSlash(string path)
        {
            return path.Trim('/');
        }

        private static int GetRemoteStartIndex(string external)
        {
            var index = external.IndexOf("svn", StringComparison.Ordinal);

            if (index < 0)
                index = external.IndexOf("http", StringComparison.Ordinal);

            if (index < 0)
                index = external.IndexOf("https", StringComparison.Ordinal);

            return index;
        }

        private static bool CannotBeParsed(IReadOnlyList<string> columns)
        {
            return columns.Count != 2
                   || string.IsNullOrEmpty(columns[0])
                   || string.IsNullOrEmpty(columns[1]);
        }
    }
}