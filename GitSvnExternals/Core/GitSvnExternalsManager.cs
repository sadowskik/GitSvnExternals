using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitSvnExternals.Core
{
    public class GitSvnExternalsManager
    {
        private readonly string _repoPath;
        private readonly IRunCommand _commandRunner;
        private readonly Lazy<IEnumerable<SvnExternal>> _externals;

        public GitSvnExternalsManager(string repoPath, IRunCommand commandRunner)
        {
            _repoPath = repoPath;
            _commandRunner = commandRunner;
            _externals = new Lazy<IEnumerable<SvnExternal>>(RetriveExternals);
        }

        public bool IsGitSvnRepo
        {
            get
            {
                var gitInternalPath = Path.Combine(_repoPath, ".git");
                return Directory.Exists(gitInternalPath);
            }
        }

        public IEnumerable<SvnExternal> Externals
        {
            get { return _externals.Value; }
        }

        private IEnumerable<SvnExternal> RetriveExternals()
        {
            using (var reader = _commandRunner.Run("git", "svn show-externals", _repoPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var external = ParseExternalLine(line);

                    if (external != SvnExternal.Empty)
                        yield return external;
                }
            }
        }

        private static SvnExternal ParseExternalLine(string line)
        {
            if (string.IsNullOrEmpty(line))
                return SvnExternal.Empty;

            var columns = line.Split(' ');

            if (columns.Length != 2)
                return SvnExternal.Empty;

            if (string.IsNullOrEmpty(columns[0]) || string.IsNullOrEmpty(columns[1]))
                return SvnExternal.Empty;

            var externalUri = columns
                .FirstOrDefault(x => Uri.IsWellFormedUriString(x.Substring(1), UriKind.Absolute));

            if (externalUri == null)
                return SvnExternal.Empty;

            var uriIndex = Array.BinarySearch(columns, externalUri);
            int pathIndex = uriIndex == 0 ? 1 : 0;

            return new SvnExternal(new Uri(externalUri.Substring(1)), columns[pathIndex]);
        }
    }
}