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

            if (Path.HasExtension(localPath) && Path.GetExtension(localPath) != localPath)
                return new FileExternal(new Uri(externalUri.Substring(1)), localPath);

            return new DirectoryExternal(new Uri(externalUri.Substring(1)), localPath);
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