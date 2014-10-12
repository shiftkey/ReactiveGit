using System.Windows;
using ReactiveGit.Demo.ViewModels;
using ReactiveUI;

namespace ReactiveGit.Demo.Views
{
    public partial class ShellView : IViewFor<ShellViewModel>
    {
        public ShellView()
        {
            InitializeComponent();

            this.BindCommand(ViewModel, vm => vm.OpenRepository, v => v.openRepository);

            this.OneWayBind(ViewModel, vm => vm.SelectedRepository, v => v.selectedRepository.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.SelectedRepository, v => v.selectedRepository.Visibility,
                vm => vm == null ? Visibility.Collapsed : Visibility.Visible);
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof (ShellViewModel), typeof (ShellView), new PropertyMetadata(default(ShellViewModel)));

        public ShellViewModel ViewModel
        {
            get { return (ShellViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value as ShellViewModel; }
        }
    }
}
