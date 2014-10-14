using System;
using System.Reactive.Linq;
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

            this.Bind(ViewModel, vm => vm.CloneUrl, v => v.cloneUrl.Text);
            this.BindCommand(ViewModel, vm => vm.CloneRepository, v => v.cloneRepository);

            this.WhenAnyValue(x => x.ViewModel.CloneViewModel)
                .Where(vm => vm != null)
                .SelectMany(vm =>
                {
                    var view = new CloneRepositoryView {ViewModel = vm};
                    content.Content = view;
                    return vm.StartClone.ExecuteAsync();
                })
                .Subscribe();
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(ShellViewModel), typeof(ShellView), new PropertyMetadata(default(ShellViewModel)));

        public ShellViewModel ViewModel
        {
            get { return (ShellViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value as ShellViewModel; }
        }
    }
}
