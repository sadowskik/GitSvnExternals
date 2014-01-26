using System.Windows;
using Caliburn.Micro;
using GitSvnExternals.ViewModels;

namespace GitSvnExternals
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Start();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}