using System;
using System.Reactive.Linq;
using Microsoft.Win32;
using ReactiveUI;

namespace ReactiveGit.Demo.ViewModels
{
    public class ShellViewModel : ReactiveObject
    {
        readonly ObservableAsPropertyHelper<CloneRepositoryViewModel> cloneViewModel;

        public ShellViewModel()
        {
            // some default values because lazy
            CloneUrl = "https://github.com/shiftkey/reactivegit.git";

            // setup the option to clone a repository down
            var canCloneRepository = this.WhenAny(x => x.CloneUrl, x => IsValidUri(x.Value));
            CloneRepository = ReactiveCommand.Create(canCloneRepository);

            cloneViewModel = CloneRepository.Select(_ => new CloneRepositoryViewModel(CloneUrl))
                .ToProperty(this, x => x.CloneViewModel);

            // TODO: user can open a folder on disk
            OpenRepository = ReactiveCommand.Create();
            OpenRepository.Subscribe(_ =>
            {
                // how the hell do i get this to choose a directory instead?
            });

            // TODO: if that folder is valid (and is a repository)
            // TODO: we should display options to checkout, push and pull
        }

        private static bool IsValidUri(string x)
        {
            Uri result;
            return Uri.TryCreate(x, UriKind.Absolute, out result);
        }

        public CloneRepositoryViewModel CloneViewModel { get { return cloneViewModel.Value; } }

        RepositoryViewModel _selectedRepository;
        public RepositoryViewModel SelectedRepository
        {
            get { return _selectedRepository; }
            set { this.RaiseAndSetIfChanged(ref _selectedRepository, value); }
        }

        public ReactiveCommand<object> OpenRepository { get; private set; }
        public ReactiveCommand<object> CloneRepository { get; private set; }

        string _cloneUrl;

        public string CloneUrl
        {
            get { return _cloneUrl; }
            set { this.RaiseAndSetIfChanged(ref _cloneUrl, value); }
        }
    }
}
