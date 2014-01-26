using System.Collections.Generic;
using Caliburn.Micro;
using GitSvnExternals.Core;

namespace GitSvnExternals.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {        
        private IEnumerable<SvnExternal> _externals;
        private string _repoPath;

        public ShellViewModel()
        {
            RepoPath = "Paste your repo path here...";
        }

        public void GetExternals()
        {
            var manager = new GitSvnExternalsManager(RepoPath, new ConsoleRunner());

            if (manager.IsGitSvnRepo)
                Externals = manager.Externals;
        }

        public void CloneAll()
        {
            var manager = new GitSvnExternalsManager(RepoPath, new ConsoleRunner());

            if (!manager.IsGitSvnRepo)
                return;

            Externals = manager.Externals;
            manager.CloneAllExternals();
        }

        public string RepoPath
        {
            get { return _repoPath; }
            set
            {
                if (value == _repoPath) return;
                _repoPath = value;
                NotifyOfPropertyChange(() => RepoPath);
            }
        }

        public IEnumerable<SvnExternal> Externals
        {
            get { return _externals; }
            set
            {
                if (Equals(value, _externals)) return;
                _externals = value;
                NotifyOfPropertyChange(() => Externals);
            }
        }
    }
}
