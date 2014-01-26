using System;
using System.Windows;
using Caliburn.Micro;
using GitSvnExternals.ViewModels;

namespace GitSvnExternals
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {        
        public Bootstrapper()
        {
            Start();
        }
        
        protected override object GetInstance(Type serviceType, string key)
        {
            return serviceType == typeof (IWindowManager)
                ? new MetroWindowManager()
                : base.GetInstance(serviceType, key);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}