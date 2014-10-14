using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReactiveGit.Demo.ViewModels;
using ReactiveUI;

namespace ReactiveGit.Demo.Views
{
    /// <summary>
    /// Interaction logic for CloneRepositoryView.xaml
    /// </summary>
    public partial class CloneRepositoryView : IViewFor<CloneRepositoryViewModel>
    {
        public CloneRepositoryView()
        {
            InitializeComponent();
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
