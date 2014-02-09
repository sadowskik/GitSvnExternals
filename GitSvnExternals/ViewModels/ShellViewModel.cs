﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using GitSvnExternals.Core;
using GitSvnExternals.Core.Parsers;
using Microsoft.Win32;

namespace GitSvnExternals.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        private string _repoPath;
        private ObservableCollection<SvnExternalViewModel> _externals;        
        private SvnExternalViewModel _newExternal;
        private SvnExternalViewModel _selectedExternal;

        public ShellViewModel()
        {
            RepoPath = "Paste your repo path here...";
            
            _newExternal = new SvnExternalViewModel();
            _externals = new ObservableCollection<SvnExternalViewModel>();
        }

        public void GetExternals()
        {
            if (!CanGetExternals)
                return;

            var manager = CreateManager();

            var models = manager.Externals.Select(MapToModel).ToList();
            Externals = new ObservableCollection<SvnExternalViewModel>(models);
        }

        private GitSvnExternalsManager CreateManager()
        {
            var parser = new ChainedParser(new IParseExternals[]
            {
                new NewExternalsParser(),
                new OldExternalsParser()
            });

            return new GitSvnExternalsManager(RepoPath, new ConsoleRunner(), parser);
        }

        public bool CanGetExternals
        {
            get
            {
                var manager = CreateManager();
                return manager.IsGitSvnRepo;
            }
        }

        public void CloneAll()
        {
            var manager = CreateManager();

            if (!manager.IsGitSvnRepo)
                return;

            var manuallyAdded = Externals
                .Where(x => x.ManuallyAdded)
                .Select(MapFrom);

            manager.IncludeManualExternals(manuallyAdded);
            manager.CloneAllExternals();
        }

        public bool CanCloneAll
        {
            get { return CanGetExternals; }
        }

        public void AddNew()
        {
            if (!NewExternal.IsValid)
                return;

            NewExternal.ManuallyAdded = true;
            Externals.Add(NewExternal);

            NewExternal = new SvnExternalViewModel();
        }

        public void RemoveSelected()
        {
            if (!CanRemoveSelected)
                return;

            Externals.Remove(SelectedExternal);
        }

        public void LoadFromFile()
        {
            var fileDialog = new OpenFileDialog();            
            if (!fileDialog.ShowDialog().Value)
                return;

            var manager = CreateManager();
            manager.IncludeManualExternals(fileDialog.FileName);

            var models = manager.Externals.Select(MapToModel).ToList();
            Externals = new ObservableCollection<SvnExternalViewModel>(models);
        }

        public bool CanLoadFromFile
        {
            get { return CanGetExternals; }
        }

        public bool CanRemoveSelected
        {
            get
            {
                return SelectedExternal != null 
                    && SelectedExternal.ManuallyAdded;
            }
        }

        private static SvnExternal MapFrom(SvnExternalViewModel model)
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
                RemotePath = external.RemotePath.ToString(),
                IsFile = external is FileExternal                
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
                NotifyOfPropertyChange(() => CanGetExternals);
                NotifyOfPropertyChange(() => CanLoadFromFile);
                NotifyOfPropertyChange(() => CanCloneAll);
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

        public SvnExternalViewModel SelectedExternal
        {
            get { return _selectedExternal; }
            set
            {
                if (Equals(value, _selectedExternal)) return;
                _selectedExternal = value;
                NotifyOfPropertyChange(() => SelectedExternal);
                NotifyOfPropertyChange(() => CanRemoveSelected);
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
