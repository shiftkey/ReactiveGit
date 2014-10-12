using System;
using Microsoft.Win32;
using ReactiveUI;

namespace ReactiveGit.Demo.ViewModels
{
    public class ShellViewModel : ReactiveObject
    {
        public ShellViewModel()
        {
            // TODO: user can open a folder on disk
            
            OpenRepository = ReactiveCommand.Create();
            OpenRepository.Subscribe(_ =>
            {
                var dialog = new OpenFileDialog();
                // how the hell do i get this to choose a directory instead?
            });

            // TODO: if that folder is valid (and is a repository)
            // TODO: we should display options to checkout, push and pull
        }

        RepositoryViewModel _selectedRepository;
        public RepositoryViewModel SelectedRepository
        {
            get { return _selectedRepository; }
            set { this.RaiseAndSetIfChanged(ref _selectedRepository, value); }
        }

        public ReactiveCommand<object> OpenRepository { get; private set; }
    }
}
