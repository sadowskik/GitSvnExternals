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
        private readonly IParseExternals _parser;
        private readonly Lazy<IEnumerable<SvnExternal>> _externals;
        
        private readonly List<SvnExternal> _manuallyAdded;

        public GitSvnExternalsManager(string repoPath, IRunCommand commandRunner, IParseExternals parser)
        {
            _repoPath = repoPath;
            _commandRunner = commandRunner;
            _parser = parser;
            _externals = new Lazy<IEnumerable<SvnExternal>>(RetriveExternals);
            _manuallyAdded = new List<SvnExternal>();
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
                return _externals.Value
                    .Concat(_manuallyAdded)
                    .Where(x => x != SvnExternal.Empty);
            }
        }

        private IEnumerable<SvnExternal> RetriveExternals()
        {
            using (var reader = _commandRunner.Run(new CommandWithArgs("git", "svn show-externals"), _repoPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var external = _parser.ParseLine(line);

                    if (external != SvnExternal.Empty)
                        yield return external;
                }
            }
        }
                
        public void Clone(SvnExternal svnExternal)
        {
            var externalsDir = Path.Combine(_repoPath, ".git_externals");

            if (!Directory.Exists(externalsDir))
            {
                var externalsStore = Directory.CreateDirectory(externalsDir);
                externalsStore.Attributes |= FileAttributes.Hidden;
            }

            svnExternal.Clone(_commandRunner, _repoPath);
            svnExternal.Link(_repoPath);
        }

        public void CloneAllExternals()
        {
            foreach (var svnExternal in Externals)
                Clone(svnExternal);
        }

        public void IncludeManualExternals(IEnumerable<SvnExternal> manuallyAdded)
        {
            _manuallyAdded.AddRange(manuallyAdded);
        }
    }
}