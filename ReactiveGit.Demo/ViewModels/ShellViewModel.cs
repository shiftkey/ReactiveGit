using System.IO;
using System.Reactive.Linq;
using ReactiveUI;

namespace ReactiveGit.Demo.ViewModels
{
    public class ShellViewModel : ReactiveObject
    {
        public ShellViewModel()
        {
            // TODO: user can open a folder on disk
            
            // TODO: if that folder is valid (and is a repository)
            // TODO: we should display options to checkout, push and pull

        }




        string _repositoryPath;
        public string RepositoryPath
        {
            get { return _repositoryPath; }
            set { this.RaiseAndSetIfChanged(ref _repositoryPath, value); }
        }
    }
}
