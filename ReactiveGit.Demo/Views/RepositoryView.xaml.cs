using System.Windows;
using ReactiveGit.Demo.ViewModels;
using ReactiveUI;

namespace ReactiveGit.Demo.Views
{
    public partial class RepositoryView : IViewFor<RepositoryViewModel>
    {
        public RepositoryView()
        {
            InitializeComponent();

            // display the progress messages
            this.OneWayBind(ViewModel, vm => vm.ProgressText, v => v.progressMessage.Text);

            // show the progress bar filling up
            this.OneWayBind(ViewModel, vm => vm.ProgressValue, v => v.progressBar.Value);
            this.OneWayBind(ViewModel, vm => vm.IsExecuting, v => v.progressBar.Visibility);
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof (RepositoryViewModel), typeof (RepositoryView), new PropertyMetadata(default(RepositoryViewModel)));

        public RepositoryViewModel ViewModel
        {
            get { return (RepositoryViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value as RepositoryViewModel; }
        }
    }
}
