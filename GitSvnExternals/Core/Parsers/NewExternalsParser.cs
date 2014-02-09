using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitSvnExternals.Core.Parsers
{
    public class NewExternalsParser : IParseExternals
    {
        public SvnExternal ParseLine(string line)
        {
            if (string.IsNullOrEmpty(line))
                return SvnExternal.Empty;

            var columns = line.Split(' ').ToList();

            if (CannotBeParsed(columns))
                return SvnExternal.Empty;

            var externalUri = columns
                .FirstOrDefault(x => Uri.IsWellFormedUriString(x.Substring(1), UriKind.Absolute));

            if (externalUri == null)
                return SvnExternal.Empty;

            var localPath = ExtractLocalPath(columns, externalUri);

            if (IsFile(localPath))
                return new FileExternal(new Uri(TryRemoveSlash(externalUri)), TryRemoveSlash(localPath));

            return new DirectoryExternal(new Uri(TryRemoveSlash(externalUri)), TryRemoveSlash(localPath));
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

        private static string ExtractLocalPath(IList<string> columns, string externalUri)
        {
            var uriIndex = columns.IndexOf(externalUri);
            int pathIndex = uriIndex == 0 ? 1 : 0;

            var localPath = columns[pathIndex];
            return localPath;
        }

        private static bool CannotBeParsed(IReadOnlyList<string> columns)
        {
            return columns.Count != 2
                   || string.IsNullOrEmpty(columns[0])
                   || string.IsNullOrEmpty(columns[1]);
        }
    }
}