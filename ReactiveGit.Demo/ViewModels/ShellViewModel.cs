using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace ReactiveGit.Demo.ViewModels
{
    public class ShellViewModel : ReactiveObject
    {
        readonly ObservableAsPropertyHelper<CloneRepositoryViewModel> cloneViewModel;
        readonly ObservableAsPropertyHelper<RepositoryViewModel> existingRepository;

        public ShellViewModel()
        {
            // some default values because lazy
            CloneUrl = "https://github.com/shiftkey/reactivegit.git";

            // setup the option to clone a repository down
            CloneRepository = ReactiveCommand.CreateAsyncObservable(
                this.WhenAny(x => x.CloneUrl, x => IsValidUri(x.Value)),
                _ => ObservableFolderPicker.SelectFolder());

            cloneViewModel = CloneRepository
                .Select(path => new CloneRepositoryViewModel(CloneUrl, path))
                .ToProperty(this, x => x.CloneViewModel);

            OpenRepository = ReactiveCommand.CreateAsyncObservable(
                _ => ObservableFolderPicker.SelectFolder());

            existingRepository = OpenRepository
                .Where(IsGitRepository)
                .Select(path => new RepositoryViewModel(path))
                .ToProperty(this, x => x.ExistingRepository);
        }

        bool IsGitRepository(string directory)
        {
            // TODO: validate this path is a repository
            return true;
        }

        private static bool IsValidUri(string x)
        {
            Uri result;
            return Uri.TryCreate(x, UriKind.Absolute, out result);
        }

        public CloneRepositoryViewModel CloneViewModel
        {
            get { return cloneViewModel.Value; }
        }

        public RepositoryViewModel ExistingRepository
        {
            get { return existingRepository.Value; }
        }

        public ReactiveCommand<string> OpenRepository { get; private set; }
        public ReactiveCommand<string> CloneRepository { get; private set; }

        string cloneUrl;
        public string CloneUrl
        {
            get { return cloneUrl; }
            set { this.RaiseAndSetIfChanged(ref cloneUrl, value); }
        }
    }
}
