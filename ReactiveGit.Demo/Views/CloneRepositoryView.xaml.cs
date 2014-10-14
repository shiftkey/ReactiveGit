using System.Windows;
using ReactiveGit.Demo.ViewModels;
using ReactiveUI;

namespace ReactiveGit.Demo.Views
{
    public partial class CloneRepositoryView : IViewFor<CloneRepositoryViewModel>
    {
        public CloneRepositoryView()
        {
            InitializeComponent();

            // display the progress messages
            this.OneWayBind(ViewModel, vm => vm.ProgressText, v => v.progressMessage.Text);

            // show the progress bar filling up
            this.OneWayBind(ViewModel, vm => vm.ProgressValue, v => v.progressBar.Value);
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof (CloneRepositoryViewModel), typeof (CloneRepositoryView), new PropertyMetadata(default(CloneRepositoryViewModel)));

        public CloneRepositoryViewModel ViewModel
        {
            get { return (CloneRepositoryViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value as CloneRepositoryViewModel; }
        }
    }
}
