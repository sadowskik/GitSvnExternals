using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using GitSvnExternals.Core;

namespace GitSvnExternals.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        private string _repoPath;
        private ObservableCollection<SvnExternalViewModel> _externals;        
        private SvnExternalViewModel _newExternal;

        public ShellViewModel()
        {
            RepoPath = "Paste your repo path here...";
            
            _newExternal = new SvnExternalViewModel();
            _externals = new ObservableCollection<SvnExternalViewModel>();
        }

        public void GetExternals()
        {
            var manager = new GitSvnExternalsManager(RepoPath, new ConsoleRunner());

            if (!manager.IsGitSvnRepo)
                return;

            manager.Externals
                .Select(MapToModel).ToList()
                .ForEach(x => Externals.Add(x));
        }

        public void CloneAll()
        {
            var manager = new GitSvnExternalsManager(RepoPath, new ConsoleRunner());

            if (!manager.IsGitSvnRepo)
                return;

            var manuallyAdded = Externals.Where(x => x.ManuallyAdded)
                .Select(MapFrom);

            manager.IncludeManualExternals(manuallyAdded);
            manager.CloneAllExternals();
        }

        public void AddNew()
        {
            if (!NewExternal.IsValid)
                return;

            NewExternal.ManuallyAdded = true;
            Externals.Add(NewExternal);

            NewExternal = new SvnExternalViewModel();
        }

        public static SvnExternal MapFrom(SvnExternalViewModel model)
        {
            var remotePath = new Uri(model.RemotePath);
            var localPath = model.LocalPath;

            return model.IsFile
                ? (SvnExternal) new FileExternal(remotePath, localPath)
                : new DirectoryExternal(remotePath, localPath);
        }

        private static SvnExternalViewModel MapToModel(SvnExternal external)
        {
            return new SvnExternalViewModel
            {
                LocalPath = external.LocalPath,
                RemotePath = external.RemotePath.ToString()
            };
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

        public ObservableCollection<SvnExternalViewModel> Externals
        {
            get { return _externals; }
            set
            {
                if (Equals(value, _externals)) return;
                _externals = value;
                NotifyOfPropertyChange(() => Externals);
            }
        }

        public SvnExternalViewModel NewExternal
        {
            get { return _newExternal; }
            set
            {
                if (Equals(value, _newExternal)) return;
                _newExternal = value;
                NotifyOfPropertyChange(() => NewExternal);
            }
        }
    }
}
