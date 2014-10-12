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
