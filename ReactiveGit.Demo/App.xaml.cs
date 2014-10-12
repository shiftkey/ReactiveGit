using System.Windows;
using ReactiveGit.Demo.ViewModels;
using ReactiveGit.Demo.Views;

namespace ReactiveGit.Demo
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var view = new ShellView();
            var viewModel = new ShellViewModel();

            view.ViewModel = viewModel;

            view.Show();
        }
    }
}
