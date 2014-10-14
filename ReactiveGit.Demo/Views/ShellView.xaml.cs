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

            // wireup the data necessary for the clone
            this.Bind(ViewModel, vm => vm.CloneUrl, v => v.cloneUrl.Text);
            this.BindCommand(ViewModel, vm => vm.CloneRepository, v => v.cloneRepository);

            // hide the clone panel after we have kicked off the clone
            this.WhenAnyValue(x => x.ViewModel.CloneViewModel)
                .Select(vm => vm == null)
                .BindTo(this, v => v.clonePanel.Visibility);

            // once setup, trigger the clone operation
            this.WhenAnyValue(x => x.ViewModel.CloneViewModel)
                .Where(vm => vm != null)
                .SelectMany(vm =>
                {
                    var view = new CloneRepositoryView { ViewModel = vm };
                    content.Content = view;
                    return vm.Clone.ExecuteAsync();
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
