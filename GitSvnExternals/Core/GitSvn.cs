using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitSvnExternals.Core
{
    public class GitSvn
    {
        private readonly string _repoPath;
        private readonly IRunCommand _commandRunner;

        public GitSvn(string repoPath, IRunCommand commandRunner)
        {
            _repoPath = repoPath;
            _commandRunner = commandRunner;
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
            get
            {
                using (var reader = _commandRunner.Run("git", "svn show-externals", _repoPath))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line))
                            continue;

                        var columns = line.Split(' ');

                        if (columns.Length != 2)
                            continue;

                        if (string.IsNullOrEmpty(columns[0]) || string.IsNullOrEmpty(columns[1]))
                            continue;

                        var externalUri = columns.FirstOrDefault(x => Uri.IsWellFormedUriString(x.Substring(1), UriKind.Absolute));

                        if (externalUri == null)
                            continue;

                        yield return new SvnExternal(new Uri(externalUri.Substring(1)), columns[1]);
                    }
                }
            }
        }
    }
}