using ReactiveUI;

namespace ReactiveGit.Demo.ViewModels
{
    public class ShellViewModel : ReactiveObject
    {





        string _repositoryPath;
        public string RepositoryPath
        {
            get { return _repositoryPath; }
            set { this.RaiseAndSetIfChanged(ref _repositoryPath, value); }
        }
    }
}
