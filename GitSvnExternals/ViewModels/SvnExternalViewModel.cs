using Caliburn.Micro;

namespace GitSvnExternals.ViewModels
{
    public class SvnExternalViewModel : PropertyChangedBase
    {
        private string _remotePath;
        private string _localPath;
        private bool _isFile;

        public string RemotePath
        {
            get { return _remotePath; }
            set
            {
                if (value == _remotePath) return;
                _remotePath = value;
                NotifyOfPropertyChange(() => RemotePath);
            }
        }

        public string LocalPath
        {
            get { return _localPath; }
            set
            {
                if (value == _localPath) return;
                _localPath = value;
                NotifyOfPropertyChange(() => LocalPath);
            }
        }

        public bool IsFile
        {
            get { return _isFile; }
            set
            {
                if (value.Equals(_isFile)) return;
                _isFile = value;
                NotifyOfPropertyChange(() => IsFile);
                NotifyOfPropertyChange(() => Type);
            }
        }

        public string Type
        {
            get { return IsFile ? "File" : "Dir"; }
        }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(LocalPath)
                    && !string.IsNullOrEmpty(RemotePath);
            }
        }

        public bool ManuallyAdded { get; set; }
    }
}